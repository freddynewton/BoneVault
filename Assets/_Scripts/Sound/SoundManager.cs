using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public Sound[] music;

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
        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
    }

    public void playMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null) return;

        foreach (Sound _s in music)
        {
            if (_s.source.isPlaying) _s.source.Stop();
        }

        s.source.Play();
    }

    public void playRandomSFX(AudioClip[] sounds, AudioSource source, float minPitch, float maxPitch)
    {
        if (source == null || sounds.Length == 0) return;

        source.clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        source.Play();
    }

    public void playSFX(AudioClip sound, AudioSource source, float minPitch, float maxPitch)
    {
        if (source == null || sound == null) return;

        source.clip = sound;
        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        source.Play();
    }
}
