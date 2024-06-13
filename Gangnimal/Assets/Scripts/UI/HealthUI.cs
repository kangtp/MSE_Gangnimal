using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour, Observerinterface
{
    [SerializeField] Text healthText;
    private PlayerInfo playerInfo;

    void Start()
    {

        //healthText = GameObject.Find("HP").GetComponent<Text>();
        GameObject.Find("Canvas").transform.GetChild(5).gameObject.SetActive(true);
        healthText = GameObject.Find("HP").GetComponent<Text>();
        healthText.text = "100";
    }


    void OnDestroy()
    {
        //playerInfo.RemoveObserver(this);
    }


    public void InformationUpdate(int health)
    {
        Debug.Log(health);
        healthText.text = health.ToString();
    }

    public void RegisterObserver()
    {
        Debug.Log("register observer" );
        playerInfo = FindObjectOfType<PlayerInfo>();
        playerInfo.RegisterObserver(this);
    }

  
}
