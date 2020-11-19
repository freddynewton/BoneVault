using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordUI : MonoBehaviour
{
    public List<GameObject> uiObjects = new List<GameObject>();

    private const string isNotChargedImg = "UI/charge_empty";
    private const string isChargedImg = "UI/charge_full";

    public void spawnUI(int value)
    {
        foreach (GameObject obj in uiObjects)
        {
            if (uiObjects.IndexOf(obj) < value) obj.SetActive(true);
            else obj.SetActive(false);
        }
    }

    public void setCharge(int value)
    {
        foreach (GameObject obj in uiObjects)
        {
            if (uiObjects.IndexOf(obj) < value)
            {
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(isChargedImg);
                LeanTween.scale(obj, Vector3.one * 1.6f, 0.5f).setEaseOutBounce().setOnComplete(() =>
                {
                    LeanTween.scale(obj, Vector3.one, 0.3f).setEaseOutCirc();
                });
            }
            else
            {
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(isNotChargedImg);
            }
        }
    }
}