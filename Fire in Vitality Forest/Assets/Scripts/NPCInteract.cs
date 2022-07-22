using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : Interaction
{
    public override void interact()
    {
        //!!!face the player

        //Talk to player
        triggerDialogue();
    }
}
