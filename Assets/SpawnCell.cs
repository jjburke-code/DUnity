using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCell : NetworkBehaviour
{
    public GameObject cell;
    readonly NetworkConnection conn;
    public MouseController mouseController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSpawnCell()
    {
        print("made it");
        GameObject cellSpawn = Instantiate(cell, mouseController.currentMousePosition, Quaternion.identity);
        //Enemy enemy1 = cellSpawn.GetComponent<Enemy>();
        NetworkServer.Spawn(cellSpawn, conn);
    }
}
