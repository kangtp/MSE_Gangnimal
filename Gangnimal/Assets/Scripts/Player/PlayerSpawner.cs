using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefab_Bear; // Prefab for the bear character, assigned in the inspector
    [SerializeField] private GameObject playerPrefab_Horse; // Prefab for the horse character, assigned in the inspector
    [SerializeField] private GameObject playerPrefab_Rabbit; // Prefab for the rabbit character, assigned in the inspector
 
    [ServerRpc(RequireOwnership=false)] // This method is a server RPC that can be called by clients, without requiring ownership of the object
    public void SpawnPlayerServerRpc(ulong clientId, int prefabId) {
        GameObject newPlayer;

        // Instantiate the appropriate player prefab based on the prefabId
        if (prefabId == 0)
            newPlayer = (GameObject)Instantiate(playerPrefab_Bear);
        else if (prefabId == 1)
            newPlayer = (GameObject)Instantiate(playerPrefab_Horse);
        else
            newPlayer = (GameObject)Instantiate(playerPrefab_Rabbit);

        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>(); // Get the NetworkObject component from the instantiated prefab
        newPlayer.SetActive(true); // Activate the new player object
        netObj.SpawnAsPlayerObject(clientId, true); // Spawn the new player object on the network, associating it with the specified client
    }

    public override void OnNetworkSpawn() {
        // When the network object this script is attached to spawns, call the server RPC to spawn the player
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, PlayerPrefs.GetInt("SelectedCharacterIndex"));
    }
}
