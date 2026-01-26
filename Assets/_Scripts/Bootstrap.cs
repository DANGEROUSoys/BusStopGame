using System.Collections.Generic;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ShadowHandler _shadowHandler;
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Inventory.Inventory _inventory;
    [SerializeField] private DogHandler _dogHandler;
    [SerializeField] private LightHandler _lightHandler;
    private PlayerInput _input;
    private EventBus _eventBus;
    private ScreamerService _screamerService;
    private SoundService _soundService;
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
        
        _screamerService = new ScreamerService(_eventBus);
        _disposables.Add(_screamerService);

        _lightHandler.Initialize(_eventBus);
        _disposables.Add(_lightHandler);

        _soundService = new SoundService(_eventBus);
        _disposables.Add(_screamerService);
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
        _shadowHandler.Initialize(_eventBus);
        //_shadowAnomaly.StartScreamerTimer();
        _disposables.Add(_shadowHandler);

        _dogHandler.Initialize(_eventBus);
        _dogHandler.StartDogHandlerUpdate();
        _disposables.Add(_dogHandler);
    }
}
