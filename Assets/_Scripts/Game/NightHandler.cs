using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NightHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Player _player;
    [SerializeField] private Smartphone _smartphone;
    [SerializeField] private ShadowHandler _shadowHandler;
    [SerializeField] private Backpack _backpack;
    [SerializeField] private Inventory.Inventory _inventory;
    [SerializeField] private DogHandler _dogHandler;
    private NightData _data;
    private PauseService _pauseService;
    private ScreamerService _screamerService; 
    private LightHandler _lightHandler; 

    private PlayerInput _input;
    private CancellationTokenSource _updateTokenSource;
    private List<IDisposable> _disposables;

    public void Initialize()
    {
        _disposables = new List<IDisposable>();
        _input = ProjectContext.Instance.PlayerInput;
        _data = new NightData();
        InitializeServices();
        InitializePlayer();
        InitializeBackpack();
        InitializeAnomalies();
        _data.MenuIsActive = false;

        StartNight();
    }
    private void StartNight()
    {
        StartHandlerUpdate();
        _shadowHandler.StartShadowHandlerUpdate();
        _player.EnableCameraMovement();
        _player.EnableRaycast();
        _smartphone.StartSmartphoneUpdate();
        _dogHandler.StartDogHandlerUpdate();
    }

    private void OnDisable()
    {
        StopHandlerUpdate();
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
        _data = null;
        _input = null;
        _pauseService = null;
    }

    private async void StartHandlerUpdate()
    {
        if (_updateTokenSource != null) return;
        
        _updateTokenSource = new CancellationTokenSource();
        await HandlerUpdate(_updateTokenSource.Token);
    }

    private void StopHandlerUpdate()
    {
        _updateTokenSource?.Cancel();
        _updateTokenSource?.Dispose();
        _updateTokenSource = null;
    }

    private async Task HandlerUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_input.Game.Quit.WasPressedThisFrame())
            {
                _data.MenuIsActive = !_data.MenuIsActive;
                //_pauseService.SetPaused(_data.MenuIsActive);
                _menu.SetActive(_data.MenuIsActive);
            }
            await Task.Yield();
        }
    }

    private void InitializeServices()
    {
        _pauseService = new PauseService();
        _disposables.Add(_pauseService);

        _screamerService = new ScreamerService();
        _disposables.Add(_screamerService);

        _lightHandler = gameObject.GetComponent<LightHandler>();
        _lightHandler.Initialize();
        _disposables.Add(_lightHandler);
    }

    private void InitializePlayer()
    {
        _player.Initialize();
        _disposables.Add(_player);

        _inventory.Initialize();
        _disposables.Add(_inventory);

        _smartphone.Initialize();
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
        _disposables.Add(_shadowHandler);

        _dogHandler.Initialize();
        _disposables.Add(_dogHandler);
    }
}
