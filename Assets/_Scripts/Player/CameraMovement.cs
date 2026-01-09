using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CameraMovement
{
    private CameraMovementSettings _settings;
    private CancellationTokenSource _movementTokenSource;
    private PlayerInput _input;
    private EventBus _eventBus;
    private Transform _playerTransform;
    private float _currentYRotation;

    public CameraMovement(PlayerInput input, EventBus eventBus, Transform transform, CameraMovementSettings settings)
    {
        _settings = settings;
        _input = input;
        _eventBus = eventBus;
        _playerTransform = transform;
        _movementTokenSource?.Dispose();
        _movementTokenSource = null;
        _eventBus.Subscribe<BackpackWasEnabled>(StopMovement);
        _eventBus.Subscribe<BackpackWasDisabled>(StartMovement);

        _playerTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void Dispose()
    {
        _movementTokenSource?.Cancel();
        _movementTokenSource?.Dispose();
        _eventBus.UnSubscribe<BackpackWasEnabled>(StopMovement);
        _eventBus.UnSubscribe<BackpackWasDisabled>(StartMovement);
    }

    public async void StartMovement()
    {
        if (_movementTokenSource != null) return;

        _movementTokenSource = new CancellationTokenSource();
        await MovementUpdate(_movementTokenSource.Token);
    }

    public void StopMovement()
    {
        _movementTokenSource.Cancel();
        _movementTokenSource.Dispose();
        _movementTokenSource = null;
    }
    
    private async Task MovementUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            float xInput = _input.Game.XInput.ReadValue<float>();
            float xPosition = _input.Game.MousePosition.ReadValue<Vector2>().x;
            float screenWidth = Screen.width;

            if(xPosition <= _settings.EdgeThreshold)
            {
                xInput = -1;
            }
            else if(xPosition >= screenWidth - _settings.EdgeThreshold)
            {
                xInput = 1;
            }

            RotateCamera(xInput);
            await Task.Yield();
        }
    }
    
    private void RotateCamera(float xInput)
    {
        float rotationChange = xInput * _settings.Speed * Time.deltaTime;
        _currentYRotation = Mathf.Clamp(_currentYRotation + rotationChange, _settings.LeftLimitAngle, _settings.RightLimitAngle);
        _playerTransform.localRotation = Quaternion.Euler(0f, _currentYRotation, 0f);
    }
}
