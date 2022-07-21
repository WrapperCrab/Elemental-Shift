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
    }

    void FixedUpdate()
    {
        // update the player's velocity
        if (isRunning)
        {
            rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + (newVel.normalized * runSpeed * Time.fixedDeltaTime));
            Debug.Log("running");
        }
        else
        {
            rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + (newVel.normalized * walkSpeed * Time.fixedDeltaTime));
            Debug.Log("walking");
        }
        //!!!Movement actas a little weird when 3 inputs at once.
        //Can't run in up-right direction. This is a problem
    }

    private void OnTriggerStay2D(Collider2D other)//Note, other ALWAYS represents the object the current script does NOT belong to
    {
        //!!!In the future, I will need to make sure I only activate one trigger at a time
        if (Input.GetKey(KeyCode.X) && canInteract())//!!!In the future, I will also want to make sure the player is facing the correct direction
        {
            //Activate other.gameObject's interaction method
            other.gameObject.GetComponent<Interaction>().interact();//I can't believe this actually works. It's like magic.
        }
    }

    public bool canInteract()
    {
        return !(dialogueManager.GetComponent<DialogueManager>().getIsDisplayingText());
    }

}
