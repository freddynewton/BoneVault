using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/Boss/Udok/HaveEnoughFireballs")]
public class HaveEnoughFireballs : Transition
{
    public override bool CheckTransition(StateMachine sm)
    {
        return sm.udokUnit.fireBalls.Count == sm.udokUnit.maxFireBalls;
    }
}
