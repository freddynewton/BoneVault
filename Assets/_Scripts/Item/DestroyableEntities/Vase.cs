using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vase : WorldItem
{
    public List<GameObject> dropItems = new List<GameObject>();
    public int itemCounts;
    public float dropChance = 0.1f;

    public override void interact()
    {
        base.interact();

        if (dropItems.Count != 0)
        {
            for (int i = 0; i < itemCounts; i++)
            {
                if (Random.Range(0, dropChance) < dropChance) Instantiate(dropItems[Random.Range(0, dropItems.Count)], gameObject.transform.position, Quaternion.identity, null);
            }
        }
    }
}
