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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") )
        {
            
            if (collision.gameObject.CompareTag("Player"))
            {
                ApplyDamageToPlayer(collision.gameObject);
            }

            if (gameObject.name == "BranchB(Clone)")
            {
                Debug.Log("Branch");
                GameManager.instance.PlayExplosionSound3();
            }
            else if (gameObject.name == "StoneB(Clone)")
            {
                Debug.Log("stone");
                GameManager.instance.PlayExplosionSound2();
            }
            else if (gameObject.name == "BombB(Clone)")
            {
                Debug.Log("BomB");
                GameManager.instance.PlayExplosionSound1();
            }

            PlayDestructionEffect();
            Destroy(gameObject);
        }

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

        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();

        
        if (playerInfo != null)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == 0)    //If Player that damaged is host, 
            {
                Debug.Log("Server damaged");
                playerInfo.TakeDamage(damageAmount);
            }
            else       //If Player that damaged is client,
            {
                Debug.Log("client damaged");
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
