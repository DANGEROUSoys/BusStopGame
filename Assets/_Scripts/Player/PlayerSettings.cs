using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [field: SerializeField] public CameraMovementSettings CameraMovementSettings;
}
