using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    Vector2 mousePosition;
    public Vector3 currentMousePosition;
    Transform mouseSpot;

    // Start is called before the first frame update
    void Start()
    {
        mouseSpot = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseSpot.position = currentMousePosition;
    }
/*
    void OnFire() 
    {
        
    }*/

    void OnMousePosition(InputValue position)
    {
        mousePosition = position.Get<Vector2>();
        currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        currentMousePosition.z = 0;
    }
}
