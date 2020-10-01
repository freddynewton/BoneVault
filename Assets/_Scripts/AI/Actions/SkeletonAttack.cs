using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "AI/Actions/SkeletonAttack")]
public class SkeletonAttack : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        if (!controller.unit.isAttacking)
        {
            controller.unit.isAttacking = true;
            controller.unit.changeAnimationState("Attack");

            controller.StartCoroutine(attack(controller, controller.unit.isAttackingDuration));
            controller.Invoke("controller.unit.changeAttackingBool", controller.unit.isAttackingDuration);
        }
    }

    IEnumerator attack(UtilityAIHandler controller, float t)
    {
        yield return new WaitForSecondsRealtime(t);

        if (!controller.unit.isHit)
        {
            foreach (GameObject obj in controller.unit.playerList)
            {
                obj.GetComponent<Unit>().DoDamage(controller.gameObject.transform.position, controller.unit.stats.damage, 0f);
            }
        }
    }
}
