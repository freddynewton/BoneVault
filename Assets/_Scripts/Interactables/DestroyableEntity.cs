using UnityEngine;
using System.Collections;


public class DestroyableEntity : Interactable
{
    private ParticleSystem vfx;

    public override void interact()
    {
        vfx.Play();
        GFX.SetActive(false);
    }


    private void Start()
    {
        vfx = gameObject.GetComponentInChildren<ParticleSystem>();
    }
}
