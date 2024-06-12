using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("SelectedCharacterIndex"));
        Debug.Log(PlayerPrefs.GetInt("SeletMapIndex"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
