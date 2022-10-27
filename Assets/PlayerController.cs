using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private float currSpeed;
    private float collisionOffset = float.Epsilon;
    public ContactFilter2D movementFilter;
    bool moving = false;
    bool canFly = true; // will eventually be able to set via skill
    bool flying = false;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;
    int lastDirection;

    Animator animator;

    public BasicAttack basicAttack;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        animator.SetBool("IsFlying", flying);
        //print(moving);
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool sucess = TryMove(movementInput);

                if (!sucess)
                {
                    sucess = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!sucess)
                {
                    sucess = TryMove(new Vector2(0, movementInput.y));
                }

                if (movementInput.x > 0)
                {
                    DisableRestMoveThisWay("right");
                }
                else if (movementInput.x < 0)
                {
                    DisableRestMoveThisWay("left");
                }
                else if (movementInput.y > 0)
                {
                    DisableRestMoveThisWay("up");
                }
                else if (movementInput.y < 0)
                {
                    DisableRestMoveThisWay("down");
                }
            }
            else
            {
                DisableRestMoveThisWay("all");
            }
        }

    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            if (flying)
            {
                currSpeed = (moveSpeed * 2f) * playerStats.player.speed; // maybe also multiply by fly skill needs work on Equation
              //  print(currSpeed);
            }
            else{
                currSpeed = moveSpeed + playerStats.player.speed;
               // print(currSpeed);

            }
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions 
                movementFilter, // the settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the cast is finished
                currSpeed * Time.fixedDeltaTime + collisionOffset); // the amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * currSpeed * Time.fixedDeltaTime);
                    if (direction.x > 0 || direction.x > 0 && (direction.y != 0))
                    {
                        if (flying)
                        {
                            animator.SetBool("IsFlyingRight",true);
                        }
                        else
                        {
                            animator.SetBool("IsMovingRight", true);
                        }
                        
                        lastDirection = 0;
                    }
                    if (direction.x < 0 || direction.x < 0 && (direction.y != 0))
                    {
                        
                        if (flying)
                        {
                            animator.SetBool("IsFlyingLeft",true);
                        }
                        else
                        {
                            animator.SetBool("IsMovingLeft", true);
                        }
                        
                        lastDirection = 1;

                    }
                    if (direction.y > 0 && direction.x == 0)
                    {
                        if (flying)
                        {
                            animator.SetBool("IsFlyingUp",true);
                        }
                        else
                        {
                            animator.SetBool("IsMovingUp", true);
                        }
                        
                        lastDirection = 2;
                    }
                    if (direction.y < 0 && direction.x == 0)
                    {
                        if (flying)
                        {
                            animator.SetBool("IsFlyingDown",true);
                        }
                        else
                        {
                            animator.SetBool("IsMovingDown", true);
                        }
                        
                        lastDirection = 3;
                    }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    // probably should make this rebindable, but thats a problem for later
    void OnPressR()
    {
        //print("Pressed R");
        if (canFly && !flying)
        {
            //print("Can FLy");
            LayerMask mask = LayerMask.GetMask("Flight", "Bounds");
            gameObject.layer = 6;

            movementFilter.SetLayerMask(mask);
            flying = true;
            animator.SetTrigger("Flying");
            transform.position = transform.position + new Vector3(0f, .1f);
        }
        else if(flying)
        {
            gameObject.layer = 9;
            movementFilter.NoFilter();
            flying = false;
            animator.SetTrigger("Walking");
            transform.position = transform.position + new Vector3(0f, -.1f);
        }
    }

    private void DisableRestMoveThisWay(string direction)
    {
        switch (direction){
            case "right":
                animator.SetBool("IsMovingLeft", false);
                animator.SetBool("IsMovingDown", false);
                animator.SetBool("IsMovingUp", false);

                animator.SetBool("IsFlyingLeft", false);
                animator.SetBool("IsFlyingUp", false);
                animator.SetBool("IsFlyingDown", false);
                break;
            case "left":
                animator.SetBool("IsMovingRight", false);
                animator.SetBool("IsMovingDown", false);
                animator.SetBool("IsMovingUp", false);

                animator.SetBool("IsFlyingUp", false);
                animator.SetBool("IsFlyingDown", false);
                animator.SetBool("IsFlyingRight", false);
                break;
            case "up":
                animator.SetBool("IsMovingDown", false);
                animator.SetBool("IsMovingLeft", false);
                animator.SetBool("IsMovingRight", false);

                animator.SetBool("IsFlyingDown", false);
                animator.SetBool("IsFlyingRight", false);
                animator.SetBool("IsFlyingLeft", false);
                break;
            case "down":
                animator.SetBool("IsMovingUp", false);
                animator.SetBool("IsMovingLeft", false);
                animator.SetBool("IsMovingRight", false);

                animator.SetBool("IsFlyingRight", false);
                animator.SetBool("IsFlyingLeft", false);
                animator.SetBool("IsFlyingUp", false);
                break;
            case "all":
                animator.SetBool("IsMovingLeft", false);
                animator.SetBool("IsMovingRight", false);
                animator.SetBool("IsMovingDown", false);
                animator.SetBool("IsMovingUp", false);

                animator.SetBool("IsFlyingRight", false);
                animator.SetBool("IsFlyingLeft", false);
                animator.SetBool("IsFlyingUp", false);
                animator.SetBool("IsFlyingDown", false);
                break;
        }
    }
    //end of movement code

    //start of attack code

    void OnPunch()
    {
        basicAttack.damage = 10f * playerStats.player.strength; // dunno actual eq
        switch (lastDirection)
        {
            case 0:
                animator.SetTrigger("AttackRight");
                basicAttack.AttackRight();
                break;
            case 1:
                animator.SetTrigger("AttackLeft");
                basicAttack.AttackLeft();
                break;
            case 2:
                animator.SetTrigger("AttackUp");
                basicAttack.AttackUp();
                break;
            case 3:
                animator.SetTrigger("AttackDown");
                basicAttack.AttackDown();
                break;
        }
        
    }

    public void EndAttack()
    {
        basicAttack.AttackStop();
    }

    // end of attack code
}
