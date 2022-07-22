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
        canvas.SetActive(false);
    }

    void Update()
    {
        if (hasControl &&!ControlManager.instance.getSwitched())
        {
            Debug.Log("here");
            if (Input.GetKeyDown(KeyCode.X))
            {
                displayNextSentence();
            }
        }
        Debug.Log("get switched is "+ControlManager.instance.getSwitched());
    }

    public void startDialogue(Dialogue dialogue)
    {
        canvas.SetActive(true);

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
        canvas.SetActive(false);
        ControlManager.instance.switchControl(PlayerMovement.instance);
    }
}
