using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private PlayerRaycast _playerRaycast;
    [SerializeField] private ShadowAnomaly _shadowAnomaly;
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Inventory.Inventory _inventory;
    [SerializeField] private Dog _dog;
    private PlayerInput _input;
    private EventBus _eventBus;

    private void Awake()
    {
        InitializeServices();

        _cameraMovement.Initialize(_input, _eventBus);
        _cameraMovement.StartMovement();

        _playerRaycast.Initialize(_input, _eventBus);
        _playerRaycast.StartRaycast();

        _backpack.Initialize(_input, _eventBus);

        _inventory.Initialize(_eventBus);

        InitializeAnomalies();
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

    private void InitializeAnomalies()
    {
        _shadowAnomaly.Initialize();
        _shadowAnomaly.StartScreamerTimer();

        _dog.Initialize(_eventBus, _inventory);
        _dog.Appear();
    }
}
