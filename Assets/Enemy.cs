using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    SpriteRenderer spriteRenderer;

    [SyncVar]
    Color color;

    
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


    [SyncVar]
    public float health = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [Client]
    public void TakeDamage(float damage)
    {
        if (!hasAuthority) return;
        CmdTakeDamage(damage);
    }

    [Command]
    private void CmdTakeDamage(float damage)
    {
        StartCoroutine("FlashRed");
        Health -= damage;
        RpcTakeDamage(damage);
    }

    [ClientRpc]
    private void RpcTakeDamage(float damage)
    {
        StartCoroutine("FlashRed");
        Health -= damage;
    }

    IEnumerator FlashRed()
    {
        //print("made it");
        color = Color.red;
        spriteRenderer.material.color = color;
        yield return new WaitForSeconds(.2f);
        color = Color.white;
        spriteRenderer.material.color = color;
        yield return null;
    }

/*    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void Defeated()
    {
        //Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
