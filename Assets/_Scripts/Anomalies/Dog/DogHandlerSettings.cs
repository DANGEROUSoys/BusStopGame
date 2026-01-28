using UnityEngine;

[CreateAssetMenu(fileName = "DogHandlerSettings", menuName = "Settings/Dog/Dog Handler Settings")]
public class DogHandlerSettings : ScriptableObject
{
    public int MinTimeToFirstAppearingInSeconds;
    public int MaxTimeToFirstAppearingInSeconds;
    public int MinTimeToAppearingInSeconds;
    public int MaxTimeToAppearingInSeconds;
}
