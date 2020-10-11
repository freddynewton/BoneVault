using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

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

    private void Awake()
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

    public GameObject getRandomEnemie()
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
}
