using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    private State _currentState;
    [Header("First state in the Array will be the Starting State")] public State[] States;


    [Header("Unit Type")]
    public EnemyUnit eUnit;
    public WrathEnemy wrathUnit;
    public MeleeEnemyUnit meleeUnit;
    public BossUdokEnemyUnit udokUnit;
    public BossUnit bossUnit;

    private float timeTicker;
    private float transitionTicker;

    public bool isTransitionTimerDone(float time)
    {
        if (transitionTicker <= 0)
        {
            transitionTicker = time;
            return true;
        }
        else
        {
            transitionTicker -= Time.deltaTime - StateMachineController.Instance.checkTransitionRate;
            return false;
        }
    }

    public bool isTimerDone(float time)
    {
        if (timeTicker <= 0)
        {
            timeTicker = time;
            return true;
        }
        else
        {
            timeTicker -= Time.deltaTime;
            return false;
        }
    }

    public void Tick()
    {
        // Check if unit == null
        if (States.Length == 0) return;
        if (_currentState == null) _currentState = States[0];

        // Iterate through all current state Actions
        foreach (StateAction sa in _currentState.Actions)
        {
            sa.Action(this);
        }
    }

    public void CheckTransition()
    {
        if (States.Length == 0) return;
        if (_currentState == null) _currentState = States[0];
        if (_currentState.Transitions.Length == 0) return;

        foreach (State.transStruct ts in _currentState.Transitions)
        {
            bool transCheck = ts.transition.CheckTransition(this);

            if (transCheck && !ts.isFalseTrue)
            {
                _currentState = ts.toState;
                timeTicker /= 4;
            }
            else if (!transCheck && ts.isFalseTrue)
            {
                _currentState = ts.toState;
                timeTicker /= 4;
            }
        }
    }
}
