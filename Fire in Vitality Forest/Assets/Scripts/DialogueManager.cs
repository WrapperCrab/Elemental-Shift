using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Controllable
{
    #region Singleton

    public static DialogueManager instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    public TextMeshProUGUI dialogueText;
    public GameObject canvas;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        menuDepth = 1;
        canvas.SetActive(false);
    }

    void Update()
    {
        if (hasControl &&!ControlManager.instance.getSwitched())
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                displayNextSentence();
            }
        }
    }

    public void startDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        displayNextSentence();
    }

    public void displayNextSentence()
    {
        if (sentences.Count == 0)
        {
            endDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void endDialogue()
    {
        ControlManager.instance.switchControl(PlayerMovement.instance);
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
    }
}
