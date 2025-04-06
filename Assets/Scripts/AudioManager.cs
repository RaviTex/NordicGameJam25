using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioClip> logCrashSounds;
    public float logSinkSoundDelay = 0.5f;
    public List<AudioClip> paddleSounds;
    
    
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioClip beaverScreaming;
    [SerializeField] private AudioClip pickUpSound;
    [SerializeField] private AudioClip dropOffSound;
    
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EngineIdle()
    {
        engineSound.clip = paddleSounds[2];
        engineSound.Play();
    }
    public void EngineSlow()
    {
        engineSound.clip = paddleSounds[1]; 
        engineSound.Play();
    }
    public void EngineFast()
    {
        engineSound.clip = paddleSounds[0];
        engineSound.Play();
    }
    public void StopEngineSound()
    {
        engineSound.Stop();
    }
    public void PlayPickUpSound()
    {
        sfxAudioSource.PlayOneShot(pickUpSound);
    }
    public void PlayDropOffSound()
    {
        sfxAudioSource.PlayOneShot(dropOffSound);
    }
    
    public void PlayBeaverScream()
    {
        sfxAudioSource.PlayOneShot(beaverScreaming);
    }
    
    public void PlayLogCrashSound()
    {
        if (logCrashSounds.Count > 0)
        {
            StartCoroutine(PlayLogCrashSoundIE());
        }
    }

    private IEnumerator PlayLogCrashSoundIE()
    {
        sfxAudioSource.PlayOneShot(logCrashSounds[0]);
        yield return new WaitForSeconds(logSinkSoundDelay);
        sfxAudioSource.PlayOneShot(logCrashSounds[1]);
    }
}
