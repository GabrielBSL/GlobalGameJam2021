using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnButton : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject button;
    EventSystem eventSys;

    // Start is called before the first frame update
    void Start()
    {
        // mainMenu = GameObject.Find("MainMenu");
        // Debug.Log(mainMenu);
        // button = GameObject.Find("Button_" + gameObject.name);
        eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
            eventSys.SetSelectedGameObject(button);
        }
    }
}
