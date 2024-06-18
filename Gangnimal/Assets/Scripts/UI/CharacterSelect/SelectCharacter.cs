using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour //Character select Class
{
    private List<GameObject> models; // List of character models on the screen
    private int select_index; // Select index  1. bear 2. horse 3. rabbit
    private GameObject[] explainPannels; // Characeter Explain pannel
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        select_index=0;//defalut is bear so 0
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index); // setting default
        models =new List<GameObject>(); // model list
        foreach(Transform t in transform) // bring model in child and set active false
        {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        models[select_index].SetActive(true); // select model set active true
        explainPannels = GameObject.FindGameObjectsWithTag("Explain"); // bring explain panel
        explainPannels = ChangeSepuence(explainPannels); // when bring by tag , mixed order so change
        foreach(GameObject pannel in explainPannels) // Set false panels(explain)
        {
            pannel.SetActive(false);
        }
        explainPannels[select_index].SetActive(true);        
    }
    public void Select(int index) // A function that adjusts the selection each time a button is pressed
    {
        if(index == select_index) // When same button no change
        {
            return;
        }
        if(index <0 || index>models.Count) // because if error 
        {
            return;
        } 
        models[select_index].SetActive(false); // befor character set acitve false 
        explainPannels[select_index].SetActive(false); // same
        select_index=index;// change index
        models[select_index].SetActive(true); // select character set true
        explainPannels[select_index].SetActive(true);
        
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index);// and save in playerPrefeb
        PlayerPrefs.Save(); //save
        
    }
    public GameObject[] ChangeSepuence(GameObject[] pannels) // when bring by tags in list then mixed order so i make sort function
    {
        GameObject[] correctOrder = new GameObject[3];// characeter is 3 so size is 3
        foreach(GameObject p in pannels)//bring pannel object
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
       
       return correctOrder; // return correct order list
    }    
    
}
