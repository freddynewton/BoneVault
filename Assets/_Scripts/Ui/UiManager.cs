using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [Header("HUD")]
    public GameObject HUDCanvas;
    public Slider healthSlider;
    public Slider staminaSlider;
    public WeaponUI weaponUI;
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
    public GameObject miniMapCanvas;

    [Header("End of Level Screen")]
    public EndOfLevelCanvas EndOfLevelCanvas;

    [Header("Preparing Level Screen")]
    public Canvas preparingLevelCanvas;

    [Header("Main Menu Canvas")]
    public Canvas mainMenuCanvas;

    [Header("Death Canvas")]
    public DeathCanvas DeathCanvas;

    [Header("Options Menu")]
    public OptionsMenuHandler optionsMenu;

    public void setActivePreparingLevel(bool active) => preparingLevelCanvas.gameObject.SetActive(active);
    public void setActiveMainMenuCanvas(bool active) => mainMenuCanvas.gameObject.SetActive(active);
    public void setActiveMiniMap(bool active) => miniMapCanvas.gameObject.SetActive(active);
    public void setActiveHUD(bool active) => HUDCanvas.gameObject.SetActive(active);
    public void setActiveDeathCanvas(bool active) => DeathCanvas.gameObject.SetActive(active);
    public void setActiveOptionsMenu(bool active) => optionsMenu.gameObject.SetActive(active);


    public void Death()
    {
        UiManager.Instance.setActiveDeathCanvas(true);
        DeathCanvas.Init();
        Time.timeScale = 0;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (optionsMenu.gameObject.activeSelf)
            {
                optionsMenu.closeAllTabs();
            }

            setActiveOptionsMenu(!optionsMenu.gameObject.activeSelf);


            if (optionsMenu.gameObject.activeSelf) Cursor.lockState = CursorLockMode.Confined;
            else Cursor.lockState = CursorLockMode.Locked;

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (optionsMenu.gameObject.activeSelf) setActiveMainMenuCanvas(false);
                else setActiveMainMenuCanvas(true);

                if (mainMenuCanvas.gameObject.activeSelf) Cursor.lockState = CursorLockMode.Confined;
                else Cursor.lockState = CursorLockMode.Locked;
            }
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
            staminaSlider.maxValue = PlayerController.Instance.unit.playerStats.stamina + PlayerController.Instance.unit.upgradeHandler.maxStaminaUpgrade;
            staminaSlider.value = PlayerController.Instance.unit.currentStamina;
        }
    }

    public void setHealth()
    {
        if (PlayerController.Instance != null)
        {
            healthSlider.maxValue = PlayerController.Instance.unit.baseStats.maxHealth + PlayerController.Instance.unit.upgradeHandler.maxHealthUpgrade;

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