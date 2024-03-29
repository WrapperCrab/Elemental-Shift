using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public bool playerInTrigger = false;//true if player walks into trigger
    public PresetBattle battle;//different for each trigger. Defines the battle to be initiated

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
        if (other.tag == "Player")
        {
            playerInTrigger = true;
        }
    }

    void activateBattle()
    {
        Debug.Log("activating battle");
        playerInTrigger = false;
        battle.setPlayerPrefabs(TeamManager.instance.getPlayerGOs());//set current player game objects

        BattleInitializer.instance.addActivatedTrigger(this);//!!!Will work?
        BattleInitializer.instance.initBattle(battle);
    }

    public void deactivate()
    {//deactivates this trigger. Usually after it has already been activated
        gameObject.SetActive(false);
    }
}
