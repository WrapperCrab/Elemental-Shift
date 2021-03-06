using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Dialogue dialogue;

    public virtual void interact()
    {
        Debug.Log("in interact()");
    }

    public void triggerDialogue()
    {
        //switch control to textbox
        ControlManager.instance.switchControl(DialogueManager.instance);
        DialogueManager.instance.startDialogue(dialogue);
    }
}
