using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGround : MonoBehaviour
{

    List<GameObject> grounds = new List<GameObject>();
    int mapSelect=0;
   
    // Start is called before the first frame update
    void Start()
    {
        mapSelect=0;
        PlayerPrefs.SetInt("SelectedCharacterIndex", mapSelect);
       foreach(Transform t in transform)
       {
            grounds.Add(t.gameObject);
            t.gameObject.SetActive(false);
       }
       mapSelect = PlayerPrefs.GetInt("SelectedMapIndex");
       grounds[mapSelect].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
