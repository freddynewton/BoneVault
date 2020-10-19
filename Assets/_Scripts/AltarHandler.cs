using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltarHandler : MonoBehaviour
{
    [HideInInspector] public List<Outline> outlines;

    [Header("Altar Settings")]
    public float distance = 5;
    public GameObject hand;

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

    private void Awake()
    {
        LeanTween.moveY(hand, 2.2f, 1f).setEaseInOutQuad().setDelay(UnityEngine.Random.Range(0, 0.3f)).setLoopPingPong();
    }

    // Start is called before the first frame update
    void Start()
    {
        outlines = gameObject.GetComponentsInChildren<Outline>().ToList();
    }
}
