using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] AudioSource soundFXObject;

    public static SoundFXManager instance;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, Camera.main.transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClipXTimes(AudioClip audioClip, float volume, int times)
    {
        StartCoroutine(PlayAudio(audioClip, volume, times));
    }

    IEnumerator PlayAudio(AudioClip audioClip, float volume, int times)
    {
        for (int i = 0; i < times; i++)
        {
            PlaySoundFXClip(audioClip, volume);
            yield return new WaitForSeconds(audioClip.length);
        }
    }
}
