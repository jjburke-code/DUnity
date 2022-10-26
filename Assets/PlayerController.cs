using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private float collisionOffset = float.Epsilon;
    public ContactFilter2D movementFilter;
    bool moving = false;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;
    int lastDirection;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
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
                    animator.SetBool("IsMovingLeft", false);
                    animator.SetBool("IsMovingDown", false);
                    animator.SetBool("IsMovingUp", false);
                }
                else if (movementInput.x < 0)
                {
                    animator.SetBool("IsMovingRight", false);
                    animator.SetBool("IsMovingDown", false);
                    animator.SetBool("IsMovingUp", false);
                }
                else if (movementInput.y > 0)
                {
                    animator.SetBool("IsMovingDown", false);
                    animator.SetBool("IsMovingLeft", false);
                    animator.SetBool("IsMovingRight", false);
                }
                else if (movementInput.y < 0)
                {
                    animator.SetBool("IsMovingUp", false);
                    animator.SetBool("IsMovingLeft", false);
                    animator.SetBool("IsMovingRight", false);
                }
            }
            else
            {
                animator.SetBool("IsMovingLeft", false);
                animator.SetBool("IsMovingRight", false);
                animator.SetBool("IsMovingDown", false);
                animator.SetBool("IsMovingUp", false);
            }
            
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions 
                movementFilter, // the settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // the amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                if (moving)
                {
                    if (direction.x > 0 || direction.x > 0 && (direction.y != 0))
                    {
                        animator.SetBool("IsMovingRight", true);
                        lastDirection = 0;
                    }
                    if (direction.x < 0 || direction.x < 0 && (direction.y != 0))
                    {
                        animator.SetBool("IsMovingLeft", true);
                        lastDirection = 1;

                    }
                    if (direction.y > 0 && direction.x == 0)
                    {
                        animator.SetBool("IsMovingUp", true);

                    }
                    if (direction.y < 0 && direction.x == 0)
                    {
                        animator.SetBool("IsMovingDown", true);

                    }
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
        moving = true;
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    //end of movement code

    //start of attack code

    void OnPunch()
    {
        if (lastDirection == 0)
        {
            animator.SetTrigger("AttackRight");
        }
        else if(lastDirection == 1)
        {
            animator.SetTrigger("AttackLeft");

        }
    }

}
