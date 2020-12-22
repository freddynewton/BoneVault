using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCanvas : MonoBehaviour
{
    public GameObject titleText;
    public GameObject mainMenuButton;

    public void Init()
    {
        UiManager.Instance.HUDCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        mainMenuButton.transform.localScale = Vector3.zero;

        titleText.SetActive(true);
        LeanTween.moveLocalY(titleText, 200, 2).setEaseOutBack().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            mainMenuButton.SetActive(true);

            LeanTween.scale(mainMenuButton, Vector3.one, 1f).setIgnoreTimeScale(true).setEaseOutBack().setDelay(0.5f);
        });
    }

    public void MainMenuButton()
    {
        // Load Main Menu
        titleText.SetActive(false);
        titleText.transform.position = new Vector3(0, 1000, 0);
        mainMenuButton.SetActive(false);

        if (SceneManagerHelper.Instance != null) SceneManagerHelper.Instance.loadMainMenu();

    }
}
