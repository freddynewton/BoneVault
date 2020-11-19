using UnityEngine;

[CreateAssetMenu(menuName = "Altar/Upgrade/IncreaseMaxStaminaAltarUpgrade")]
public class IncreaseMaxStamina : AltarUpgrade
{
    [Header("Amount Max Stamina Upgrade")]
    public int maxStaminaUpgradeValue = 1;

    public override void use()
    {
        PlayerController.Instance.unit.upgradeHandler.maxStaminaUpgrade += maxStaminaUpgradeValue;
    }
}