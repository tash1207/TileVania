using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", GetLogLevel(level));
        PlayerPrefs.SetFloat("masterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", GetLogLevel(level));
        PlayerPrefs.SetFloat("soundFXVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", GetLogLevel(level));
        PlayerPrefs.SetFloat("musicVolume", level);
    }

    float GetLogLevel(float level)
    {
        return Mathf.Log10(level) * 20f;
    }
}
