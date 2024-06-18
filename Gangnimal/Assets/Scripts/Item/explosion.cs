using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class explosion : NetworkBehaviour
{
    public GameObject explosionEffect;
    public int damageAmount; 


    private void OnCollisionEnter(Collision collision)
    {
        //The weapon explodes when it touches a player or the ground.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") )
        {
            //If the weapon touch the player, player will get damage.
            if (collision.gameObject.CompareTag("Player"))
            {
                ApplyDamageToPlayer(collision.gameObject);
            }

            PlayDestructionEffect();

            //different explosion sounds depending on the weapon.
            if (gameObject.name == "BranchB(Clone)")
            {
                GameManager.instance.PlayExplosionSound3();
            }
            else if (gameObject.name == "StoneB(Clone)")
            {
                GameManager.instance.PlayExplosionSound2();
            }
            else if (gameObject.name == "BombB(Clone)")
            {
                GameManager.instance.PlayExplosionSound1();
            }

            Destroy(gameObject);
        }
    }

    //Show Explosion Effect
    private void PlayDestructionEffect()
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            effect.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void ApplyDamageToPlayer(GameObject player)
    {

        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
        
        if (playerInfo != null)
        {
            //If the person who received the damage is the host
            if (player.GetComponent<NetworkObject>().OwnerClientId == 0)
            {
                playerInfo.TakeDamage(damageAmount);
            }
            //If the person who received the damage is the client
            else
            {
                playerInfo.ApplyDamageToClientRpc(damageAmount);
            }
        }
    }
}
