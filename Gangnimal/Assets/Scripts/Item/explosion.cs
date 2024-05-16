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
            // 자신을 삭제
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Player에게 데미지 입히기
            ApplyDamageToPlayer(collision.gameObject);

            // 파괴 효과 실행
            PlayDestructionEffect();

            // 자신을 삭제
            Destroy(gameObject);
        }
    }

    // 파괴 효과를 실행하는 메서드
    private void PlayDestructionEffect()
    {
        if (explosionEffect != null)
        {
            // 파괴 효과 인스턴스 생성
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            
            // 파괴 효과를 일정 시간 후에 삭제
            Destroy(effect, 2.0f); // 2초 후에 파괴 효과 삭제
        }
    }

        // Player에게 데미지를 입히는 메서드
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
