using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Controllable
{
    #region Singleton

    public static PlayerMovement instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerMovement found!");
            return;
        }
        instance = this;
    }

    #endregion

    public Rigidbody2D rigidBodyPlayer;
    public DialogueManager dialogueManager;

    public float walkSpeed;
    public float runSpeed;

    public SpriteRenderer fill;
    public SpriteRenderer outline;
    public Sprite[] fillSprites;
    public Sprite[] outlineSprites;
    public Element color;

    Vector2 newVel;
    bool isRunning;
    
    bool justPressedX;

    int numTriggers;

    void Start()
    {
        hasControl = true;
        menuDepth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        setColor(TeamManager.instance.getFirstPlayerColor());//updates the player's color


        newVel.x = Input.GetAxisRaw("Horizontal");
        newVel.y = Input.GetAxisRaw("Vertical");

        if (hasControl && !ControlManager.instance.getSwitched())
        {
            if (Input.GetKey(KeyCode.Space))//run button
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
            updateSprite();

            if (Input.GetKeyDown(KeyCode.X) && isInTrigger())//used for interacting with triggers
            {
                justPressedX = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                ControlManager.instance.switchControl(OverworldMenuControl.instance);
            }
        }
    }

    void FixedUpdate()
    {
        // update the player's velocity
        if (hasControl)//otherwise, no change to movement is made
        {
            if (isRunning)
            {
                rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + (newVel.normalized * runSpeed * Time.fixedDeltaTime));
            }
            else
            {
                rigidBodyPlayer.MovePosition(rigidBodyPlayer.position + (newVel.normalized * walkSpeed * Time.fixedDeltaTime));
            }
            //!!!Movement acts a little weird when 3 inputs at once.
            //Can't run in up-right direction. This is a problem
        }

    }

    void updateSprite()
    {//based on current player input, changes the sprite. 
        if (newVel.x > 0)
        {//right facing
            fill.sprite = fillSprites[0];
            fill.flipX = false;
            outline.sprite = outlineSprites[0];
            outline.flipX = false;
        }else if (newVel.x < 0)
        {//left facing
            //reflect sprites
            fill.sprite = fillSprites[1];
            fill.flipX = true;
            outline.sprite = outlineSprites[1];
            outline.flipX = true;
        }
        if (newVel.y > 0)
        {//up facing
            fill.sprite = fillSprites[2];
            outline.sprite = outlineSprites[2];
        }
        else if (newVel.y < 0)
        {//down facing
            fill.sprite = fillSprites[3];
            outline.sprite = outlineSprites[3];
        }
    }

    void setColor(Element newColor)
    {
        color = newColor;
        updateColor();
    }

    void updateColor()
    {
        fill.color = ElementManager.instance.getColorHue(color);
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
            other.gameObject.GetComponent<Interaction>().interact();
        }
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
