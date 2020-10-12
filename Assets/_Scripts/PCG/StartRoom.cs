using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : Room
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
