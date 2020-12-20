using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/Boss/Udok/SummonMinionTransition")]
public class SummonMinionTransition : Transition
{
    public override bool CheckTransition(StateMachine sm)
    {
        return sm.udokUnit.isSpawning;
    }
}
