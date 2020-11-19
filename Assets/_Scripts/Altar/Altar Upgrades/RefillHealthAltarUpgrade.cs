using UnityEngine;

[CreateAssetMenu(menuName = "Altar/Upgrade/RefillHealthAltarUpgrade")]
public class RefillHealthAltarUpgrade : AltarUpgrade
{
    [Header("Refill Health Settings")]
    public int amount = 1;

    public override void use()
    {
        PlayerController.Instance.unit.setHealthPlayer(amount);
    }
}