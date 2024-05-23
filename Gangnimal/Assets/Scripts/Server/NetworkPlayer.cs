using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    // Update is called once per frame
    [SerializeField] private Transform spawnObjectPrefab;
    private Transform spawnObjectPrefabTransform;
    private NetworkVariable<int> randomnumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    private void Update()
    {
        if(!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.T)){
            
        }

        if(Input.GetKeyDown(KeyCode.E)){
            spawnObjectPrefabTransform =  Instantiate(spawnObjectPrefab);
            spawnObjectPrefabTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        if(Input.GetKeyDown(KeyCode.F)){
            spawnObjectPrefabTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnObjectPrefabTransform.gameObject);
        }

        Vector3 moveDIR = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W)) moveDIR.z = +1f;
        if(Input.GetKey(KeyCode.S)) moveDIR.z = -1f;
        if(Input.GetKey(KeyCode.A)) moveDIR.x = -1f;
        if(Input.GetKey(KeyCode.D)) moveDIR.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDIR * moveSpeed * Time.deltaTime;
    }

}
