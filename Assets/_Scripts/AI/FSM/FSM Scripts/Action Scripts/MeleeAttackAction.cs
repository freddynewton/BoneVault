using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/MeleeAttackAction")]
public class MeleeAttackAction : StateAction
{
    public override void Action(StateMachine sm)
    {
        if (sm.isTimerDone(Random.Range(sm.eUnit.enemyStats.attackRateMin, sm.eUnit.enemyStats.attackRateMax)))
        {
            sm.eUnit.animator.SetTrigger("Attack");
            SoundManager.Instance.playRandomSFX(sm.eUnit.attackSFX, sm.eUnit.audioSource, 0.8f, 1.2f);
        }
    }
}
