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
    public GameObject[] turnOnobj;
    public TMP_InputField codeinput;

    public GameObject LoadingObject;

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

    IEnumerator turnOn()
    {
        LoadingObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        while (true)
        {
            if (condition)
            {
                yield return new WaitForSeconds(0.1f);
                Load_map.Instance.LoadMapFunction();
                foreach (GameObject item in turnOnobj)
                {
                    item.SetActive(true);
                }
                turnOnobj[3].SetActive(false);
                turnOnobj[4].SetActive(false);
                foreach (Transform child in turnOnobj[4].transform)
                {
                    child.gameObject.SetActive(false);
                }
                StopCoroutine("turnOn");
            }
        }
    }

    public async Task<string> CreateRelay()
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

            condition = NetworkManager.Singleton.StartClient();
            StartCoroutine("turnOn");

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void EndGame()
    {
        /*
        // 서버나 호스트인 경우
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("호스트가 꺼짐");
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
        }
        // 서버가 아닐 경우
        else if (!NetworkManager.Singleton.IsServer)
        {
            Debug.Log("클라이언트가 꺼짐");
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
        }
        // 씬 전환
        */
        StartCoroutine("ShutdownClientFirst");
    }

    IEnumerator ShutdownClientFirst()
    {
        yield return new WaitForSeconds(3.0f);
        if (!NetworkManager.Singleton.IsServer)
        {
            Debug.Log("클라이언트가 꺼짐");
            SceneManager.LoadScene("MainMenu");
        }

        yield return new WaitForSeconds(1.0f);
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("호스트가 꺼짐");
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
        }
    }
}
