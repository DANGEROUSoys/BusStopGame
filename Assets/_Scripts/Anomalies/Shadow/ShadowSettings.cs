using UnityEngine;

[CreateAssetMenu(fileName = "ShadowSettings", menuName = "Settings/Shadow Settings")]
public class ShadowSettings : ScriptableObject
{
    public int MinTimeToAppearingInSeconds;
    public int MaxTimeToAppearingInSeconds;
}