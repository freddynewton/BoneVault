using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : Room
{
    public override void Awake()
    {
        getAllLights();

        foreach (Light l in lights)
        {
            l.gameObject.SetActive(false);

            SpriteRenderer rend = l.gameObject.GetComponent<SpriteRenderer>();
            if (rend != null) l.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        setLights(mainColor);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
