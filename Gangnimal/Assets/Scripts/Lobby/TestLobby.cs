using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TestLobby : MonoBehaviour
{

    private Lobby hostlobby;
    private Lobby joinedlobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;

    private float lobbyPollTimer;
    private string PlayerName;

    private bool enableStart;

    private bool stopUpdateLobby;

    public const string KEY_START_GAME = "Start";

    private void Start()
    {
        enableStart = false;
        stopUpdateLobby = false;
        start();
    }
    private async void start()
    {


        await UnityServices.InitializeAsync(); //request

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("sigend in  " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        PlayerName = "MSE" + UnityEngine.Random.Range(10, 99);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat() // 로비 호스트가 나가면 15초뒤에 사라지게하는 함수
    {
        if (hostlobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostlobby.Id);

            }
        }
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedlobby != null && !stopUpdateLobby) 
        {

            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedlobby.Id);
                joinedlobby = lobby;
        
                if (joinedlobby.Data[KEY_START_GAME].Value != "0")
                {
                    if (hostlobby != null)
                    {
                        TestRelay.Instance.JoinRelay(joinedlobby.Data[KEY_START_GAME].Value);
                        stopUpdateLobby = true;
                    }
                }

            }
        }
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";

            int maxPlayers = 2;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>{
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public, "Castle")},
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostlobby = lobby;
            joinedlobby = hostlobby;

            Debug.Log("Create Lobby!" + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            PrintPlayers(hostlobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0" , QueryFilter.OpOptions.GT)
                    //new QueryFilter(QueryFilter.FieldOptions.S1, "CaptureTheFlag", QueryFilter.OpOptions.EQ)
                },
                Order = new List<QueryOrder>{
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found : " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["Map"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobby(string lobbyCode) // 로비코드로 들어오기
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            Lobby JoinLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            Debug.Log("Joined Lobby with code" + lobbyCode);

            PrintPlayers(JoinLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby() // 빠른참가 <- 이거 사용할거임
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobby = new QuickJoinLobbyOptions
            {
                Player = GetPlayer()
            };
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobby);
            joinedlobby = lobby;
            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,PlayerName)}
                    }
        };
    }

    public void PrintPlayers()
    {
        PrintPlayers(joinedlobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby" + lobby.Name + " " + lobby.Data["Map"].Value);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    public async void StartGame()
    {
        if (joinedlobby != null)
        {
            int count = 0;
            foreach (Player player in joinedlobby.Players)
            {
                count++;
            }
            if (count == 2)
            {
                if (hostlobby != null)
                {
                    string relayCode = await TestRelay.Instance.CreateRelay();

                    Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedlobby.Id, new UpdateLobbyOptions
                    {
                        Data = new Dictionary<string, DataObject>{
                            {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member,relayCode)}
                        }
                    });
                }
                else
                {
                    TestRelay.Instance.JoinRelay(joinedlobby.Data[KEY_START_GAME].Value);
                }

                //SceneManager.LoadScene("ForestScene");

            }
        }

    }

    private async void UpdateGameMode(string map)
    {
        try
        {
            hostlobby = await Lobbies.Instance.UpdateLobbyAsync(hostlobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>{
                {"Map", new DataObject(DataObject.VisibilityOptions.Public, map)}
            }
            });
            joinedlobby = hostlobby;

            PrintPlayers(hostlobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedlobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedlobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void CreateLobbyButton()
    {
        CreateLobby();
    }

    public void SearchLobby()
    {
        ListLobbies();
    }

    public void JoinLobbyLobbyBt()
    {
        QuickJoinLobby();
    }

    public void LeaveLobbyBt()
    {
        LeaveLobby();
    }

    public void DeleteLobbyBt()
    {
        DeleteLobby();
    }
}
