using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public string name; 
    public Sprite icon;

    public abstract void interact();
}
