using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Serializable]
    public class SavedSprites
    {
        public string name;
        public Sprite sprite;
    }

    public bool[] isFull;
    public string[] names;
    [SerializeField] SavedSprites[] savedSprites;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<ItemManager>().hasData)
        {
            isFull = FindObjectOfType<ItemManager>().LoadDataIsFull();
            names = FindObjectOfType<ItemManager>().LoadDataNames();

            for (int i = 0; i < savedSprites.Length; i++)
            {
                for (int j = 0; j < names.Length; j++)
                {
                    if (savedSprites[i].name == names[j])
                    {
                        transform.GetChild(j).transform.GetChild(0).transform.GetComponent<Image>().sprite = savedSprites[i].sprite;
                    }
                }
            }
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(isFull[i] == false)
                transform.GetChild(i).gameObject.SetActive(false);

            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotItem(GameObject item, int index)
    {
        names[index] = item.GetComponent<Item>().title;
        item.gameObject.SetActive(false);
        isFull[index] = true;

        for (int i = 0; i < savedSprites.Length; i++)
        {
            if(savedSprites[i].name == names[index])
            {
                transform.GetChild(index).transform.GetChild(0).transform.GetComponent<Image>().sprite = savedSprites[i].sprite;
            }
        }
        
        transform.GetChild(index).gameObject.SetActive(true);

        Destroy(item);
        SaveItem();
    }

    public void SaveItem()
    {
        FindObjectOfType<ItemManager>().SaveData(isFull, names);
    }
}
