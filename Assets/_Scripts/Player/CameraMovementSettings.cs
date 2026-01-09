using UnityEngine;

[CreateAssetMenu(fileName = "CameraMovementSettings", menuName = "Game/Camera Movement Settings")]
public class CameraMovementSettings : ScriptableObject
{
    [Header("Rotation Limits")]
    [SerializeField] private float _rightLimitAngle = 30f;
    [SerializeField] private float _leftLimitAngle = -30f;
    
    [Header("Mouse Edge Detection")]
    [SerializeField] private float _edgeThreshold = 50f;
    
    [Header("Movement")]
    [SerializeField] private float _speed = 100f;

    public float RightLimitAngle => _rightLimitAngle;
    public float LeftLimitAngle => _leftLimitAngle;
    public float EdgeThreshold => _edgeThreshold;
    public float Speed => _speed;
}