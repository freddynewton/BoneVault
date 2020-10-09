using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [Header("HUD")]
    public GameObject HUDCanvas;
    public Slider healthSlider;
    public Slider staminaSlider;

    public WeaponUI weaponUI;
    


    public void setStamina()
    {
        if (PlayerController.Instance != null)
        {
            staminaSlider.maxValue = PlayerController.Instance.unit.stats.stamina;
            staminaSlider.value = PlayerController.Instance.unit.currentStamina;
        }


        // TODO LEANTWEEN
    }

    public void setHealth()
    {
        if (PlayerController.Instance != null)
        {
            healthSlider.maxValue = PlayerController.Instance.unit.stats.health;
            healthSlider.value = PlayerController.Instance.unit.currentHealth;
        }

        // TODO LEANTWEEN
    }

    private IEnumerator loadUi()
    {
        yield return new WaitForEndOfFrame();

        setHealth();
        setStamina();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(loadUi());
    }
}
