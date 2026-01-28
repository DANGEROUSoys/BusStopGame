using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [field: SerializeField] public CameraMovementSettings CameraMovementSettings;
}
