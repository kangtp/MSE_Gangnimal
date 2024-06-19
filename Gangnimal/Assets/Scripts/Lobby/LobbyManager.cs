using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System;
public class LobbyManager : MonoBehaviour
{
    private Lobby hostlobby; // The lobby created by this player if they are the host
    private Lobby joinedlobby; // The lobby this player has joined
    private float heartbeatTimer; // Timer for sending heartbeat pings to the lobby
    private float lobbyUpdateTimer; // Timer for polling lobby updates
    private string PlayerName; // Name of the player

    private bool joinCondtiion; // Condition to check if the player has successfully joined a lobby
    public TMP_InputField codeinput; // Input field for entering the join code
    public TMP_Text joinCode_Text; // Text field to display the join code
    public static LobbyManager Instance; // Singleton instance of this class

    [SerializeField]
    private GameObject[] canvasobj; // Array of canvases: 0 = Lobby Canvas, 1 = MapSelectCanvas, 2 = CharSelectCanvas

    private void Start()
    {
        Instance = this; // Set the singleton instance to this object
        start(); // Call the start method
    }

    private void start()
    {
        foreach (GameObject canvas in canvasobj) // Disable all canvases initially
        {
            canvas.SetActive(false);
        }
        canvasobj[0].SetActive(true); // Enable the lobby canvas
    }

    private void Update()
    {
        HandleLobbyPollForUpdates(); // Call method to handle lobby updates
    }

