using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private PlayerRaycast _playerRaycast;
    [SerializeField] private ShadowAnomaly _shadowAnomaly;
    [SerializeField] private Backpack _backpack;
    private PlayerInput _input;
    private EventBus _eventBus;

    private void Awake()
    {
        InitializeServices();

        _cameraMovement.Initialize(_input, _eventBus);
        _cameraMovement.StartMovement();

        _playerRaycast.Initialize(_input, _eventBus);
        _playerRaycast.StartRaycast();

        _shadowAnomaly.Initialize();
        _shadowAnomaly.StartScreamerTimer();

        _backpack.Initialize(_input, _eventBus);
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void InitializeServices()
    {
        _input = new PlayerInput();
        _input.Enable();

        _eventBus = new EventBus();
        _eventBus.Initialize();
    }
}
