using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _rightLimitAngle;
    [SerializeField] private float _leftLimitAngle;
    [SerializeField] private float _edgeThreshold = 50f;
    [SerializeField] private float _speed;
    private Coroutine _movementUpdate;
    private PlayerInput _input;
    private EventBus _eventBus;
    private float _currentYRotation;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;

        _eventBus.Subscribe<BackpackWasEnabled>(StopMovement);
        _eventBus.Subscribe<BackpackWasDisabled>(StartMovement);
    }

    private void OnDisable()
    {
        _eventBus.UnSubscribe<BackpackWasEnabled>(StopMovement);
        _eventBus.UnSubscribe<BackpackWasDisabled>(StartMovement);
    }

    public void StartMovement()
    {
        _movementUpdate = StartCoroutine(MovementUpdate());
    }

    public void StopMovement()
    {
        StopCoroutine(_movementUpdate);
    }
    
    private IEnumerator MovementUpdate()
    {
        while (true)
        {
            float xInput = _input.Game.XInput.ReadValue<float>();
            float xPosition = _input.Game.MousePosition.ReadValue<Vector2>().x;
            float screenWidth = Screen.width;

            if(xPosition <= _edgeThreshold)
            {
                xInput = -1;
            }
            else if(xPosition >= screenWidth - _edgeThreshold)
            {
                xInput = 1;
            }

            RotateCamera(xInput);
            yield return null;
        }
    }
    
    private void RotateCamera(float xInput)
    {
        float rotationChange = xInput * _speed * Time.deltaTime;
        _currentYRotation = Mathf.Clamp(_currentYRotation + rotationChange, _leftLimitAngle, _rightLimitAngle);
        transform.localRotation = Quaternion.Euler(0f, _currentYRotation, 0f);
    }
}
