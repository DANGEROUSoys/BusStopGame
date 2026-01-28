using UnityEngine;

[CreateAssetMenu(fileName = "CameraMovementSettings", menuName = "Settings/Player/Camera Movement Settings")]
public class CameraMovementSettings : ScriptableObject
{
    [Header("Rotation Limits")]
    [field: SerializeField] public float RightLimitAngle = 30f;
    [field: SerializeField] public float LeftLimitAngle = -30f;
    
    [Header("Mouse Edge Detection")]
    [field: SerializeField] public float EdgeThreshold = 50f;
    
    [Header("Movement")]
    [field: SerializeField] public float Speed = 100f;
}