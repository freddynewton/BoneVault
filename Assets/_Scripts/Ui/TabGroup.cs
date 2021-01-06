using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class TabGroup : MonoBehaviour
{
    [Header("Settings")]
    public string m_name;

    public abstract void interact();

}
