using UnityEngine;

public struct PlayAudioEvent : IEvent
{
    public AudioSource AudioSource;
    public AudioClip Clip;
    
    public PlayAudioEvent(AudioSource audioSource, AudioClip clip)
    {
        AudioSource = audioSource;
        Clip = clip;
    }
}
