using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Singleton;
using UnityEngine;

public class SoundManager : AbstractSingleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource loopAudioSource;
    
    [SerializeField] private float lowPitchRange = .95f;
    [SerializeField] private float highPitchRange = 1.05f;
    [SerializeField] private AudioClip loopSFX;
    
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
        loopAudioSource.Play();
    }

    public void StopLoop()
    {
        loopAudioSource.Stop();
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
