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
        if (GameManager.instance != null)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 300 * GameManager.instance.mouseSensitivity;
        }

        
        GameObject lookat = GameObject.FindGameObjectWithTag("Lookat");
        if (lookat != null)
        {
            cinemachineFreeLook.LookAt = lookat.GetComponent<Transform>();
        }
        else
        {
            Debug.LogError("Lookat GameObject를 찾을 수 없습니다.");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            cinemachineFreeLook.Follow = player.GetComponent<Transform>();
        }
        else
        {
            
        }
}

}
