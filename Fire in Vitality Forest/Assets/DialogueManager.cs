using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject canvas;

    private Queue<string> sentences;
    bool isDisplayingText;
    bool justClosedText;//I don't like that I need this, but it works

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        isDisplayingText = false;
        canvas.SetActive(false);
    }

    void Update()
    {
        justClosedText = false;
        if (isDisplayingText)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                displayNextSentence();
            }
        }
    }

    public void startDialogue(Dialogue dialogue)
    {
        Debug.Log("starting conversation");
        
        
        
        isDisplayingText = true;
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
        Debug.Log("end of conversation");
        canvas.SetActive(false);
        isDisplayingText = false;
        justClosedText = true;
    }

    public bool getIsDisplayingText()
    {
        return isDisplayingText;
    }

    public bool getJustClosedText()
    {
        return justClosedText;
    }
}
