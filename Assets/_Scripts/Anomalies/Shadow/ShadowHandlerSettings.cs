using UnityEngine;

[CreateAssetMenu(fileName = "ShadowHandlerSettings", menuName = "Settings/Shadow/Shadow Handler Settings")]
public class ShadowHandlerSettings : ScriptableObject
{
    public int MinTimeToAppearingInSeconds;
    public int MaxTimeToAppearingInSeconds;
}
