using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_SettingsPanel : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider soundSlider, musicSlider;

    public void setVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("masterVolume", soundSlider.value);
        }
    }

    public void setMusicVolume()
    {
        if(audioMixer != null)
        {
            audioMixer.SetFloat("musicVolume", musicSlider.value);
        }
    }

    public void changeQualty(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
