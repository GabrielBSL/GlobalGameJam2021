using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    private void Update()
    {
        // skip da sentença
        if (Input.GetKeyDown(KeyCode.F) && dialogueBox.activeInHierarchy == true)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        Debug.Log(sentences);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(ShowingSentece(sentence));
    }

    IEnumerator ShowingSentece (string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        FindObjectOfType<Ghost>().dialogueBox.SetActive(false);
    }
}
