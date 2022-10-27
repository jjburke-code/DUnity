using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public struct Stats
    {
        public float bp; // not used atm
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
        MakeCharacter();
    }

    void MakeCharacter()
    {
        SetStats("bp", 100f);
        SetStats("strength", 1f);
        SetStats("durability", 1f);
        SetStats("speed", 1f);
        SetStats("force", 1f);
        SetStats("resistance", 1f);
        SetStats("accuracy", 1f);
        SetStats("reflex", 1f);

        SetStats("energy", 1f);
        SetStats("regen", 1f);
        SetStats("recovery", 1f);
        SetStats("anger", 1f);
        print("character made");
    }

    public void SetStats(string stat,float value)
    {
        switch (stat)
        {
            case "bp":
                player.bp = value;
                break;
            case "strength":
                player.strength = value;
                break;
            case "durability":
                player.durability = value;
                break;
            case "speed":
                player.speed = value;
                break;
            case "force":
                player.force = value;
                break;
            case "resistance":
                player.resistance = value;
                break;
            case "accuracy":
                player.accuracy = value;
                break;
            case "reflex":
                player.reflex = value;
                break;
            case "energy":
                player.energy = value;
                break;
            case "regen":
                player.regen = value;
                break;
            case "recovery":
                player.recovery = value;
                break;
            case "anger":
                player.anger = value;
                break;
        }
    }

}
