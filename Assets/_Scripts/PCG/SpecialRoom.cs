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
                altar.setOutline(true);

                if (Input.GetKeyDown(KeyCode.E)) altar.use();
            }
            else
            {
                altar.setOutline(false);
            }
        }
    }

    public void fillUpgrades()
    {
        List<AltarUpgrade> upgrades = Resources.LoadAll<AltarUpgrade>("Altar Upgrades").ToList();

        foreach (AltarHandler a in altars)
        {
            a.room = this;
            a.setUpgrade(upgrades[Random.Range(0, upgrades.Count)]);
        }
    }


    public override void Awake()
    {
        base.Awake();
        
        //setLights(mainColor);

        altars = gameObject.transform.GetComponentsInChildren<AltarHandler>().ToList();

        fillUpgrades();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
