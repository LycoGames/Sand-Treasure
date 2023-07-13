using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Singleton;
using DG.Tweening;
using UnityEngine;

public class SoundManager : AbstractSingleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource loopAudioSource;
    [SerializeField] private AudioSource loopEngineSource;

    [SerializeField] private float lowPitchRange = .95f;
    [SerializeField] private float highPitchRange = 1.05f;
    [SerializeField] private AudioClip loopSFX;
    [SerializeField] private AudioClip engineSound;

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayLoop()
    {
        loopAudioSource.clip = loopSFX;
        if (!loopAudioSource.isPlaying)
        {
            loopAudioSource.Play();
        }
        else
        {
            loopAudioSource.UnPause();
        }
    }

    public void StopLoop()
    {
        loopAudioSource.Pause();
        // loopAudioSource.Stop();
    }

    public void PlayLoopEngine()
    {
        loopEngineSource.clip = engineSound;
        loopEngineSource.Play();
    }

    public void StopEngine()
    {
        loopEngineSource.Stop();
    }

    public void EngineSoundPitchIncrease()
    {
        loopEngineSource.DOPitch(1.8f, 1f);
    }

    public void EngineSoundPitchDecrease()
    {
        loopEngineSource.DOPitch(0.8f, 1f);
    }
    public void EngineSoundVolumeIncrease()
    {
        loopEngineSource.volume = 0.25f;
    }
    public void EngineSoundVolumeDecrease()
    {
        loopEngineSource.volume = 0.1f;
    }
    public void PlayRandomSoundEffect(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        audioSource.pitch = randomPitch;
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }
}