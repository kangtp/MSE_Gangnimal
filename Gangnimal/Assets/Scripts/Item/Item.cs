using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour
{
    public enum Type { Weapon };
    public Type type;
    public int value;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    [ServerRpc(RequireOwnership = false)]
    public void RequestDespawnServerRpc()
    {
        NetworkObject.Despawn(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddForceServerRPC(Vector3 force)
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.AddForce(force , ForceMode.Impulse);
        AddForceClientRPC(force);
    }

    [ClientRpc]
    private void AddForceClientRPC(Vector3 force)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.AddForce(force, ForceMode.Impulse);
    }
}
