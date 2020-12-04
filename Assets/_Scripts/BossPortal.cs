using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossPortal : MonoBehaviour
{

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("EndLevelCanvas") != null)
        {
            UiManager.Instance.EndOfLevelCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            UiManager.Instance.EndOfLevelCanvas.gameObject.SetActive(true);
            UiManager.Instance.EndOfLevelCanvas.showScreen();
        }
    }
}