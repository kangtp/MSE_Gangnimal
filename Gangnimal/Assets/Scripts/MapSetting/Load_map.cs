using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_map : MonoBehaviour
{
    public GameObject[] Map_Object;

    public static Load_map Instance;

    private void Awake() {
        Instance = this;
    }

    void ClearAndSpawn_Map(int arg)
    {
        foreach (GameObject item in Map_Object) // clear the map
        {
            item.SetActive(false);
        }
        Map_Object[arg].SetActive(true);
    }

    public void LoadMapFunction()
    {
        switch (PlayerPrefs.GetString("SelectedMapIndex"))
        {
            case "0": // when load forest map
                {
                    ClearAndSpawn_Map(0);
                }
                break;

            case "1": // when load desert map
                {
                    ClearAndSpawn_Map(1);
                }
                break;

            case "2": // when load winter map
                {
                    ClearAndSpawn_Map(2);
                }
                break;
            default:
                break;
        }

    }
}
