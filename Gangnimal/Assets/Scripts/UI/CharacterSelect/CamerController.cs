using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Unity.Netcode;
public class CamerController : MonoBehaviour
{
    NetworkObject player;
    public CinemachineFreeLook cinemachineFreeLook;

    public static CamerController Instance;
    void Start()
    {
        Instance = this;
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        if (GameManager.instance != null)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 300 * GameManager.instance.mouseSensitivity;
        }

        StartCoroutine("WaitingForObject");
    }

    IEnumerator WaitingForObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (NetworkManager.Singleton.LocalClient.PlayerObject != null)
            {
                player = NetworkManager.Singleton.LocalClient.PlayerObject;
                Debug.Log("client ID : " + NetworkManager.Singleton.IsClient);
                cinemachineFreeLook.Follow = player.GetComponent<Transform>();
                GameObject lookat = player.transform.GetChild(0).gameObject;
                Debug.Log("lookat : " + lookat.transform);
                if (lookat != null)
                {
                    cinemachineFreeLook.LookAt = lookat.GetComponent<Transform>();
                }
                else
                {
                    Debug.LogError("Lookat GameObject를 찾을 수 없습니다.");
                }
                StopCoroutine("WaitingForObject");
            }
        }
    }

}
