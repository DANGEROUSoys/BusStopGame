using UnityEngine;

public class Player : MonoBehaviour, IDisposable
{
    [SerializeField] private PlayerSettings _playerSettings;
    private Camera _camera;
    private EventBus _eventBus;
    private PlayerInput _input;

    private CameraMovement _cameraMovement;
    private PlayerRaycast _playerRaycast;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
        _camera = Camera.main;

        _cameraMovement = new CameraMovement(_input, _eventBus, transform, _playerSettings.CameraMovementSettings);
        _playerRaycast = new PlayerRaycast(_input, _eventBus, _camera);
    }
    public void EnableCameraMovement() => _cameraMovement.StartMovement();
    public void DisableCameraMovement() => _cameraMovement.StopMovement();
    public void EnableRaycast() => _playerRaycast.StartRaycast();
    public void DisableRaycast() => _playerRaycast.StopRaycast();

    public void Dispose()
    {
        _cameraMovement.Dispose();
        _playerRaycast.Dispose();
        _input = null;
        _eventBus = null;
        _camera = null;
    }
}
