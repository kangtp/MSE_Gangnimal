using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RandomSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] objects;
    public int spawnNumber;
    // Start is called before the first frame update
    void Start()
    {
        SpawnObject();
    }

    // Update is called once per fram

    void SpawnObject()
    {
        if (NetworkManager.Singleton.IsHost ||  NetworkManager.Singleton.IsServer)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < spawnNumber; j++)
                {
                    //Specify the area in which the item is to be randomly spawned for Forest Map

                    Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 30), 3, Random.Range(0, 50));
                    Transform spawn = Instantiate(objects[i], randomSpawnPosition, Quaternion.identity);
                    NetworkObject networkObject = spawn.GetComponent<NetworkObject>();
                    networkObject.Spawn(true);
                    //networkObject.transform.SetParent(this.gameObject.transform);

                }
            }
        }
    }
}
