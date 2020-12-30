using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerHelper : MonoBehaviour
{
    public static SceneManagerHelper Instance { get; private set; }

    public void loadMainMenu()
    {
        LeanTween.cancelAll();
        Time.timeScale = 1;

        StateMachineController.Instance.enemyUnits.Clear();

        GameManagerHelper.Instance.resetAll();
        PlayerController.Instance.playerCameraHandler.gameObject.SetActive(false);

        UiManager.Instance.setActiveMainMenuCanvas(true);
        UiManager.Instance.setActiveDeathCanvas(false);
        UiManager.Instance.setActiveHUD(false);
        UiManager.Instance.setActiveMiniMap(false);

        SceneManager.LoadScene(0);
    }

    public void loadLoadingScene()
    {
        Time.timeScale = 1;

        UiManager.Instance.setActiveMainMenuCanvas(false);

        // Load Loading Scene
        LeanTween.cancelAll();
        SceneManager.LoadScene(1);
    }

    public AsyncOperation loadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        StartCoroutine(loadGameSceneEnum(operation));
        return operation;
    }

    private IEnumerator loadGameSceneEnum(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            yield return null;
        }

        if (operation.isDone)
        {
            UiManager.Instance.setActivePreparingLevel(true);
            UiManager.Instance.setActiveMainMenuCanvas(false);
            UiManager.Instance.setActiveHUD(true);
            UiManager.Instance.setActiveMiniMap(true);

            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.playerCameraHandler.gameObject.SetActive(true);
            }

            WorldGeneratorManager.Instance.startGeneratingMap();
        }
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
    }
}
