using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    private Image flashImage;

    private void Awake()
    {
        flashImage = gameObject.GetComponent<Image>();
    }

    public void flashScreen(float time, Color color)
    {
        flashImage.color = color;

        LeanTween.value(flashImage.color.a, 0, time).setEaseOutQuint().setOnUpdate((float val) =>
        {
            flashImage.color = new Color(color.r, color.g, color.b, val);
        });
    }
}