    private async void HandleLobbyHeartbeat() // Function to send heartbeat pings to the lobby
    {
        if (hostlobby != null) // If this player is the host
        {
            heartbeatTimer -= Time.deltaTime; // Decrement the timer
            if (heartbeatTimer < 0f) // If the timer has expired
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax; // Reset the timer

                await LobbyService.Instance.SendHeartbeatPingAsync(hostlobby.Id); // Send a heartbeat ping to keep the lobby alive
            }
        }
    }

    private async void HandleLobbyPollForUpdates() // Function to poll for lobby updates
    {
        if (joinedlobby != null) // If this player has joined a lobby
        {
            lobbyUpdateTimer -= Time.deltaTime; // Decrement the timer
            if (lobbyUpdateTimer < 0f) // If the timer has expired
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax; // Reset the timer

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedlobby.Id); // Get the latest lobby information
                joinedlobby = lobby; // Update the joined lobby
            }

            if (joinedlobby.Data["KEY_START_GAME"].Value != "0") // Check if the game has started
            {
                if (!IsLobbyHost()) // If this player is not the host
                {
                    TestRelay.Instance.JoinRelay(joinedlobby.Data["KEY_START_GAME"].Value); // Join the relay with the start game key
                }

                joinedlobby = null; // Clear the joined lobby
            }
        }
    }

    public bool IsLobbyHost() // Check if this player is the host of the lobby
    {
        return joinedlobby != null && joinedlobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public bool IsPlayerInLobby() // Check if this player is in the lobby
    {
        if (joinedlobby != null && joinedlobby.Players != null)
        {
            foreach (Player player in joinedlobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    return true; // This player is in the lobby
                }
            }
        }
        return false; // This player is not in the lobby
    }

    public async void CreateLobby() // Function to create a new lobby
    {
        try
        {
            string lobbyName = "MyLobby"; // Name of the lobby
            int maxPlayers = 2; // Maximum number of players

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(), // Get the player data
                Data = new Dictionary<string, DataObject>{
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public,  PlayerPrefs.GetString("SelectedMapIndex"))},
                    {"KEY_START_GAME", new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions); // Create the lobby

            hostlobby = lobby; // Set the host lobby
            joinedlobby = hostlobby; // Set the joined lobby

            Debug.Log("Who is owner? " + IsLobbyHost() + ", " + lobby.LobbyCode); // Log the host status and lobby code
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e); // Log any exceptions
        }
    }

    private async void JoinLobby(string lobbyCode) // Function to join a lobby using a code
    {
        if (lobbyCode.Length > 0) // Check if the code is valid
        {
            try
            {
                JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
                {
                    Player = GetPlayer() // Get the player data
                };
                Lobby JoinLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions); // Join the lobby using the code

                if (JoinLobby != null)
                {
                    joinCondtiion = true; // Set the join condition to true
                    foreach (GameObject canvas in canvasobj)
                    {
                        canvas.SetActive(false); // Disable all canvases
                    }
                    canvasobj[2].SetActive(true); // Enable the character select canvas
                    foreach (Transform child in canvasobj[2].transform)
                    {
                        child.gameObject.SetActive(true); // Enable all children of the character select canvas
                    }
                }
                else
                {
                    joinCondtiion = false; // Set the join condition to false
                }

                Debug.Log("Joined Lobby with code" + lobbyCode); // Log the join code

                PrintPlayers(JoinLobby); // Print the players in the lobby
                PlayerPrefs.SetString("SelectedMapIndex", JoinLobby.Data["Map"].Value); // Save the selected map index
                PlayerPrefs.Save(); // Save the player preferences
                Debug.Log("Who is owner? " + IsLobbyHost()); // Log the host status

                joinedlobby = JoinLobby; // Set the joined lobby
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e); // Log any exceptions
            }
        }
    }

    private async void QuickJoinLobby() // Function to quickly join a lobby
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobby = new QuickJoinLobbyOptions
            {
                Player = GetPlayer() // Get the player data
            };
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobby); // Quickly join a lobby
            joinedlobby = lobby; // Set the joined lobby
            PrintPlayers(lobby); // Print the players in the lobby
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e); // Log any exceptions
        }
    }

    private Player GetPlayer() // Function to get the player data
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName)}
            }
        };
    }

    public void PrintPlayers() // Function to print the players in the joined lobby
    {
        PrintPlayers(joinedlobby); // Call the overloaded function
    }

    private void PrintPlayers(Lobby lobby) // Function to print the players in a specified lobby
    {
        Debug.Log("Players in Lobby" + lobby.Name + " " + lobby.Data["Map"].Value); // Log the lobby name and map
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value); // Log each player's ID and name
        }
    }

    public async void StartGame() // Function to start the game
    {
        if (IsLobbyHost()) // If this player is the host
        {
            try
            {
                string relaycode = await TestRelay.Instance.CreateRelay(); // Create a relay and get the relay code

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedlobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>{
                        {"KEY_START_GAME", new DataObject(DataObject.VisibilityOptions.Member, relaycode)}
                    }
                });

                joinedlobby = lobby; // Update the joined lobby
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e); // Log any exceptions
            }
        }
    }

    public void MapSelectActive() // Function to activate the map select canvas
    {
        canvasobj[0].SetActive(false); // Disable the lobby canvas
        canvasobj[1].SetActive(true); // Enable the map select canvas
    }

    public void CharacterSelectActiveHost() // Function to activate the character select canvas as host
    {
        CreateLobby(); // Create the lobby
        foreach (GameObject canvas in canvasobj)
        {
            canvas.SetActive(false); // Disable all canvases
        }
        canvasobj[2].SetActive(true); // Enable the character select canvas
        foreach (Transform child in canvasobj[2].transform)
        {
            child.gameObject.SetActive(true); // Enable all children of the character select canvas
        }
        StartCoroutine("Delaycreatecode"); // Start the coroutine to delay creating the code
    }

    IEnumerator Delaycreatecode() // Coroutine to delay creating the join code
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second
        joinCode_Text.text = "Code: " + hostlobby.LobbyCode; // Display the join code
    }

    public void CharacterSelectActiveClient() // Function to activate the character select canvas as client
    {
        JoinLobby(codeinput.text); // Join the lobby using the code from the input field
    }

    public void CreateLobbyButton() // Function to create a lobby when a button is pressed
    {
        CreateLobby(); // Create the lobby
    }

    public bool StartCondition() // Function to check if the game can start
    {
        int count = 0;
        foreach (Player player in joinedlobby.Players)
        {
            count++; // Count the number of players in the lobby
        }
        if (count == 2)
            return true; // If there are 2 players, return true
        else
            return false; // Otherwise, return false
    }
}
