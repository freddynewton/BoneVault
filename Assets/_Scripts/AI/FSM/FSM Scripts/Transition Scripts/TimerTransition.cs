using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/TimerTransition")]
public class TimerTransition : Transition
{
    public float minTime = 5;
    public float maxTime = 15;

    public override bool CheckTransition(StateMachine sm)
    {
        return sm.isTransitionTimerDone(Random.Range(minTime, maxTime));
    }
}
