using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public int spawnNumber;
    // Start is called before the first frame update
    void Start()
    {
        SpawnItem();
    }

    // Update is called once per fram
    void SpawnItem()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < spawnNumber; j++)
                {
                    Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 30), 3, Random.Range(0, 50));
                    GameObject spawn = Instantiate(objects[i], randomSpawnPosition, Quaternion.identity);
                    spawn.transform.SetParent(this.gameObject.transform);

                }
            }
        }
    }
}
