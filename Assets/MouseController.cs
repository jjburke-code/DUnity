using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
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
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;
        Vector3 currPosition = new Vector3();
        currPosition = worldPosition;
        SpawnEnemy(currPosition); // for testing
    }

    void OnMousePosition(InputValue position)
    {
        mousePosition = position.Get<Vector2>();
    }

    void SpawnEnemy(Vector3 input)
    {
        Instantiate(enemy, input, Quaternion.identity);
    }
}
