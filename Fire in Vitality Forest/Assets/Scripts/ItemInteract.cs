using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : Interaction
{
    public Item item;

    public override void interact()
    {
        //add item to inventory
        bool itemAdded = Inventory.instance.Add(item);


        if (itemAdded)
        {
            //trigger text
            triggerDialogue();

            //destroy object in world
            Destroy(gameObject);
            return;
        }
        Dialogue noSpace = Inventory.instance.getDialogue();
        FindObjectOfType<DialogueManager>().startDialogue(noSpace);
    }
}
