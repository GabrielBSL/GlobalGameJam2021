using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public string title;
    [SerializeField] string description;
    [SerializeField] int spriteIndex;
    [SerializeField] Sprite icon;
    [SerializeField] public bool picked = false;
}
