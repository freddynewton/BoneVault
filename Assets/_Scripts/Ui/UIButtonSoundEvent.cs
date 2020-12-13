using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSoundEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {
    public AudioClip hoverSFX;
    public AudioClip clickSFX;


    public void OnPointerEnter (PointerEventData ped) {
        SoundManager.Instance.playSFX(hoverSFX, GetComponent<AudioSource>(), 1, 1);
    }

    public void OnPointerDown (PointerEventData ped) {
        SoundManager.Instance.playSFX(clickSFX, GetComponent<AudioSource>(), 1, 1);
    }
}
