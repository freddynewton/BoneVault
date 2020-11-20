using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/SummonMinionsUdok")]
public class SummonMinions : ActionAI
{
    public List<GameObject> enemyTypes = new List<GameObject>();

    public override void use(UtilityAIHandler controller)
    {
        if (controller.bossUdokEnemyUnit.returnLivingMinions() < controller.bossUdokEnemyUnit.minionsLivingCount)
        {
            RaycastHit hit;

            Physics.Raycast(controller.gameObject.transform.position, Vector3.down, out hit);
            LeanTween.moveY(controller.gameObject, hit.point.y + Vector3.up.y, 1.5f).setEaseOutSine().setOnComplete(() =>
            {
                LeanTween.moveY(controller.gameObject, -40f, 3f).setEaseOutSine().setDelay(8f);
            });

            controller.unit.animator.SetTrigger("Summon");
            GameObject e = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], new Vector3(controller.transform.position.x, hit.point.y, controller.transform.position.z), Quaternion.identity, controller.bossUdokEnemyUnit.bossRoom.transform) as GameObject;

            controller.bossUdokEnemyUnit.spawnedMinions.Add(e.GetComponent<EnemyUnit>());
        }
    }
}