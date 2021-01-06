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

    public void setHeaderText()
    {
        foreach (TabGroup t in Tabs)
        {
            if (t.gameObject.activeSelf) HeaderText.text = t.m_name;
        }
    }

    public void closeAllTabs()
    {
        foreach (TabGroup t in Tabs) t.gameObject.SetActive(false);
    }
    
}
