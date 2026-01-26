using UnityEngine;

public struct PlaySoundEvent : IEvent
{
    public AudioSource AudioSource;
    public AudioClip Clip;
    
    public PlaySoundEvent(AudioSource audioSource, AudioClip clip)
    {
        AudioSource = audioSource;
        Clip = clip;
    }
}
