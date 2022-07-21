using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DisplayText : MonoBehaviour
{
    //!!!Massive changes will be made to this script
    public TMP_Text tMP;
    private bool displayingText;

    public void displayText(string text)
    {
        tMP.text = text;
        displayingText = true;
    }

    void Update()
    {
        if (displayingText)
        {
            if (Input.GetKey(KeyCode.X))
            {
                tMP.text = "";
                displayingText = false;
            }
        }
    }
}
