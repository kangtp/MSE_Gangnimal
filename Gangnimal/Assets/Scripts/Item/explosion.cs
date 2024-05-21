using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public int damageAmount; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 충돌이 발생했을 때 호출되는 메서드
    private void OnCollisionEnter(Collision collision)
    { 
        

        //자신에 닿아서 터지는 경우 방지
        WaitCoroutine(1.0f);

        // 충돌한 오브젝트의 태그가 "Map"인지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayDestructionEffect();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamageToPlayer(collision.gameObject);

            PlayDestructionEffect();

            // 자신을 삭제
            Destroy(gameObject);
        }
    }

    private void PlayDestructionEffect()
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            
            Destroy(effect, 2.0f); // 2초 후에 파괴 효과 삭제
        }
    }

    private void ApplyDamageToPlayer(GameObject player)
    {
        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
        if (playerInfo != null)
        {
            playerInfo.TakeDamage(damageAmount);
        }
    }

    IEnumerator WaitCoroutine(float t)
    {
        //Debug.Log("MySecondCoroutine;" + t);
        yield return new WaitForSeconds(t);
    }
}
