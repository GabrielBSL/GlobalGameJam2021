using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    
    public string objectName;
    public GameObject dialogueBox;
    public GameObject buttonTrigger;
    public bool canBeTrigged;
    public Dialogue dialogue;

    private void Start()
    {
        dialogueBox.SetActive(false);
        buttonTrigger.SetActive(false);
    }

    private void Update()
    {
        if (canBeTrigged == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueBox.SetActive(true);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && dialogueBox.activeSelf == true)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            buttonTrigger.SetActive(true);
            canBeTrigged = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            buttonTrigger.SetActive(false);
            canBeTrigged = false;
        }
    }

}
