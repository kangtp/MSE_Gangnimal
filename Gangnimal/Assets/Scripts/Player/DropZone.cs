using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public Transform respawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && other.CompareTag("Player"))
        {
            cc.enabled = false; 
            other.transform.position = respawnPosition.position;
            cc.enabled = true; 
            return;
        }
        else if(other.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
        }
    
    }
}
