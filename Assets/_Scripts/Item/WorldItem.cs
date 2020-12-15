using UnityEngine;
using System.Collections;


public class WorldItem : Item
{
    [Header("World Item")]
    public GameObject GFX;
    public AudioClip[] useSFX;
    public bool isDestroyable;

    private ParticleSystem vfx;
    private AudioSource audioSource;
    private bool isUsed = false;

    public override void interact()
    {
        if (!isUsed)
        {
            if (vfx != null) vfx.Play();
            if (GFX != null) GFX.SetActive(false);

            SoundManager.Instance.playRandomSFX(useSFX, audioSource, 0.8f, 1.2f);
            isUsed = true;
        }
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        vfx = gameObject.GetComponentInChildren<ParticleSystem>();
    }
}
