using UnityEngine;

[CreateAssetMenu(fileName = "DogSettings", menuName = "Settings/Dog Settings")]
public class DogSettings : ScriptableObject
{
    public int MinTimeToFirstAppearingInSeconds;
    public int MaxTimeToFirstAppearingInSeconds;
    public int MinTimeToAppearingInSeconds;
    public int MaxTimeToAppearingInSeconds;
}
