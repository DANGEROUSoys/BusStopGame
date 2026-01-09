using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public CameraMovementSettings CameraMovementSettings;
}
