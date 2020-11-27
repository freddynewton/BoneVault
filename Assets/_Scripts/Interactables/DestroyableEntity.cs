using UnityEngine;
using System.Collections;


public class DestroyableEntity : Interactable
{
    private ParticleSystem vfx;

    public override void interact()
    {
        vfx.Play();
        LeanTween.scale(GFX, Vector3.zero, 0.3f).setEaseInBack().setOnComplete(() => { GFX.SetActive(false); });
    }


    private void Start()
    {
        vfx = gameObject.GetComponentInChildren<ParticleSystem>();
    }
}
