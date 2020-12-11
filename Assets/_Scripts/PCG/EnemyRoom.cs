using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : Room
{
    [Header("Enemy Room Settings")]
    public GameObject EnemyContainer;

    public GameObject[] EnemySpawnPositions;

    [Header("Enemy Types")]
    public List<GameObject> EnemyTypesPF = new List<GameObject>();

    [Header("Enemy Spawn Count")]
    public int waves = 1;
    public int spawnNewWaveUnder = 1;
    public int minEnemies = 2;
    public int maxEnemies = 10;
    private bool isCleared = false;

    [Header("Enemy Spawn Settings")]
    public float spawnRange;
    public Vector3 spawnOffset;


    public override void Awake()
    {
        base.Awake();
        EnemyContainer.transform.position = gameObject.transform.position;
    }

    public bool checkIfRoomCleared()
    {
        if (waves == 0 && returnLivingEnemyCount() == 0)
        {
            setDoors(true);
            isCleared = true;
            setLights(secColor);
            StartCoroutine(fadeMusic(GetComponent<AudioSource>(), 3f, true));           
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
        GameObject e = Instantiate(EnemyTypesPF[UnityEngine.Random.Range(0, EnemyTypesPF.Count)], EnemySpawnPositions[UnityEngine.Random.Range(0, EnemySpawnPositions.Length - 1)].transform.position, Quaternion.identity, EnemyContainer.gameObject.transform) as GameObject;
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

        if (other.gameObject.CompareTag("Player") && !isCleared)
        {
            if (!GetComponent<AudioSource>().isPlaying) StartCoroutine(fadeMusic(GetComponent<AudioSource>(), 3f, false));
            setDoors(false);
            InvokeRepeating("startWave", 1, 1);
            setLights(mainColor);
            InvokeRepeating("checkIfRoomCleared", 3, 1);           
        }
    }
}