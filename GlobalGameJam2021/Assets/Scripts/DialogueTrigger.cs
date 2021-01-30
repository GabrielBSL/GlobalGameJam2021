using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public bool canBeTrigged;
    public Dialogue dialogue;
    public GameObject dialogBox;


    private void Update()
    {
        if(canBeTrigged == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogBox.SetActive(true);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
        }
    }

    /*
    public void TriggerDialogue() 
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    */
}
