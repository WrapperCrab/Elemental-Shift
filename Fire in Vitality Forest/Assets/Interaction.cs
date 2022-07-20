using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (Input.GetKey(KeyCode.X))
            {
                /*Here, based on the object, we want to perform an action
                For a sign, we display text
                For a fruit, we add a fruit to inventory and change the sprite

                There are many use cases for this. We will decide which thing to do based on the object's tag
                that way I don't need a different script for each interactable object.
                On the downside, the script could become very long
                 */

                if (gameObject.tag == "FullTree")
                {
                    Debug.Log("Hello!");
                    fullTree();
                }
                else if (other.gameObject.tag == "EmptyTree")
                {
                    emptyTree();
                }
            }
        }
    }

    private void fullTree()
    {
        //change the sprite of the tree
        spriteRenderer.sprite = newSprite;

        //change the object's tag
        //make text "you de-leaved the tree"
    }

    private void emptyTree()
    {
        //make text "the tree has already been de-leaved"
    }

}
