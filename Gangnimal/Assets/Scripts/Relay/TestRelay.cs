using System.Collections;
using Unity.Services.Relay; 
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP; 
using Unity.Networking.Transport.Relay; 
using System.Threading.Tasks; 
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
public class TestRelay : MonoBehaviour
{
    [SerializeField]
    public GameObject[] turnOnobj; // Array of GameObjects to activate when the game starts
    public TMP_InputField codeinput; // Input field for entering the join code

    public GameObject LoadingObject; // Loading screen object

    public static TestRelay Instance { get; private set; } // Singleton instance of this class
    bool condition; // Condition to check if a player has joined the game

    public bool canSpawn; // Condition to check if spawning is allowed

    private void Awake()
    {
        Instance = this; // Set the singleton instance to this object
        condition = false; // Initialize condition to false
        canSpawn = false; // Initialize canSpawn to false
    }

    public void JointheRelay() // Function called when the client clicks the join button
    {
        JoinRelay(codeinput.text); // Call JoinRelay with the text from the input field
    }

    IEnumerator turnOn() // Coroutine to handle loading objects before the game starts
    {
        LoadingObject.SetActive(true); // Activate the loading screen
        yield return new WaitForSeconds(4.0f); // Wait for 4 seconds
        while (true)
        {
            if (condition) // Check if a player has joined the game
            {
                yield return new WaitForSeconds(0.1f); // Small delay to ensure objects are ready
                Load_map.Instance.LoadMapFunction(); // Activate the map
                foreach (GameObject item in turnOnobj) // Loop through the objects to activate them
                {
                    item.SetActive(true); // Activate the object
                }
                turnOnobj[3].SetActive(false); // Deactivate the canvas object
                turnOnobj[4].SetActive(false); // Deactivate another canvas object
                foreach (Transform child in turnOnobj[4].transform) // Deactivate all children of the canvas
                {
                    child.gameObject.SetActive(false); // Deactivate the child object
                }
                StopCoroutine("turnOn"); // Stop this coroutine
            }
        }
    }

    public async Task<string> CreateRelay() // Function to create a relay lobby
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); // Create a relay allocation for 3 players

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); // Get the join code for the allocation

            Debug.Log("joincode= " + joinCode); // Log the join code

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); // Create RelayServerData with allocation and protocol

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // Set the relay server data for the transport

            condition = NetworkManager.Singleton.StartHost(); // Start hosting the game
            canSpawn = true; // Allow spawning
            StartCoroutine("turnOn"); // Start the turnOn coroutine
            return joinCode; // Return the join code
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e); // Log any exceptions
            return null; // Return null if there is an exception
        }
    }

    public async void JoinRelay(string joinCode) // Function to join a relay lobby
    {
        try
        {
            Debug.Log("Joining Relay with" + joinCode); // Log the join attempt with the join code
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode); // Join the relay allocation

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); // Create RelayServerData with join allocation and protocol

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); // Set the relay server data for the transport

            condition = NetworkManager.Singleton.StartClient(); // Start the client
            StartCoroutine("turnOn"); // Start the turnOn coroutine
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e); // Log any exceptions
        }
    }

    public void EndGame() // Function to end the game
    {
        StartCoroutine("ShutdownClientFirst"); // Start the coroutine to shutdown clients first
    }

    IEnumerator ShutdownClientFirst() // Coroutine to handle shutdown process
    {
        yield return new WaitForSeconds(3.0f); // Wait for 3 seconds
        if (!NetworkManager.Singleton.IsServer) // If this instance is not the server
        {
            Debug.Log("클라이언트가 꺼짐"); // Log that the client is shutting down
            SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        }

        yield return new WaitForSeconds(1.0f); // Wait for 1 second
        if (NetworkManager.Singleton.IsServer) // If this instance is the server
        {
            Debug.Log("호스트가 꺼짐"); // Log that the host is shutting down
            NetworkManager.Singleton.Shutdown(); // Shutdown the network manager
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); // Load the main menu scene
        }
    }
}
