using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{

    public float damage = 1f;

    public BoxCollider2D fistCollider;
    Vector2 rightAttackOffset;
    // Start is called before the first frame update
    void Start()
    {
        rightAttackOffset = transform.position;
    }

/*    // Update is called once per frame
    void Update()
    {
       
    }*/

    public void AttackRight()
    {
        //print("Attacked Right");
        fistCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        //print("Attacked Left");
        fistCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1f, rightAttackOffset.y);
    }

    public void AttackUp()
    {
       // print("Attacked Up");
        fistCollider.enabled = true;
        transform.localPosition = new Vector3(0f, .1f);
    }

    public void AttackDown()
    {
        //print("Attacked Down");
        fistCollider.enabled = true;
        transform.localPosition = new Vector3(0f, -.1f);
    }

    public void AttackStop()
    {
        fistCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            //deal damage
            Enemy enemy = collision.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.Health -= damage;
            }
        }
    }
}
