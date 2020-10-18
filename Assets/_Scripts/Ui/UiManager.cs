using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [Header("HUD")]
    public GameObject HUDCanvas;
    public Slider healthSlider;
    public Slider staminaSlider;

    public WeaponUI weaponUI;

    [Header("Bones")]
    public TextMeshProUGUI BonesCount;
    public Image BonesLogo;


    [Header("Red Flash")]
    public FlashScreen flashScreen;
    
    public void setBones(string amount)
    {
        BonesCount.text = amount;

        LeanTween.scale(BonesLogo.gameObject, new Vector3(1.3f, 0.7f, 1), 0.2f).setEaseOutQuad().setOnComplete(() => {
            LeanTween.scale(BonesLogo.gameObject, Vector3.one, 0.4f).setEaseOutBounce();
        });
    }


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
