using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public struct Stats
    {
        public float strength;
        public float durability;
        public float speed;
        public float force;
        public float resistance;
        public float accuracy;
        public float reflex;

        public float energy;
        public float regen;
        public float recovery;
        public float anger;
    }

    public Stats player;
    // Start is called before the first frame update
    void Start()
    {
        player = new Stats();
        int[] startingStats = Enumerable.Repeat(1, 11).ToArray();
        SetStats(startingStats);
    }

    // Update is called once per frame
/*    void Update()
    {
        
    }*/

    void SetStats(int[] statsArray)
    {
        player.strength = statsArray[0];
        player.durability = statsArray[1];
        player.speed = statsArray[2];
        player.force = statsArray[3];
        player.resistance = statsArray[4];
        player.accuracy = statsArray[5];
        player.reflex = statsArray[6];
        player.energy = statsArray[7];
        player.regen = statsArray[8];
        player.recovery = statsArray[9];
        player.anger = statsArray[10];
    }
}
