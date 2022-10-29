using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 2f;
    private float currSpeed;
    private float collisionOffset = float.Epsilon;
    public ContactFilter2D movementFilter;
    bool canFly = true; // will eventually be able to set via skill
    bool flying = false;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); 
    bool canMove = true;
    int lastDirection;

    Animator animator;
    NetworkAnimator networkAnimator;
    
    public MouseController mouseController;

    //public BasicAttack basicAttack;
    public PlayerStats playerStats;

    [SerializeField] private float damage;

    // Start is called before the first frame update 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        currSpeed = moveSpeed;
        damage = 10f * playerStats.player.strength;

        //rightAttackOffset = fistHitbox.transform.position;

    }

    [Client]
    private void FixedUpdate()
    {
        if (!hasAuthority) return;
        damage = 10f * playerStats.player.strength;
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
            else
            {
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
                        animator.SetBool("IsFlyingRight", true);
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
                        animator.SetBool("IsFlyingLeft", true);
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
                        animator.SetBool("IsFlyingUp", true);
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
                        animator.SetBool("IsFlyingDown", true);
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

    [Client]
    void OnMove(InputValue movementValue)
    {
        if (!hasAuthority) return;
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

    [Client]
    // probably should make this rebindable, but thats a problem for later
    void OnPressR()
    {
        if (!hasAuthority) return;
        CmdOnPressR();
    }

    [Command]
    private void CmdOnPressR()
    {
        RpcOnPressR();
    }

    [ClientRpc]
    private void RpcOnPressR()
    {
        if (!isLocalPlayer) return;
        //print("Pressed R");
        if (canFly && !flying)
        {
            //print("Can FLy");
            LayerMask mask = LayerMask.GetMask("Flight", "Bounds");
            gameObject.layer = 6;

            movementFilter.SetLayerMask(mask);
            flying = true;
            //animator.SetTrigger("Flying");
            networkAnimator.SetTrigger("Flying");
            transform.position = transform.position + new Vector3(0f, .1f);
        }
        else if (flying)
        {
            gameObject.layer = 9;
            movementFilter.NoFilter();
            flying = false;
            //animator.SetTrigger("Walking");
            networkAnimator.SetTrigger("Walking");
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

    [Client]
    void OnPunch()
    {
        if (!hasAuthority) return;
        CmdOnPunch();
    }

    [Command]
    private void CmdOnPunch()
    {
        RpcOnPunch();
    }

    

    [ClientRpc]
    private void RpcOnPunch()
    {
        if (!isLocalPlayer) return;
        ; // dunno actual eq
        switch (lastDirection)
        {
            case 0:
                //animator.SetTrigger("AttackRight");
                networkAnimator.SetTrigger("AttackRight");
                AttackRight();
                break;
            case 1:
                //animator.SetTrigger("AttackLeft");
                networkAnimator.SetTrigger("AttackLeft");
                AttackLeft();
                break;
            case 2:
                //animator.SetTrigger("AttackUp");
                networkAnimator.SetTrigger("AttackUp"); 
                AttackUp();
                break;
            case 3:
                //animator.SetTrigger("AttackDown");
                networkAnimator.SetTrigger("AttackDown");
                AttackDown();
                break;
        }
    }

    public void EndAttack()
    {
        AttackStop();
    }

    

    public BoxCollider2D fistCollider;

    [Client]
    public void AttackRight()
    {
        //print("Attacked Right");
        /*fistCollider.enabled = true;
        fistCollider.transform.localPosition = new Vector3(0.089f, 0f);*/
        if(!hasAuthority) return;
        CmdAttack("right");
    }


    [Client]

    public void AttackLeft()
    {
        //print("Attacked Left");
        /*fistCollider.enabled = true;
        fistCollider.transform.localPosition = new Vector3(0.089f * -1f, 0f);*/
        if (!hasAuthority) return;
        CmdAttack("left");
    }

    [Client]
    public void AttackUp()
    {
        // print("Attacked Up");
        /*fistCollider.enabled = true;
        fistCollider.transform.localPosition = new Vector3(0f, .1f);*/
        if (!hasAuthority) return;
        CmdAttack("left");
    }

    [Client]
    public void AttackDown()
    {
        //print("Attacked Down");
        /*fistCollider.enabled = true;
        fistCollider.transform.localPosition = new Vector3(0f, -.1f);*/
        if (!hasAuthority) return;
        CmdAttack("left");
    }

    [Client]
    public void AttackStop()
    {
        fistCollider.enabled = false;
    }

    [Command]
    private void CmdAttack(string direction)
    {
        RpcAttack(direction);
    }

    [ClientRpc]
    private void RpcAttack(string direction)
    {
        switch (direction)
        {
            case "right":
                fistCollider.enabled = true;
                fistCollider.transform.localPosition = new Vector3(0.089f, 0f);
                break;
            case "left":
                fistCollider.enabled = true;
                fistCollider.transform.localPosition = new Vector3(0.089f * -1f, 0f);
                break;
            case "up":
                fistCollider.enabled = true;
                fistCollider.transform.localPosition = new Vector3(0f, .1f);
                break;
            case "down":
                fistCollider.enabled = true;
                fistCollider.transform.localPosition = new Vector3(0f, -.1f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //deal damage
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(10f);
                print(damage + " dmg");
            }
        }
    }

    private void CmdHitboxHit()
    {

    }

    private void RpcHitboxHit()
    {

    }

    // end of attack code

    // Per button Code

    public GameObject cell;

    [Client]
    void OnPressC()
    {
        if (!isLocalPlayer) return;
        CmdOnPressC();
    }

    [Command]
    private void CmdOnPressC()
    {
        RpcOnPressC();
    }

    [ClientRpc]
    private void RpcOnPressC()
    {
        GameObject cellSpawn = Instantiate(cell, mouseController.currentMousePosition, Quaternion.identity);
        NetworkServer.Spawn(cellSpawn);
    }

    // end of dev code
}
