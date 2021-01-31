using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    
    public string objectName;
    public GameObject dialogueBox;
    public GameObject buttonTrigger;
    bool canBeTrigged, originalPosition, isFlipped;
    public Dialogue dialogue;
    public Transform player; // to look at player when he's near

    private void Start()
    {
        dialogueBox.SetActive(false);
        buttonTrigger.SetActive(false);
        originalPosition = true;
        isFlipped = GetComponent<SpriteRenderer>().flipX;
    }

    private void Update()
    {
        LookToPlayer();

        // mostra o botão para iniciar o diálogo
        if (canBeTrigged == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueBox.SetActive(true);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
        }

        /* skip da sentença
        if (Input.GetKeyDown(KeyCode.F) && dialogueBox.activeInHierarchy == true)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
        */
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

    public void LookToPlayer()
    {
        if (player.transform.position.x > transform.position.x && !isFlipped) // lado direito
        {
            Flip();

            if (originalPosition) originalPosition = false;
            else originalPosition = true;
        }
        else if (player.transform.position.x < transform.position.x && isFlipped) // lado esquerdo
        {
            Flip();

            if (!originalPosition) originalPosition = true;
            else originalPosition = false;
        }

        // fantasma volta pra posição inicial qnd o player se afasta
        if (!originalPosition && !canBeTrigged)
        {
            Flip();
            originalPosition = true;

            // esconde a caixa de dialogo caso ele tenha sido iniciado
            if (dialogueBox.activeSelf == true) {
                dialogueBox.SetActive(false);
            }
        }
    }

    void Flip()
    {
        if (isFlipped)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            isFlipped = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            isFlipped = true;
        }
    }

}
