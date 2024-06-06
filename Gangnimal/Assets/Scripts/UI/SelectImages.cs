using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImages : MonoBehaviour
{
    private List<GameObject> images;
    private int select_index=0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("SelectedMapIndex", select_index);
        images =new List<GameObject>();
        foreach(Transform t in transform)
        {
            images.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        images[select_index].SetActive(true);
    }
    public void Select(int index)
    {
        if(index == select_index)
        {
            return;
        }
        if(index <0 || index>images.Count)
        {
            return;
        }
        images[select_index].SetActive(false);
        select_index=index;
        images[select_index].SetActive(true);
        PlayerPrefs.SetInt("SelectedMapIndex", select_index);
        PlayerPrefs.Save();
    }
}
