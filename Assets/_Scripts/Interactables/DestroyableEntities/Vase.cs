using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vase : DestroyableEntity
{
    public List<GameObject> dropItems = new List<GameObject>();
    public int itemCounts;

    public override void interact()
    {
        Debug.Log("Interact: " + transform.name);
        base.interact();

        if (dropItems.Count != 0)
        {
            for (int i = 0; i < itemCounts; i++)
            {
                Instantiate(dropItems[Random.Range(0, dropItems.Count)], gameObject.transform.position, Quaternion.identity, null);
            }
        }
    }
}
