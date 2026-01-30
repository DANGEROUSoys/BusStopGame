using UnityEngine;

public class SmartphoneData
{
    public readonly EventBus EventBus;
    public readonly SmarthoneSettings Settings;
    public readonly GameObject VisualObject;
    public readonly Animator Animator;
    public readonly AudioSource AudioSource;
    public readonly AudioClip ButtonAudio;

    public SmartphoneData(EventBus eventBus, SmarthoneSettings settings, GameObject visualObject, Animator animator, AudioSource audioSource)
    { 
        EventBus = eventBus;
        Settings = settings;
        VisualObject = visualObject;
        Animator = animator;
        AudioSource = audioSource;
        ButtonAudio = Settings.ButtonAudio;
    }
}
