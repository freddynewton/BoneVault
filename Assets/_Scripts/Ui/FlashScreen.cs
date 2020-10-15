using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour {
    [HideInInspector] public bool hit;
    private Image flashImage;


    private void Awake () {
        flashImage = gameObject.GetComponent<Image>();
    }

    void Update () {
        if (hit) {
            flashImage.color = new Color(1f, 0f, 0f, 0.5f);
            hit = false;
        }
        else {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, 3f * Time.deltaTime);
        }
    }
}
