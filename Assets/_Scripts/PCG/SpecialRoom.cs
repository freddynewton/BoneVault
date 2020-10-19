using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialRoom : Room
{
    [Header("Altar")]
    public List<AltarHandler> altars;

    [Header("Raycast Settings")]
    public LayerMask ignoreMeRay;

    private void LateUpdate()
    {
        foreach (AltarHandler altar in altars)
        {
            if (Vector3.Distance(altar.gameObject.transform.position, PlayerController.Instance.transform.position) < altar.distance)
            {

                altar.outline.enabled = true;

            }
            else
            {
                altar.outline.enabled = false;
            }
        }
    }


    public override void Awake()
{
    base.Awake();
    altars = gameObject.transform.GetComponentsInChildren<AltarHandler>().ToList();
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
