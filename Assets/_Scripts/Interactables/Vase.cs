using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets._Scripts.Interactables
{
    public class Vase : DestroyableEntity
    {
        public List<GameObject> dropItems = new List<GameObject>();
        public int itemCounts;

        public override void interact()
        {
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
}