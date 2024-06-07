using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    private List<GameObject> models;
    private int select_index;
    private GameObject[] explainPannels;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        select_index=0;
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index);
        models =new List<GameObject>();
        foreach(Transform t in transform)
        {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        models[select_index].SetActive(true);
        explainPannels = GameObject.FindGameObjectsWithTag("Explain");
        explainPannels = ChangeSepuence(explainPannels);
        foreach(GameObject pannel in explainPannels)
        {
            pannel.SetActive(false);
        }
        explainPannels[select_index].SetActive(true);        
    }
    public void Select(int index)
    {
        if(index == select_index)
        {
            return;
        }
        if(index <0 || index>models.Count)
        {
            return;
        }
        models[select_index].SetActive(false);
        explainPannels[select_index].SetActive(false);
        select_index=index;
        models[select_index].SetActive(true);
        explainPannels[select_index].SetActive(true);
        
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index);
        GameManager.instance.PlayCharacterSound(select_index);
        PlayerPrefs.Save();
        
    }
    public GameObject[] ChangeSepuence(GameObject[] pannels)
    {
        GameObject[] correctOrder = new GameObject[3];
        foreach(GameObject p in pannels)
        {
            if(p.name=="BearExplain")
            {
                correctOrder[0] = p;
            }
            else if(p.name=="HorseExplain")
            {
                correctOrder[1]=p;
            }
            else if(p.name=="RabbitExplain")
            {
                correctOrder[2]=p;
            }
        }
        //확인용
        foreach(GameObject p in correctOrder)
        {
            Debug.Log(p.name);
        }
       
       return correctOrder;
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
