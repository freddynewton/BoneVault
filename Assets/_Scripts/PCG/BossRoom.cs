using UnityEngine;

public class BossRoom : Room
{
    [Header("TrapDoor")]
    public GameObject[] TrapDoor;

    [Header("Portal Room")]
    public Door portalDoor;

    [Header("Button")]
    public GameObject Button;

    public void openTrapDoor()
    {
        foreach (GameObject trap in TrapDoor)
        {
            LeanTween.rotateX(trap, 90, 4).setEaseOutBounce();
        }
    }

    public override void Awake()
    {
        getAllLights();
        setDoors(true);
        portalDoor.closeDoor();

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
        openTrapDoor();
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}