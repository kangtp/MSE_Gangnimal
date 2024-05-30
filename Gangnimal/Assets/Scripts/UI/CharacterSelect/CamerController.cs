using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    CinemachineFreeLook cinemachineFreeLook;
    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();

        
       

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            cinemachineFreeLook.Follow = player.GetComponent<Transform>();
        }
        else
        {
            
        }
         GameObject lookat = player.transform.GetChild(0).gameObject;
        if (lookat != null)
        {
            cinemachineFreeLook.LookAt = lookat.GetComponent<Transform>();
        }
        else
        {
            Debug.LogError("Lookat GameObject를 찾을 수 없습니다.");
        }
}

}
