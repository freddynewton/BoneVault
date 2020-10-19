using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarHandler : MonoBehaviour
{
    [HideInInspector] public Outline outline;

    [Header("Altar Settings")]
    public float distance = 5;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
    }
}
