using UnityEngine;

[CreateAssetMenu(fileName = "DogSettings", menuName = "Settings/Dog/Dog Settings")]
public class DogSettings : ScriptableObject
{
    public float StageTime;
    public AudioClip BarkingAudio;
    public AudioClip GrowlingAudio;
}
