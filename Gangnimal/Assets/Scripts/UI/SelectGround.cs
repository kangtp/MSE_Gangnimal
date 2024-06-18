using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGround : MonoBehaviour
{

    List<GameObject> grounds = new List<GameObject>();// Map Select class
    int mapSelect=0; // defalut index 0 : forest 1 : desert 2; winter 
   
    // Start is called before the first frame update
    void Start()
    {
       mapSelect=0;
       PlayerPrefs.SetInt("SelectedCharacterIndex", mapSelect); // set default
       foreach(Transform t in transform) // bring map 
       {
            grounds.Add(t.gameObject);
            t.gameObject.SetActive(false);
       }
       mapSelect = PlayerPrefs.GetInt("SelectedMapIndex"); // save in player prefeb
       grounds[mapSelect].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
