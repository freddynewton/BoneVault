using UnityEngine;
using System.Collections;


public class DestroyableEntity : Interactable
{
    public AudioClip [] destroySFX;
    private ParticleSystem vfx;

    public override void interact()
    {
        vfx.Play();
        GFX.SetActive(false);
        playSFX(destroySFX, GetComponent<AudioSource>(), true);
    }


    private void Start()
    {
        vfx = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // SFX Handler
    public void playSFX (AudioClip [] sounds, AudioSource source, bool random) {
        if (random) source.clip = sounds [Random.Range(0, sounds.Length)];
        else source.clip = sounds [0];

        source.pitch = Random.Range(0.8f, 1.2f);

        if (source != null) source.Play();
    }
}
