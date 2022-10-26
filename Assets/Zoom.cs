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

        if (inputValue.y > 0 && vcam.m_Lens.OrthographicSize > 0.2f)
        {
            if (vcam.m_Lens.OrthographicSize > 0.7f && vcam.m_Lens.OrthographicSize < 0.9f)
            {
                vcam.m_Lens.OrthographicSize = 0.6f;
            }
            else if (vcam.m_Lens.OrthographicSize > 0.9f && vcam.m_Lens.OrthographicSize <= 1.2f)
            {
                vcam.m_Lens.OrthographicSize = 0.8f;
            }
            else if (vcam.m_Lens.OrthographicSize > 1.2f)
            {
                vcam.m_Lens.OrthographicSize = 1.2f;
            }
            else
            {
                vcam.m_Lens.OrthographicSize -= 0.1f;
            }
            
        }
        else if (inputValue.y < 0 && vcam.m_Lens.OrthographicSize < 2f)
        {
            if (vcam.m_Lens.OrthographicSize > 0.6f && vcam.m_Lens.OrthographicSize < 0.8f)
            {
                vcam.m_Lens.OrthographicSize = 0.9f;
            }
            else if (vcam.m_Lens.OrthographicSize >= 0.9f && vcam.m_Lens.OrthographicSize < 1.2f)
            {
                vcam.m_Lens.OrthographicSize = 1.2f;
            }
            else if (vcam.m_Lens.OrthographicSize >= 1.2f)
            {
                vcam.m_Lens.OrthographicSize = 2f;
            }
            else
            {
                vcam.m_Lens.OrthographicSize += 0.1f;
            }
        }
        
    }

    void OnScrollWheel(InputValue input)
    {
        inputValue = input.Get<Vector2>();
    }
}
