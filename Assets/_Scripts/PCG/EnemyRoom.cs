using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.Networking;

public class EnemyRoom : Room
{
    [Serializable]
    public struct enemyTypes
    {
        public GameObject Enemy;

        [Tooltip("Needs to be <= 100")]
        [Range(0, 100)] public float spawnPercentage;
    }

    [Header("Enemy Room Settings")]
    public GameObject EnemyContainer;

    [Header("all spawn percentages must add up to 100")]
    [Tooltip("All spawnPercentage needs to fit into 100")]
    public enemyTypes[] EnemyTypes;

    [Header("Enemy Count")]
    public int waves = 1;
    public int spawnNewWaveUnder = 1;
    public int minEnemies = 2;
    public int maxEnemies = 10;

    private List<float> enemyListSpawnPerc;

    public override void Awake()
    {
        base.Awake();
        setEnemyList();
    }

    public void setEnemyList()
    {
        enemyListSpawnPerc = new List<float>();

        foreach (enemyTypes e in EnemyTypes)
        {
            enemyListSpawnPerc.Add(e.spawnPercentage);
        }
    }

    public GameObject getRandomEnemy()
    {
        float randomValue = UnityEngine.Random.Range(0, 100);

        foreach (enemyTypes e in EnemyTypes)
        {
            if (randomValue > e.spawnPercentage) continue;
            else return e.Enemy;
        }

        // Won't happen if set correctly
        return EnemyTypes[0].Enemy;
    }

    public void startWave()
    {
        if (EnemyContainer.transform.childCount <= spawnNewWaveUnder && waves > 0)
        {
            waves -= 1;

            for (int i = 0; i < UnityEngine.Random.Range(minEnemies, maxEnemies); i++)
            {
                spawnRandomEnemy();
            }
        }
    }

    public void spawnRandomEnemy()
    {
        // TODO Spawn VFX

        Instantiate(getRandomEnemy(), getRandomPos(), Quaternion.identity, EnemyContainer.gameObject.transform);
    }

    public Vector3 getRandomPos()
    {
        Vector3 pos = new Vector3();

        Vector3 randomPoint = gameObject.transform.position + UnityEngine.Random.insideUnitSphere * 3;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            pos = hit.position;
        }

        return pos;
    }

    private void Update()
    {
        if (waves == 0 && EnemyContainer.transform.childCount == 0)
        {
            setDoors(true);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Player"))
        {
            setDoors(false);
            InvokeRepeating("startWave", 1, 1);
            setLights(mainColor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, 4);
    }
}
