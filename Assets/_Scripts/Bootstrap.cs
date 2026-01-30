using System.Collections.Generic;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ProjectContext _projectContext; 
    [SerializeField] private NightHandler _nightHandler;
    [SerializeField] private Player _player;
    [SerializeField] private Smartphone _smartphone;
    [SerializeField] private ShadowHandler _shadowHandler;
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Inventory.Inventory _inventory;
    [SerializeField] private DogHandler _dogHandler;
    [SerializeField] private LightHandler _lightHandler;
    private ScreamerService _screamerService;
    private SoundService _soundService;
    private List<IDisposable> _disposables;

    private void Awake()
    {
        _disposables = new List<IDisposable>();
        _projectContext.Initialize();
        InitializeServices();
        InitializeNight();
        InitializePlayer();
        InitializeBackpack();
        InitializeAnomalies();
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _projectContext.Dispose(); // Убрать
        _disposables.Clear();
    }

    private void InitializeNight()
    {
        _nightHandler.Initialize();
        _disposables.Add(_nightHandler);
        _nightHandler.StartHandlerUpdate();
    }

    private void InitializeServices()
    {
        _screamerService = new ScreamerService();
        _disposables.Add(_screamerService);

        _lightHandler.Initialize();
        _disposables.Add(_lightHandler);

        _soundService = new SoundService();
        _disposables.Add(_screamerService);
    }

    private void InitializePlayer()
    {
        _player.Initialize();
        _player.EnableCameraMovement();
        _player.EnableRaycast();
        _disposables.Add(_player);

        _inventory.Initialize();
        _disposables.Add(_inventory);

        _smartphone.Initialize();
        _smartphone.StartSmartphoneUpdate();
        _disposables.Add(_smartphone);
    }

    private void InitializeBackpack()
    {
        _backpack.Initialize();
        _disposables.Add(_backpack);
    }

    private void InitializeAnomalies()
    {
        _shadowHandler.Initialize();
        _shadowHandler.StartShadowHandlerUpdate();
        _disposables.Add(_shadowHandler);

        _dogHandler.Initialize();
        _dogHandler.StartDogHandlerUpdate();
        _disposables.Add(_dogHandler);
    }
}
