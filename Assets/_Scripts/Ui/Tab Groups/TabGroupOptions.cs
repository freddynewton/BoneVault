using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TabGroupOptions : TabGroup
{
    public Slider MouseSensitivitySlider;
    public Slider SFXVolumeSlider;
    public Slider MusicVolumeSlider;

    public AudioMixer AudioMixer;

    private void Awake()
    {
        // TODO Add Options Save

        MouseSensitivitySlider.maxValue = 1500;
        MouseSensitivitySlider.value = 500;
    }

    public override void interact()
    {
        
    }

    // Set Mouse Sensitivity Slider Function
    public void setMouseSensitivity(float val) => PlayerController.Instance.unit.playerStats.mouseSensitivity = val;

    #region Music Volume Slider
    public void setSFXVol(float vol) => AudioMixer.SetFloat("sfxVolume", vol);
    public void setMasterVol(float vol) => AudioMixer.SetFloat("masterVolume", vol);
    public void setAmbienceVol(float vol) => AudioMixer.SetFloat("ambienceVolume", vol);
    public void setMusicVol(float vol) => AudioMixer.SetFloat("musicVolume", vol);
    #endregion
}
