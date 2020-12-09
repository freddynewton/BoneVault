using System.Collections;
using UnityEngine;

public class StartRoom : Room
{
    [Header("Start Room Settings")]
    public Transform PlayerSpawn;

    public override void Awake()
    {
        base.Awake();
        setLights(mainColor);
        StartCoroutine(setSpawnPos());
    }

    public IEnumerator setSpawnPos()
    {
        yield return new WaitForSecondsRealtime(1);
        PlayerController.Instance.transform.position = PlayerSpawn.position + Vector3.up;
        UiManager.Instance.setActivePreparingLevel(false);
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