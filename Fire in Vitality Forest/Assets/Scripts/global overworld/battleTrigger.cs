using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleTrigger : MonoBehaviour
{
    public bool playerInTrigger = false;//true if player walks into trigger
    public bool battleCompleted = false;//true after associated battle is completed
    public PresetBattle battle;//different for each trigger. Defines the battle top be initiated

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            activateBattle();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("here");
        if (other.tag == "Player")
        {
            playerInTrigger = true;
        }
    }

    void activateBattle()
    {
        Debug.Log("activating battle");
        playerInTrigger = false;
    }
}
