using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveWhenSelected : MonoBehaviour
{
    private EventSystem _event;
    public GameObject selectIndicator;

    void Start()
    {
        _event = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void Update()
    {
        if (_event.currentSelectedGameObject == gameObject)
        {
            selectIndicator.SetActive(true);
        }
        else
        {
            if (selectIndicator.activeSelf)
            {
                selectIndicator.SetActive(false);
            }
        }
    }
}
