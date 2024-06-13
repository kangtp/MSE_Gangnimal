using System.Collections;
using UnityEngine;
using Unity.Netcode;
public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefab_Bear; //add prefab in inspector
    [SerializeField] private GameObject playerPrefab_Horse; //add prefab in inspector
    [SerializeField] private GameObject playerPrefab_Rabbit; //add prefab in inspector
 
    [ServerRpc(RequireOwnership=false)] //server owns this object but client can request a spawn
    public void SpawnPlayerServerRpc(ulong clientId,int prefabId) {
        GameObject newPlayer;
        if (prefabId==0)
             newPlayer=(GameObject)Instantiate(playerPrefab_Bear);
        else if(prefabId==1)
            newPlayer=(GameObject)Instantiate(playerPrefab_Horse);
        else
            newPlayer=(GameObject)Instantiate(playerPrefab_Rabbit);
        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId,true);
    }

    public override void OnNetworkSpawn() {
    SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId,PlayerPrefs.GetInt("SelectedCharacterIndex"));

}
}