using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCanvas : MonoBehaviour
{
    public GameObject titleText;
    public GameObject mainMenuButton;

    public void Init()
    {
        mainMenuButton.SetActive(false);
        mainMenuButton.transform.localScale = Vector3.zero;

        titleText.SetActive(false);
        titleText.transform.localScale = Vector3.zero;

        UiManager.Instance.HUDCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;

        titleText.SetActive(true);
        LeanTween.scale(titleText, Vector3.one, 2).setEaseOutBack().setIgnoreTimeScale(true).setOnComplete(() =>
        {
            mainMenuButton.SetActive(true);
            LeanTween.scale(mainMenuButton, Vector3.one, 1f).setIgnoreTimeScale(true).setEaseOutBack().setDelay(0.5f);
        });
    }
}