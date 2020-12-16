using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController Instance { get; private set; }

    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();

    [Range(0f, 1f)] public float checkTransitionRate = 0.2f;
    private float _transitionRate = 0;

    // Update is called once per frame
    void Update()
    {
        Iterate();

        if (_transitionRate <= 0)
        {
            CheckTransitions();
            _transitionRate = checkTransitionRate;
        } else _transitionRate -= Time.deltaTime;
    }

    private void Iterate()
    {
        foreach (EnemyUnit e in enemyUnits)
        {
            if (e.sm != null) e.sm.Tick();
        }
    }

    private void CheckTransitions()
    {
        foreach (EnemyUnit e in enemyUnits)
        {
            if (e.sm != null) e.sm.CheckTransition();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
