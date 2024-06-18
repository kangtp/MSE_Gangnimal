using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour
{
    public enum Type { Weapon , Item };
    public Type type;
    public int value;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Function that the client asks the server to remove the object
    [ServerRpc(RequireOwnership = false)]
    public void RequestDespawnServerRpc()
    {
        NetworkObject.Despawn(true);
    }

}
