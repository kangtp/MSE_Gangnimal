using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestRelay : MonoBehaviour
{

    public TMP_InputField codeinput;

    public static TestRelay Instance {get; private set;}
    string code;

    private void Awake() {
        Instance = this;
    }

    public void JointheRelay()
    {
        JoinRelay(codeinput.text);
    }

    private async void Start()
    {
        /*
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        */

    }

    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("joincode= "+joinCode);

            code = joinCode;

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
        
    }

/*
    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }
    */

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with" + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
