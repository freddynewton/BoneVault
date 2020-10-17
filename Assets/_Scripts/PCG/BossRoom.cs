using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{

    [Header("TrapDoor")]
    public GameObject[] TrapDoor;

    [Header("Button")]
    public GameObject Button;

    public void openTrapDoor()
    {
        foreach(GameObject trap in TrapDoor)
        {
            LeanTween.rotateX(trap, 90, 4).setEaseOutBounce();
        }
    }


    public override void Awake()
    {
        base.Awake();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        setLights(mainColor);
        openTrapDoor();
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
