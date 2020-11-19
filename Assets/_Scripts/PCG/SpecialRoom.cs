using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialRoom : Room
{
    [Header("Altar")]
    public List<AltarHandler> altars;

    [Header("Special Room Lights")]
    public GameObject lightsParentSpecialRoom;

    public List<Light> specialLights;

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
        getAllLights();
        getAllDoors();
        setDoors(true);

        altars = gameObject.transform.GetComponentsInChildren<AltarHandler>().ToList();

        fillUpgrades();

        specialLights = lightsParentSpecialRoom.GetComponentsInChildren<Light>().ToList();

        foreach (Light l in specialLights)
        {
            SpriteRenderer rend = l.gameObject.gameObject.GetComponent<SpriteRenderer>();
            rend.enabled = false;
            l.gameObject.SetActive(false);
        }
    }

    public IEnumerator setSpecialRoomLights(Color color, int idx)
    {
        yield return new WaitForSecondsRealtime(activateLightDelay);

        Light l = specialLights[idx];

        SpriteRenderer rend = l.gameObject.gameObject.GetComponent<SpriteRenderer>();

        if (rend != null)
        {
            rend.enabled = true;
            rend.color = color;
        }

        l.color = color;
        l.gameObject.SetActive(true);

        if (idx < specialLights.Count - 1) StartCoroutine(setSpecialRoomLights(color, idx += 1));
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        StartCoroutine(setSpecialRoomLights(mainColor, 0));
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}