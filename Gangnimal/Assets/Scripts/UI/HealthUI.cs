using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour, Observerinterface
{
    [SerializeField] Text healthText;
    private PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        playerInfo.RegisterObserver(this);
        healthText.text = playerInfo.HP.ToString();
    }

    void OnDestroy()
    {
        playerInfo.RemoveObserver(this);
    }


    public void InformationUpdate(int health)
    {
        healthText.text = health.ToString();
    }

  
}
