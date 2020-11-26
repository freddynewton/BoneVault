using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [Header("BackGround Settings")]
    public Color color;

    public Camera camera;

    public float lightWaitTime;

    public List<Light> lights = new List<Light>();
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        Application.OpenURL("https://twitter.com/playBoneVault");
    }

    public void Play()
    {
        // Load Loading Scene
        LeanTween.cancelAll();
        SceneManager.LoadScene(1);
    }

    private IEnumerator lightDelay(Color color, int idx)
    {
        yield return new WaitForSecondsRealtime(lightWaitTime);

        Light l = lights[idx];
        SpriteRenderer s = sprites[idx];

        s.color = color;
        l.color = color;
        l.gameObject.SetActive(true);
        l.enabled = true;
        s.enabled = true;

        if (idx < lights.Count - 1) StartCoroutine(lightDelay(color, idx += 1));
    }

    private void Awake()
    {
        LeanTween.rotateY(camera.gameObject, 10, 15f).setLoopPingPong().setEaseInOutSine();

        foreach (Light l in lights) l.enabled = false;
        foreach (SpriteRenderer s in sprites) s.enabled = false;
        StartCoroutine(lightDelay(color, 0));
    }
}