using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>().GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    Debug.Log("Coletou Item");
                    collision.gameObject.SetActive(false);
                    inventory.isFull[i] = true;
                    inventory.slots[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    inventory.slots[i].SetActive(true);
                    Destroy(collision.gameObject);
                    break;
                }
            }
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    Debug.Log("Coletou Item");
                    collision.collider.gameObject.SetActive(false);
                    inventory.isFull[i] = true;
                    inventory.slots[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    inventory.slots[i].SetActive(true);
                    Destroy(collision.gameObject);
                    break;
                }
            }
        }
    }
    */
}
