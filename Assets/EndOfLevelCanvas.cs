using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndOfLevelCanvas : MonoBehaviour
{
    public GameObject title;
    public GameObject infoText;
    public GameObject mainMenuButton;
    public GameObject surveyButton;
    public GameObject socialsButton;

    public void showScreen()
    {
        UiManager.Instance.HUDCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;

        mainMenuButton.transform.localScale = Vector3.zero;
        surveyButton.transform.localScale = Vector3.zero;
        socialsButton.transform.localScale = Vector3.zero;
        infoText.transform.localScale = Vector3.zero;

        title.SetActive(true);
        LeanTween.moveLocalY(title, 100, 2).setEaseOutBack().setIgnoreTimeScale(true).setOnComplete(() => {

            infoText.SetActive(true);
            LeanTween.scale(infoText, Vector3.one, 3f).setEaseOutBounce().setIgnoreTimeScale(true).setOnComplete(() => {

                mainMenuButton.SetActive(true);
                surveyButton.SetActive(true);
                socialsButton.SetActive(true);

                LeanTween.scale(mainMenuButton, Vector3.one, 1f).setIgnoreTimeScale(true).setEaseOutBack().setDelay(0.5f);
                LeanTween.scale(surveyButton, Vector3.one, 1f).setIgnoreTimeScale(true).setEaseOutBack().setDelay(1f);
                LeanTween.scale(socialsButton, Vector3.one, 1f).setIgnoreTimeScale(true).setEaseOutBack().setDelay(1.5f);
            });
        });
    }

    public void MainMenu()
    {
        // Load Main Menu
        title.SetActive(false);
        title.transform.position = new Vector3(0, 200, 0);
        infoText.SetActive(false);
        mainMenuButton.SetActive(false);
        surveyButton.SetActive(false);
        socialsButton.SetActive(false);

        SceneManager.LoadScene(0);
    }

    public void Survey()
    {

    }

    public void socials()
    {
        Application.OpenURL("https://twitter.com/playBoneVault");
    }
}
