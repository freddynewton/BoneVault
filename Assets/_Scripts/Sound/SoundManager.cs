using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip [] musicClip;
    public AudioSource musicSource;

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

    public static IEnumerator fadeMusic (int clipNumber, float fadeTime, bool fadeIn) {
        AudioSource audioSource = SoundManager.Instance.musicSource;
        float startVolume = audioSource.volume;

        audioSource.clip = SoundManager.Instance.musicClip [clipNumber];

        if (!fadeIn) {
            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }           
        }
        else {
            audioSource.volume = 0;
            audioSource.Play();

            while (audioSource.volume < startVolume) {
                audioSource.volume += startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }
        }
    }
}
