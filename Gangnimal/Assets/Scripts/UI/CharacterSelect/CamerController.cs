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

        // GameObject 참조를 가져올 때 null 검사 추가
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
            Debug.LogError("Bear GameObject를 찾을 수 없습니다.");
        }
}

}
