using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : Interaction
{
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public string text;

    public override void interact()
    {
        deLeaveTree();
    }

    private void deLeaveTree()
    {
        //change the sprite of the tree
        spriteRenderer.sprite = newSprite;

        //change the object's tag
        //make text "you de-leaved the tree"

        triggerDialogue();

    }
}
