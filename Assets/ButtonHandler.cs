using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void MainMenuButton()
    {
        SceneManagerHelper.Instance.loadMainMenu();
    }
    public void Play()
    {
        SceneManagerHelper.Instance.loadLoadingScene();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        Application.OpenURL("https://linktr.ee/bonevault");
    }

    public void Options()
    {
        UiManager.Instance.setActiveOptionsMenu(true);
        UiManager.Instance.setActiveMainMenuCanvas(false);
    }

}
