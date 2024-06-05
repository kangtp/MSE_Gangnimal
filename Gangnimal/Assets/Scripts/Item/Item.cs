using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour
{
    public enum Type { Weapon };
    public Type type;
    public int value;

    [ServerRpc(RequireOwnership = false)]
    public void RequestDespawnServerRpc()
    {
        NetworkObject.Despawn(true);
    }
}
