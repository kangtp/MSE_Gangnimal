using System.Collections;
using UnityEngine;
using Unity.Netcode;
public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefabA; //add prefab in inspector
    [SerializeField] private GameObject playerPrefabB; //add prefab in inspector
 
    [ServerRpc(RequireOwnership=false)] //server owns this object but client can request a spawn
    public void SpawnPlayerServerRpc(ulong clientId,int prefabId) {
        GameObject newPlayer;
        if (prefabId==0)
             newPlayer=(GameObject)Instantiate(playerPrefabA);
        else
            newPlayer=(GameObject)Instantiate(playerPrefabB);
        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId,true);
        Debug.Log("들어왔지롱~");
    }

    public override void OnNetworkSpawn() {
    if (IsServer)
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId,0);
    else
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId,1);
}
}