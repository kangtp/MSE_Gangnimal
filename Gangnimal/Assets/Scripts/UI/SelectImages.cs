using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImages : MonoBehaviour //when map selct , background class  
{
    private List<GameObject> images; // backgrounds
    private int select_index=0; // default index
    [SerializeField]
    private Sprite[] sprites; // background sprites list
    public GameObject[] characterSelects; // Characeter models
    // Start is called before the first frame update
    void Start()
    {
         PlayerPrefs.SetString("SelectedMapIndex", select_index.ToString());// save default
        images =new List<GameObject>();
        foreach(Transform t in transform)//images in child so bring image
        {
            images.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
 

        
        images[select_index].SetActive(true); //set active true
        
    }
    public void MapSelect(int index)// Button in Mapselect
    {
        if(index == select_index) // when click twice then exit
        {
            return;
        }
        if(index <0 || index>images.Count) // when error
        {
            return;
        }
        images[select_index].SetActive(false); // current setactive false
        select_index=index; // select set indext
        images[select_index].SetActive(true);// select image true
        PlayerPrefs.SetString("SelectedMapIndex", select_index.ToString()); //Save index in playerprefebs
        PlayerPrefs.Save();//save
    }
  
}
