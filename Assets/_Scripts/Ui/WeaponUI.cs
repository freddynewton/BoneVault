using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public SwordUI swordUI;

    public void activateSwordUI(bool value)
    {
        swordUI.gameObject.SetActive(value);
    }
}