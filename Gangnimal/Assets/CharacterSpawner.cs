using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
     public GameObject[] characterPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        // 다음 씬에서 선택된 캐릭터의 인덱스를 얻게 됩니다.

        // 선택된 인덱스를 사용하여 선택된 캐릭터를 생성합니다.
        if (selectedIndex >= 0 && selectedIndex < characterPrefabs.Length)
        {
            //Instantiate(characterPrefabs[selectedIndex]);
            Debug.Log("Character is " + selectedIndex);
        }
        else
        {
            Debug.Log(selectedIndex);
            Debug.LogError("잘못된 선택된 캐릭터 인덱스: " + selectedIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
