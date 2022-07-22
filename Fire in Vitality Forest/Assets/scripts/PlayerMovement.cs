using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidBodyPlayer;
    public DialogueManager dialogueManager;

    public float walkSpeed;
    public float runSpeed;

    Vector2 newVel;
    bool isRunning;
    bool justPressedX;
    int numTriggers;


    // Update is called once per frame
    void Update()
    {
        newVel.x = Input.GetAxisRaw("Horizontal");
        newVel.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space))//run button
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.X) && canInteract() && isInTrigger())
        {
            justPressedX = true;
        }
        Debug.Log(justPressedX);
    }

    void FixedUpdate()
    {
        // update the player's velocity
        if (canInteract())//otherwise, no change to movement is made
        {
            if (isRunning)
            {
                rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + (newVel.normalized * runSpeed * Time.fixedDeltaTime));
            }
            else
            {
                rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + (newVel.normalized * walkSpeed * Time.fixedDeltaTime));
            }
            //!!!Movement actas a little weird when 3 inputs at once.
            //Can't run in up-right direction. This is a problem
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        numTriggers++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        numTriggers--;
    }

    private void OnTriggerStay2D(Collider2D other)//Note, other ALWAYS represents the object the current script does NOT belong to
    {
        //!!!In the future, I will need to make sure I only activate one trigger at a time
        if (justPressedX)//!!!In the future, I will also want to make sure the player is facing the correct direction
        {
            justPressedX = false;
            //Activate other.gameObject's interaction method
            other.gameObject.GetComponent<Interaction>().interact();//I can't believe this actually works. It's like magic.
        }
    }

    public bool canInteract()//This function will be expanded upon for other circumstances
    {
        bool isNotDisplayingText = !(dialogueManager.GetComponent<DialogueManager>().getIsDisplayingText());//want true
        bool didNotJustClose = !(dialogueManager.GetComponent<DialogueManager>().getJustClosedText());//want true
        return (isNotDisplayingText && didNotJustClose);
    }

    public bool isInTrigger()
    {
        if (numTriggers > 0)
        {
            return true;
        }
        return false;
    }

}
