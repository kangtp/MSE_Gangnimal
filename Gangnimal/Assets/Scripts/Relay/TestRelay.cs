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
using TMPro;

public class TestRelay : MonoBehaviour
{
    [SerializeField]
    public GameObject[] turnOnobj;
    public TMP_InputField codeinput;

    public static TestRelay Instance { get; private set; }
    bool condition;

    public bool canSpawn;

    private void Awake()
    {
        Instance = this;
        condition = false;
        canSpawn = false;
    }

    public void JointheRelay()
    {
        JoinRelay(codeinput.text);
    }


    private async void Start()
    {
        
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        

    }

    IEnumerator turnOn()
    {
        yield return new WaitForSeconds(4.0f);
        while (true)
        {
            if (condition)
            {
                yield return new WaitForSeconds(0.1f);
                foreach (GameObject item in turnOnobj)
                {
                    item.SetActive(true);
                }
                StopCoroutine("turnOn");
            }
        }
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("joincode= " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);


            condition = NetworkManager.Singleton.StartHost();
            canSpawn = true;
            StartCoroutine("turnOn"); // 플레이어를 생성하기전에 몇개 오브젝트에서 awake, start에서 바로 플레이어를 찾아야되서 delay를 줘야된다.
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
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

            condition = NetworkManager.Singleton.StartClient();
            StartCoroutine("turnOn");

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
