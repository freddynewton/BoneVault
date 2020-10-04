using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool isStaminaReg = true;

    public override void Start()
    {
        base.Start();
        currentStamina = stats.stamina;
    }

    public void updateStamina()
    {
        if (isStaminaReg && currentStamina <= stats.stamina)
        {
            setStamina(Time.deltaTime * stats.staminaRate);
        }
    }

    public void setStamina(float amount)
    {
        currentStamina += amount;

        if (currentStamina > stats.stamina) currentStamina = stats.stamina;
        else if (currentStamina < 0) currentStamina = 0;

        // TODO CALL UI UPDATE FUNCTION
        UiManager.Instance.setStamina();
    }

    public override void Update()
    {
        base.Update();
        updateStamina();
    }

    public override void death()
    {
        base.death();
        // TODO Player Hit effect & "Game over" Scene 
    }

    public override void hit()
    {
        base.hit();
        // TODO Do Player hit effect
    }

    public override void DoDamage(Vector3 damageSrcPos, int damage, float kbForce)
    {
        base.DoDamage(damageSrcPos, damage, kbForce);
        UiManager.Instance.setHealth();
    }
}
