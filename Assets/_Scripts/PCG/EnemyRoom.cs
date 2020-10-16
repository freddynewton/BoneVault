using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.Networking;
using System.Linq;

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
    public GameObject[] EnemySpawnPositions;

    [Header("all spawn percentages must add up to 100")]
    [Tooltip("All spawnPercentage needs to fit into 100")]
    public enemyTypes[] EnemyTypes;

    [Header("Enemy Spawn Count")]
    public int waves = 1;
    public int spawnNewWaveUnder = 1;
    public int minEnemies = 2;
    public int maxEnemies = 10;

    [Header("Enemy Spawn Settings")]
    public float spawnRange;
    public Vector3 spawnOffset;

    private List<float> enemyListSpawnPerc;

    public override void Awake()
    {
        base.Awake();
        setEnemyList();
        EnemyContainer.transform.position = gameObject.transform.position;
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

    public bool checkIfRoomCleared()
    {
        if (waves == 0 && returnLivingEnemyCount() == 0)
        {
            setDoors(true);
            return true;
        }

        return false;
    }

    public void startWave()
    {
        if (returnLivingEnemyCount() <= spawnNewWaveUnder && waves > 0)
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
        Debug.Log("Spawn Enemy");
        GameObject e = Instantiate(getRandomEnemy(), EnemySpawnPositions[UnityEngine.Random.Range(0, EnemySpawnPositions.Length - 1)].transform.position, Quaternion.identity, EnemyContainer.gameObject.transform) as GameObject;
    }

    private int returnLivingEnemyCount()
    {
        int check = 0;

        foreach (EnemyUnit e in EnemyContainer.GetComponentsInChildren<EnemyUnit>())
        {
            if (e.currentHealth > 0) check += 1;
        }

        return check;
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (checkIfRoomCleared()) CancelInvoke();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Player"))
        {
            setDoors(false);
            InvokeRepeating("startWave", 1, 1);
            setLights(mainColor);
            InvokeRepeating("checkIfRoomCleared", 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(gameObject.transform.position + spawnOffset, spawnRange);
    }
}
