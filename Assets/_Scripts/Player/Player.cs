using UnityEngine;

public class Player : MonoBehaviour, IDisposable, IPauseHandler
{
    [SerializeField] private PlayerSettings _playerSettings;
    private Camera _camera;
    private EventBus _eventBus;
    private PlayerInput _input;

    private CameraMovement _cameraMovement;
    private PlayerRaycast _playerRaycast;

    public void Initialize()
    {
        _input = ProjectContext.Instance.PlayerInput;
        _eventBus = ProjectContext.Instance.EventBus;
        _camera = Camera.main;

        _cameraMovement = new CameraMovement(_input, _eventBus, transform, _playerSettings.CameraMovementSettings);
        _playerRaycast = new PlayerRaycast(_input, _eventBus, _camera);
        _eventBus.Subscribe<GamePauseEvent>(SetPaused);
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<GamePauseEvent>(SetPaused);
        _cameraMovement.Dispose();
        _playerRaycast.Dispose();
        _input = null;
        _eventBus = null;
        _camera = null;
    }

    public void EnableCameraMovement() => _cameraMovement.StartMovement();
    public void DisableCameraMovement() => _cameraMovement.StopMovement();
    public void EnableRaycast() => _playerRaycast.StartRaycast();
    public void DisableRaycast() => _playerRaycast.StopRaycast();

    public void SetPaused(GamePauseEvent gamePause)
    {
        if (gamePause.IsPaused == true)
        {
            DisableCameraMovement();
            DisableRaycast();
        }
        else
        {
            EnableCameraMovement();
            EnableRaycast();
        }
    }
}
