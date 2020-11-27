public class EnemyBillboard : Billboard
{
    public MeleeEnemyUnit unit;

    public void InvokeFunction()
    {
        unit.CallTriggerDamage();
    }
}