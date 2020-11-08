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

    [Header("Dialog box")]
    public GameObject dialogBox;
    public GameObject dialogBackground;
    public TextMeshProUGUI uiboxText;
    public TextMeshProUGUI costsText;


    [Header("Red Flash")]
    public FlashScreen flashScreen;

    [Header("Minimap Canvas")]
    public MiniMapManager miniMapManager;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!miniMapManager.MiniMapGenerator.minimapTextureCanvas.activeSelf) Cursor.lockState = CursorLockMode.Confined;
            else Cursor.lockState = CursorLockMode.Locked;

            miniMapManager.openMinimap(!miniMapManager.MiniMapGenerator.minimapTextureCanvas.activeSelf);
        } 

    }

    public void setBones(string amount)
    {
        BonesCount.text = amount;

        LeanTween.scale(BonesLogo.gameObject, new Vector3(1.3f, 0.7f, 1), 0.2f).setEaseOutQuad().setOnComplete(() =>
        {
            LeanTween.scale(BonesLogo.gameObject, Vector3.one, 0.4f).setEaseOutBounce();
        });
    }

    public void showText(bool active, string text, int costs, float playRate)
    {
        uiboxText.gameObject.SetActive(false);
        costsText.gameObject.SetActive(false);

        if (!active) dialogBackground.gameObject.transform.localScale = new Vector3(0, 0, 0);

        dialogBox.SetActive(active);

        LeanTween.scale(dialogBackground.gameObject, new Vector3(0.1f, 0.8f, 1), 0.3f).setEaseOutQuint().setOnComplete(() =>
        {
            uiboxText.text = text;
            costsText.text = "Cost: " + costs.ToString();
            uiboxText.gameObject.SetActive(true);
            costsText.gameObject.SetActive(true);

            LeanTween.scale(dialogBackground.gameObject, new Vector3(1f, 1f, 1), 0.5f).setEaseOutElastic().setOnComplete(() =>
            {

            });
        });
    }

    public void showErrorMessage(AltarUpgrade upgrade, float duration)
    {
        uiboxText.text = "NOT ENOUGH BONES";
        costsText.text = "";

        Image d = dialogBackground.GetComponent<Image>();
        d.color = Color.red;

        LeanTween.moveLocalX(dialogBox, 100f, duration).setEaseOutBounce().setOnComplete(() =>
        {
            LeanTween.moveLocalX(dialogBox, 0, duration).setEaseOutBounce().setOnComplete(() =>
            {
                d.color = Color.white;
                uiboxText.text = upgrade.uiText;
                costsText.text = "Cost: " + upgrade.BoneCost.ToString();
            });
        });
    }


    public void setStamina()
    {
        if (PlayerController.Instance != null)
        {
            staminaSlider.maxValue = PlayerController.Instance.unit.stats.stamina + PlayerController.Instance.unit.upgradeHandler.maxStaminaUpgrade;
            staminaSlider.value = PlayerController.Instance.unit.currentStamina;
        }
    }

    public void setHealth()
    {
        if (PlayerController.Instance != null)
        {
            healthSlider.maxValue = PlayerController.Instance.unit.stats.health + PlayerController.Instance.unit.upgradeHandler.maxHealthUpgrade;

            LeanTween.value(healthSlider.value, PlayerController.Instance.unit.currentHealth, 1f).setEaseOutBounce().setOnUpdate((float value) =>
            {
                healthSlider.value = value;
            });
        }
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
