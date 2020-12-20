using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        yield return new WaitForSecondsRealtime(3);

        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(PlayerSpawn.transform.position + new Vector3(0, 2, 0), out navMeshHit, 20f, NavMesh.AllAreas);

        if (navMeshHit.position == null) { StartCoroutine(setSpawnPos()); Debug.Log("Didn't find player spawn pos"); yield break; }

        PlayerController.Instance.transform.position = navMeshHit.position;
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