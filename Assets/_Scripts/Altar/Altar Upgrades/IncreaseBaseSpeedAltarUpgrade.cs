using UnityEngine;

[CreateAssetMenu(menuName = "Altar/Upgrade/IncreaseBaseSpeedAltarUpgrade")]
public class IncreaseBaseSpeedAltarUpgrade : AltarUpgrade
{
    [Header("Base Speed Upgrade")]
    public float upgradeAmount;

    public override void use()
    {
        PlayerController.Instance.unit.upgradeHandler.baseSpeedPercentageUpgrade += upgradeAmount;
    }
}