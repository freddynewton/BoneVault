using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour
{
    [Header("Header")]
    public TextMeshProUGUI HeaderText;

    public TabGroup[] Tabs;

    public void closeAllTabs()
    {
        foreach (TabGroup t in Tabs) t.gameObject.SetActive(false);
    }
    
}
