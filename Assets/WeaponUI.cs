using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public SwordUI swordUI;

    public void activateSwordUI()
    {
        swordUI.gameObject.SetActive(true);
    }
}
