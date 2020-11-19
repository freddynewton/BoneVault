using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/SummonMinionsUdok")]
public class SummonMinions : ActionAI
{
    public List<GameObject> enemyTypes = new List<GameObject>();

    public override void use(UtilityAIHandler controller)
    {
        RaycastHit hit;

        Physics.Raycast(controller.gameObject.transform.position, Vector3.down, out hit);
        LeanTween.moveY(controller.gameObject, hit.point.y, 0.7f).setEaseOutSine().setOnComplete(() =>
        {
            LeanTween.moveY(controller.gameObject, -40f, 2f).setEaseOutSine().setDelay(6f);
        });

        for (int i = 0; i < controller.bossUdokEnemyUnit.minionsLivingCount; i++)
        {
            controller.unit.animator.SetTrigger("Summon");
            GameObject e = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], new Vector3(controller.transform.position.x + Random.Range(-1f, 1f), PlayerController.Instance.transform.position.y, controller.transform.position.z + Random.Range(-1f, 1f)), Quaternion.identity, controller.bossUdokEnemyUnit.bossRoom.transform) as GameObject;

            controller.bossUdokEnemyUnit.spawnedMinions.Add(e.GetComponent<EnemyUnit>());
        }
    }
}