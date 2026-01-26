using UnityEngine;

public class DogData
{
    public readonly DogView DogView;
    public readonly DogSettings DogSettings;
    public readonly Collider Collider;
    public readonly AudioSource AudioSource;
    public readonly AudioClip BarkingAudio;
    public readonly AudioClip GrowlingAudio;
    public readonly float StageTime;

    public DogData(DogView dogView, DogSettings dogSettings, Collider collider, AudioSource audioSource)
    {
        DogView = dogView;
        DogSettings = dogSettings;
        Collider = collider;
        AudioSource = audioSource;
        StageTime = DogSettings.StageTime;
        BarkingAudio = DogSettings.BarkingAudio;
        GrowlingAudio = DogSettings.GrowlingAudio;
    }
}
