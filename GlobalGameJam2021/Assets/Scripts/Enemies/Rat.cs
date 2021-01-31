using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    public float speed = 3f;
    int isRight = 1;

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy")
        {
            Turn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            Turn();
        }
    }

    private void Turn()
    {
        isRight *= -1;
        transform.eulerAngles = new Vector3(0, 90 + (90 * isRight), 0);

        speed *= -1;
    }
}
