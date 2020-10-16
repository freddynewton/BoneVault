using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBillboard : Billboard
{
    public EnemyUnit unit;

    /// <summary>
    /// Call Function from Unit
    /// </summary>
    /// <param name="type">
    /// 
    /// 0 = Call Trigger Damage from Unit
    /// 
    /// </param>
    public void InvokeFunction(int type)
    {
        switch (type)
        {
            case 0:
                unit.CallTriggerDamage();
                break;
        }

    }
}
