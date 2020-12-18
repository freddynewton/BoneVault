using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vase : WorldItem
{
    [Header("Vase Settings")]
    public List<GameObject> dropItems = new List<GameObject>();
    public int itemCounts;
    public float dropChance = 0.1f;

    private bool isDestroyed;


    public override void interact()
    {
        base.interact();

        if (dropItems.Count != 0 && !isDestroyed)
        {
            for (int i = 0; i < itemCounts; i++)
            {
                var position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+2, gameObject.transform.position.z);
                if (Random.Range(0, dropChance) < dropChance) Instantiate(dropItems[Random.Range(0, dropItems.Count)], position, Quaternion.identity, null);
            }

            isDestroyed = true;
        }
    }
}
