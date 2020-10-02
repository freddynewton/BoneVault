using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "AI/Actions/SkeletonAttack")]
public class SkeletonAttack : ActionAI
{
    public override void use(UtilityAIHandler controller)
    {
        
    }

    IEnumerator attack(UtilityAIHandler controller, float t)
    {
        yield return new WaitForSecondsRealtime(t);
            foreach (GameObject obj in controller.unit.triggerList)
            {
                obj.GetComponent<Unit>().DoDamage(controller.gameObject.transform.position, controller.unit.stats.damage, 0f);
            }
        
    }
}
