using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public SwordUI swordUI;

    public void activateSwordUI(bool value)
    {
        swordUI.gameObject.SetActive(value);
    }
}
