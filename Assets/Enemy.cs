using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float Health
    {
        set
        {
            health = value;
            if(health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }



    public float health = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine("FlashRed");
        Health -= damage;
    }

    IEnumerator FlashRed()
    {
        //print("made it");
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.material.color = Color.white;
        yield return null;
    }

/*    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void Defeated()
    {
        Destroy(gameObject);
    }
}
