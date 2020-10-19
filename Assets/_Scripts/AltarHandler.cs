using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltarHandler : MonoBehaviour
{
    [HideInInspector] public List<Outline> outlines;

    [Header("Altar Settings")]
    public float distance = 5;

    [HideInInspector] public SpecialRoom room;

    public void setOutline(bool active)
    {
        foreach (Outline o in outlines)
        {
            o.enabled = active;
        }
    }

    public void use()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        outlines = gameObject.GetComponentsInChildren<Outline>().ToList();
    }
}
