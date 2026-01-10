using System.Collections.Generic;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ShadowAnomaly _shadowAnomaly;
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Inventory.Inventory _inventory;
    [SerializeField] private DogHandler _dogHandler;
    private PlayerInput _input;
    private EventBus _eventBus;
    private List<IDisposable> _disposables;

    private void Awake()
    {
        _disposables = new List<IDisposable>();
        InitializeServices();
        InitializePlayer();

        _backpack.Initialize(_input, _eventBus);
        _disposables.Add(_backpack);

        InitializeAnomalies();
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
        _eventBus.Dispose();
    }

    private void InitializeServices()
    {
        _input = new PlayerInput();
        _input.Enable();
        _disposables.Add(_input);

        _eventBus = new EventBus();
        _eventBus.Initialize();
    }

    private void InitializePlayer()
    {
        _player.Initialize(_input, _eventBus);
        _player.EnableCameraMovement();
        _player.EnableRaycast();
        _disposables.Add(_player);

        _inventory.Initialize(_eventBus);
        _disposables.Add(_inventory);
    }

    private void InitializeAnomalies()
    {
        _shadowAnomaly.Initialize();
        _shadowAnomaly.StartScreamerTimer();
        _disposables.Add(_shadowAnomaly);

        _dogHandler.Initialize(_eventBus);
        _dogHandler.StartDogHandlerUpdate();
        _disposables.Add(_dogHandler);
    }
}
