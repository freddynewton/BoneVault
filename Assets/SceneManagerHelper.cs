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

        if (GameManagerHelper.Instance != null)
        {
            GameManagerHelper.Instance.resetAll();
        }

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.playerCameraHandler.GetComponent<Camera>().gameObject.SetActive(false);
        }

        SceneManager.LoadScene(0);
    }

    public void loadLoadingScene()
    {
        Time.timeScale = 1;

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
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.playerCameraHandler.GetComponent<Camera>().gameObject.SetActive(true);
            }
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
