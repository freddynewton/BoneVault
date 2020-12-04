using UnityEngine;

public class PlayerUpgradeHandler : MonoBehaviour
{
    public int maxHealthUpgrade;

    public float sprintSpeedPercentageUpgrade = 1;
    public float baseSpeedPercentageUpgrade = 1;
    public float sprintSpeedPercentageCostsUpgrade = 1;

    public float staminaRatePercentageUpgrade = 1;
    public int maxStaminaUpgrade;

    public void resetUpgrades()
    {
        maxHealthUpgrade = 0;
        sprintSpeedPercentageUpgrade = 1;
        baseSpeedPercentageUpgrade = 1;
        sprintSpeedPercentageCostsUpgrade = 1;
        staminaRatePercentageUpgrade = 1;
        maxStaminaUpgrade = 0;
    }
}