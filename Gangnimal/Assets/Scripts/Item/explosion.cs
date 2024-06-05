using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public int damageAmount; 
    //sound
    private AudioSource audioSource;
    private AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this game object.");
        }
        audioSource.enabled = true;
        //SoundSetting();
    }

/*
    void SoundSetting(){
        explosionSound = Resources.Load<AudioClip>("SoundEffect/MExplosion");

        if (explosionSound == null)
        {
            Debug.LogError("Failed to load sound effect from Resources.");
            return;
        }

    }
*/
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
            
            GameManager.instance.PlayExplosionSound();

            PlayDestructionEffect();
            gameObject.SetActive(false);

        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            
            GameManager.instance.PlayExplosionSound();
            
            ApplyDamageToPlayer(collision.gameObject);

            PlayDestructionEffect();

            gameObject.SetActive(false);
        }
    }

    private void PlayDestructionEffect()
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect,2.0f);
            
        }
    }

    private void ApplyDamageToPlayer(GameObject player)
    {
        PlayerInfo playerInfo = player.GetComponent<PlayerInfo>();
        if (playerInfo != null && !playerInfo.haveShield)
        {
            playerInfo.TakeDamage(damageAmount);
        }
        else if(playerInfo != null && playerInfo.haveShield){
            //delete shield 
            playerInfo.haveShield = false;
        }
    }

    IEnumerator WaitCoroutine(float t)
    {
        //Debug.Log("MySecondCoroutine;" + t);
        yield return new WaitForSeconds(t);
    }
}
