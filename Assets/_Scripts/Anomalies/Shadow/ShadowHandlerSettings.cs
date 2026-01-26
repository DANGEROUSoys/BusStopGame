using UnityEngine;

[CreateAssetMenu(fileName = "ShadowHandlerSettings", menuName = "Settings/Shadow Handler Settings")]
public class ShadowHandlerSettings : ScriptableObject
{
    public int MinTimeToAppearingInSeconds;
    public int MaxTimeToAppearingInSeconds;
}
