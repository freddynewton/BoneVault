using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Boss/Udok/ShootFireBall")]
public class ShootFireball : ActionAI
{
    public float waitShootTimer = 0.2f;

    public override void use(UtilityAIHandler controller)
    {
        if (controller.bossUdokEnemyUnit.waitTimer(waitShootTimer))
        {
            GameObject proj = controller.bossUdokEnemyUnit.fireBalls[Random.Range(0, controller.bossUdokEnemyUnit.fireBalls.Count)];
            controller.bossUdokEnemyUnit.fireBalls.Remove(proj);
            controller.unit.animator.SetTrigger("Attack");

            if (proj != null)
                proj.GetComponent<Projectile>().ShootToTarget(100f, PlayerController.Instance.transform.position - controller.transform.position);
        }
    }
}