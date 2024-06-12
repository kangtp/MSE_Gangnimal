using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class explosion : NetworkBehaviour
{
    public GameObject explosionEffect;
    public int damageAmount; 

    


    // 충돌이 발생했을 때 호출되는 메서드
    private void OnCollisionEnter(Collision collision)
    { 
        

        //자신에 닿아서 터지는 경우 방지
        WaitCoroutine(1.0f);

        // 충돌한 오브젝트의 태그가 "Map"인지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {

            if (gameObject.name == "BranchB")
            {
                GameManager.instance.PlayExplosionSound3();
            }
            else if(gameObject.name == "StoneB"){
                GameManager.instance.PlayExplosionSound2();
            }
            else if(gameObject.name == "BombB"){
                GameManager.instance.PlayExplosionSound1();
            }
            //GameManager.instance.PlayExplosionSound1();

            PlayDestructionEffect();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            
            if (gameObject.name == "BranchB")
            {
                GameManager.instance.PlayExplosionSound3();
            }
            else if(gameObject.name == "StoneB"){
                GameManager.instance.PlayExplosionSound2();
            }
            else if(gameObject.name == "BombB"){
                GameManager.instance.PlayExplosionSound1();
            }
            
            ApplyDamageToPlayer(collision.gameObject);

            PlayDestructionEffect();

            // 자신을 삭제
            Destroy(gameObject);
        }
/*
        if (gameObject.name == "BranchB")
        {
            GameManager.instance.PlayExplosionSound3();
        }
        else if(gameObject.name == "StoneB"){
            GameManager.instance.PlayExplosionSound2();
        }
        else if(gameObject.name == "BombB"){
            GameManager.instance.PlayExplosionSound1();
        }*/
    }

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
        //Debug.Log("isServer? " + IsServer + " isClient? " + IsClient + " IsHost? "+IsHost);

        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
        if (playerInfo != null)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == 0)    //If Player that damaged is host, 
            {
                Debug.Log("client -> server");
                playerInfo.TakeDamage(damageAmount);
            }
            else
            {
                Debug.Log("server -> client");
                //playerInfo.RequestDamageServerRpc(damageAmount);
                playerInfo.ApplyDamageToClientRpc(damageAmount);
            }
        }
    }

    IEnumerator WaitCoroutine(float t)
    {
        //Debug.Log("MySecondCoroutine;" + t);
        yield return new WaitForSeconds(t);
    }


}
