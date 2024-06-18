using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public Transform respawnPosition; // RespawnPosition Because Sometimes when spawn character , drop Characet through map
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) { // when trigger dropzone 
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && other.CompareTag("Player")) // if player then respawn player 
        {
            cc.enabled = false; 
            other.transform.position = respawnPosition.position;
            cc.enabled = true; 
            return;
        }
        else if(other.CompareTag("Item")) // when item then set active false
        {
            other.gameObject.SetActive(false);
        }
    
    }
}
