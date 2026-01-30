using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SmarthoneView))]
public class Smartphone : MonoBehaviour, IDisposable, IPauseHandler
{
    [SerializeField] private SmarthoneSettings _settings;
    [SerializeField] private GameObject _visualObject;
    private AudioSource _audioSource;
    private SmarthoneView _smarthoneView;
    private Animator _animator;
    private PlayerInput _input;
    private EventBus _eventBus;
    private SmartphoneData _data;
    private CancellationTokenSource _updateTokenSource;
    private bool _smartphoneIsActive;

    public void Initialize()
    {
        _input = ProjectContext.Instance.PlayerInput;
        _eventBus = ProjectContext.Instance.EventBus;
        _audioSource = GetComponent<AudioSource>();
        _smarthoneView = GetComponent<SmarthoneView>();
        _animator = GetComponent<Animator>();
        _data = new SmartphoneData(_eventBus, _settings, _visualObject, _animator, _audioSource);
        _smarthoneView.Initialize(_data);

        _eventBus.Subscribe<GamePauseEvent>(SetPaused);
    }

    public async void StartSmartphoneUpdate()
    {
        if (_updateTokenSource != null) return;

        _updateTokenSource = new CancellationTokenSource();
        await SmartphoneUpdate(_updateTokenSource.Token);
    }
    public void StopSmartphoneUpdate()
    {
        _updateTokenSource?.Cancel();
        _updateTokenSource?.Dispose();
        _updateTokenSource = null;
    }

    private async Task SmartphoneUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_input.Game.AlterInteract.WasPressedThisFrame())
            {
                if (_smartphoneIsActive == false) // Если телефон ещё НЕ включен
                {
                    _smarthoneView.PlayTurnOnAnimation();
                    _smartphoneIsActive = true;
                }
                else
                {
                    _smarthoneView.PlayTurnOffAnimation();
                    _smartphoneIsActive = false;
                }
            }
            await Task.Yield();
        }
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<GamePauseEvent>(SetPaused);
        _input = null;
        _eventBus = null;
        _smarthoneView.Dispose();
        StopSmartphoneUpdate();
    }

    public void SetPaused(GamePauseEvent gamePause)
    {
        if (gamePause.IsPaused)
        {
            StopSmartphoneUpdate();
        }
        else
        {
            StartSmartphoneUpdate();
        }
    }
}
