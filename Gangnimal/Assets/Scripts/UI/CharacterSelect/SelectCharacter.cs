using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    private List<GameObject> models;
    private int select_index;
    
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
        select_index=index;
        models[select_index].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index);
        GameManager.instance.PlayCharacterSound(select_index);
        PlayerPrefs.Save();
        Debug.Log("맵선택창");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
