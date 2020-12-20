using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/Boss/Udok/HaveEnoughFireballs")]
public class HaveEnoughFireballs : Transition
{
    public bool empty;

    public override bool CheckTransition(StateMachine sm)
    {
        if (empty)
        {
            return sm.udokUnit.fireBalls.Count == 0;
        } else
        {
            return sm.udokUnit.fireBalls.Count == sm.udokUnit.maxFireBalls;
        }
    }
}
