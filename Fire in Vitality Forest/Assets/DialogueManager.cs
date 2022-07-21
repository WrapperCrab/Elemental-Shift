using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    private Queue<string> sentences;
    bool isDisplayingText;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (isDisplayingText)
        {
            if (Input.GetKeyDown(KeyCode.X))//!!!This is too fast
            {
                displayNextSentence();
            }
        }
    }

    public void startDialogue(Dialogue dialogue)
    {
        Debug.Log("starting conversation");
        
        
        
        isDisplayingText = true;

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
        isDisplayingText = false;
    }

    public bool getIsDisplayingText()
    {
        return isDisplayingText;
    }
}
