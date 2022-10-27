using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Zoom : MonoBehaviour
{
    Vector2 inputValue;
    CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (inputValue.y > 0 && vcam.m_Lens.OrthographicSize >= 2f)
        {
            vcam.m_Lens.OrthographicSize--;
        }
        else if (inputValue.y < 0 && vcam.m_Lens.OrthographicSize <= 3f)
        {
            vcam.m_Lens.OrthographicSize++;
        }
        
    }

    void OnScrollWheel(InputValue input)
    {
        inputValue = input.Get<Vector2>();
    }
}
