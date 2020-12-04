using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    public Slider loadSlider;
    public TextMeshProUGUI text;

    private void Awake()
    {
        StartCoroutine(loadAsync());
    }

    private IEnumerator loadAsync()
    {
        // Load Game Scene
        AsyncOperation operation = SceneManagerHelper.Instance.loadGameScene();

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadSlider.value = progress;
            text.text = progress * 100 + "%";

            yield return null;
        }
    }
}