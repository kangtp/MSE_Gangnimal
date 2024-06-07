using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImages : MonoBehaviour
{
    private List<GameObject> images;
    private int select_index=0;
    [SerializeField]
    private Sprite[] sprites;
    private Image background;
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
        background = GameObject.Find("BackGround").GetComponent<Image>();

        
        images[select_index].SetActive(true);
        background.sprite = sprites[select_index];
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
        background.sprite = sprites[select_index];
        PlayerPrefs.SetInt("SelectedMapIndex", select_index);
        PlayerPrefs.Save();
    }
}
