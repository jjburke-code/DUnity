using Mirror;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLocationCharacter : NetworkBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {  
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkClient.localPlayer.gameObject != null)
        {
            virtualCamera.Follow = NetworkClient.localPlayer.gameObject.transform;
        }
    }


}
