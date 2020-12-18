using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/Boss/Udok/SummonAction")]
public class UdokSummonAction : StateAction
{
    public override void Action(StateMachine sm)
    {
        if (sm.udokUnit.returnLivingMinions() < sm.udokUnit.minionsLivingCount && !sm.udokUnit.isSpawning)
        {
            sm.udokUnit.spawnMinions();
        }
    }
}
