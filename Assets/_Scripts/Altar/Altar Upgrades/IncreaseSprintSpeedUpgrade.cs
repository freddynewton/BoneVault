using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Altar/Upgrade/IncreaseSprintSpeedAltarUpgrade")]
public class IncreaseSprintSpeedUpgrade : AltarUpgrade
{
    [Header("Sprint Speed Upgrade")]
    public float upgradeAmount;

    public override void use()
    {
        PlayerController.Instance.unit.upgradeHandler.sprintSpeedPercentageUpgrade += upgradeAmount;
    }
}
