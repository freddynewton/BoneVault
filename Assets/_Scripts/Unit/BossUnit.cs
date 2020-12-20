using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUnit : EnemyUnit
{
    [Header("Boss Unit Setting")]
    public BossRoom bossRoom;
    public int phase = 1;

    public override void death()
    {
        foreach (EnemyUnit e in StateMachineController.Instance.enemyUnits)
        {
            e.death();
        }

        base.death();
    }
}
