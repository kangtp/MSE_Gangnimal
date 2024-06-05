using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
     public GameObject[] characterPrefabs;
     public Transform spawnPoint;

    // Start is called before the first frame update
    private void Awake() {
        
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        // 다음 씬에서 선택된 캐릭터의 인덱스를 얻게 됩니다.

        // 선택된 인덱스를 사용하여 선택된 캐릭터를 생성합니다.
        if (selectedIndex >= 0 && selectedIndex < characterPrefabs.Length)
        {
            GameObject newCharacter=Instantiate(characterPrefabs[selectedIndex]);
            newCharacter.transform.position = spawnPoint.position;

            Debug.Log("Character is " + selectedIndex);
        }
        else
        {
            Debug.Log(selectedIndex);
            Debug.LogError("잘못된 선택된 캐릭터 인덱스: " + selectedIndex);
        }

    }
}
