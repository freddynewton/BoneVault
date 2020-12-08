using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // addSounds();
    }

    public void addSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
     
            s.source.loop = s.loop;
        }
    }

    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) return;

        s.source.Play();
    }

    public void playRandomSFX(AudioClip[] sounds, AudioSource source, float minPitch, float maxPitch)
    {
        if (source == null) return;

        source.clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        source.Play();
    }

    public void playSFX(AudioClip sound, AudioSource source, float minPitch, float maxPitch)
    {
        if (source == null) return;

        source.clip = sound;
        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        source.Play();
    }
}
