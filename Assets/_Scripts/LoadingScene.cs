using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    public Slider loadSlider;
    public int sceneIndex;
    public TextMeshProUGUI text;

    private void Awake()
    {
        StartCoroutine(loadAsync(sceneIndex));
    }

    private IEnumerator loadAsync(int sceneIndex)
    {
        // Load Game Scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadSlider.value = progress;
            text.text = progress * 100 + "%";

            yield return null;
        }
    }
}