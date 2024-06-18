using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Unity.Netcode;

// CamerController class controls the camera behavior using Cinemachine
public class CamerController : MonoBehaviour
{
    NetworkObject player; // Reference to the player's network object
    public CinemachineFreeLook cinemachineFreeLook; // Reference to the Cinemachine FreeLook component

    public static CamerController Instance; // Singleton instance of the CamerController

    // Start is called before the first frame update
    void Start()
    {
        Instance = this; // Assign this instance to the static Instance
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>(); // Get the Cinemachine FreeLook component

        // Set the camera rotation speed based on the mouse sensitivity from the GameManager
        if (GameManager.instance != null)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 300 * GameManager.instance.mouseSensitivity;
        }

        StartCoroutine("WaitingForObject"); // Start the coroutine to wait for the player object
    }

    // Coroutine to wait until the player object is available
    IEnumerator WaitingForObject()
    {
        while (true) // Loop continuously until the player object is found
        {
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before checking again

            // Check if the local player's network object is available
            if (NetworkManager.Singleton.LocalClient.PlayerObject != null)
            {
                player = NetworkManager.Singleton.LocalClient.PlayerObject; // Get the player's network object
                Debug.Log("client ID : " + NetworkManager.Singleton.IsClient); // Log the client status

                // Set the camera to follow the player
                cinemachineFreeLook.Follow = player.GetComponent<Transform>();

                // Get the look-at target from the player's first child
                GameObject lookat = player.transform.GetChild(0).gameObject;

                // If the look-at target is found, set the camera's LookAt property
                if (lookat != null)
                {
                    cinemachineFreeLook.LookAt = lookat.GetComponent<Transform>();
                }
                else // Log an error if the look-at target is not found
                {
                    Debug.LogError("Lookat GameObject를 찾을 수 없습니다.");
                }

                StopCoroutine("WaitingForObject"); // Stop the coroutine as the player object is found
            }
        }
    }
}
