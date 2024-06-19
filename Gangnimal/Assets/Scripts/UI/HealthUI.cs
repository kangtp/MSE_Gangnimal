using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour, Observerinterface
{
    [SerializeField] Text healthText;
    private PlayerInfo playerInfo;

    void Start()
    {
        GameObject.Find("Canvas").transform.GetChild(5).gameObject.SetActive(true);
        healthText = GameObject.Find("HP").GetComponent<Text>();

        //Indicates the HP value of the local player
        healthText.text = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<PlayerInfo>().HP.ToString();
    }

    //Modify text with changed HP
    public void InformationUpdate(int health)
    {
        Debug.Log(health);
        healthText.text = health.ToString();
    }

    public void RegisterObserver()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        playerInfo.RegisterObserver(this);
    }

  
}
