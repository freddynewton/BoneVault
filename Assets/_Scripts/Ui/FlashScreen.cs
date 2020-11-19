using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    private Image flashImage;

    private void Awake()
    {
        flashImage = gameObject.GetComponent<Image>();
    }

    public void flashScreen(float time)
    {
        flashImage.color = new Color(1f, 0f, 0f, 0.5f);

        LeanTween.value(flashImage.color.a, 0, time).setEaseOutQuint().setOnUpdate((float val) =>
        {
            flashImage.color = new Color(1f, 0f, 0f, val);
        });
    }
}