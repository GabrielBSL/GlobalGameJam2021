using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public bool[] isFull;
    public string[] names;

    public bool hasData = false;

    private void Awake()
    {
        var objects = FindObjectsOfType<ItemManager>();

        if(objects.Length > 1)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject == gameObject)
                {
                    Destroy(objects[i].gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveData(bool[] _isFull, string[] _names)
    {
        isFull = _isFull;
        names = _names;
        hasData = true;
    }

    public bool[] LoadDataIsFull()
    {
        return isFull;
    }

    public string[] LoadDataNames()
    {
        return names;
    }
}
