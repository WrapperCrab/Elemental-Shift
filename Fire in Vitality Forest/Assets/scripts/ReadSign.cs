using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadSign : MonoBehaviour
{
    public string text;
    private bool isReading;//So we can't open multiple text windows at once

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if ((Input.GetKey(KeyCode.X)) && (!isReading))
            {
                isReading = true;
                readSign();
            }
        }
    }

    private void readSign()
    {
        Debug.Log(text);
    }
}
