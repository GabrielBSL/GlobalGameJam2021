using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepCavern : MonoBehaviour
{
    private bool playerOnContact = false;
    private bool transitioning = false;
    public GameObject enterCircle = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerOnContact && !transitioning)
        {
            transitioning = true;
            FindObjectOfType<GoToScene>().RemoteTransite("DeepCavern1", GoToScene.DestinationIdenifier.C, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerOnContact = true;
            enterCircle.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerOnContact = false;
            enterCircle.SetActive(false);
        }
    }
}
