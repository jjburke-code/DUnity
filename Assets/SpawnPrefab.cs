using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SpawnPrefab : MonoBehaviour

{
    Vector2 mousePosition;
    Vector3 worldPosition;
    Transform mouseSpot;

    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        mouseSpot = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseSpot.position = worldPosition;
        
    }

    void OnFire()
    {
        Vector3 currPosition = new Vector3();
        currPosition = worldPosition;
        Instantiate(enemy, currPosition, Quaternion.identity);
    }

    void OnMousePosition(InputValue position)
    {
        mousePosition = position.Get<Vector2>();
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;
    }
}
