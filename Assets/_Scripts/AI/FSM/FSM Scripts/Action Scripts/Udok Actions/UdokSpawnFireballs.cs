using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Boss/Udok/SpawnFireballs")]
public class UdokSpawnFireballs : StateAction
{
    public override void Action(StateMachine sm)
    {
        if (sm.udokUnit.fireBalls.Count < sm.udokUnit.maxFireBalls && sm.isTimerDone(2f))
        {
            sm.udokUnit.fireBalls.Add(Instantiate<GameObject>(sm.udokUnit.fireBallPF, sm.gameObject.transform.position + new Vector3(0, 2, 2), Quaternion.identity, sm.gameObject.transform));
            Projectile proj = sm.udokUnit.fireBalls[sm.udokUnit.fireBalls.Count - 1].GetComponent<Projectile>();
            proj.projectileSource = sm.gameObject;
            proj.bossUdok = sm.udokUnit;
            proj.damageType = sm.udokUnit.damageType;
            proj.startCircleAround();
        }
    }
}
