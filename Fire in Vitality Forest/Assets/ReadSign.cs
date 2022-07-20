using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadSign : MonoBehaviour
{

    public string text;
    [SerializeField] private bool isInRange;
    private bool isReading;



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Player")//other.gameobject refers to the colliding object. In this case, the player
        {
            Debug.Log("Here!");
            isInRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Here2");
        if (other.gameObject.name == "Player")
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                readSign();
            }
        }
    }

    private void readSign()
    {
        Debug.Log(text);
    }
}
