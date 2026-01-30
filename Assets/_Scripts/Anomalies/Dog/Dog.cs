using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider),typeof(AudioSource),typeof(DogView))]
public class Dog : MonoBehaviour, IStateSwitcher, IInteractive, IDisposable,IPauseHandler
{
    [SerializeField] private DogSettings _dogSettings;
    private DogData _dogData;
    private DogView _dogView;
    private EventBus _eventBus;
    private Collider _collider;
    private AudioSource _audioSource;
    private CancellationTokenSource _screamerTokenSource;
    private IDogState _currentState;
    private bool _isActivated;

    public bool IsActivated => _isActivated;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _dogView = GetComponent<DogView>();
        _dogData = new DogData(_dogView, _dogSettings, _collider, _audioSource);
        StopScreamerTimer();
        _isActivated = false;

        _eventBus.Subscribe<GamePauseEvent>(SetPaused);
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<GamePauseEvent>(SetPaused);
        StopScreamerTimer();
        _collider = null;
        _eventBus = null;
    }

    public void Interact()
    {
        short _currentBonesCount = _eventBus.Request<GetBonesCount, short>(new GetBonesCount());
        if (_currentBonesCount > 0)
        {
            _eventBus.Invoke(new DogWasFed());
        }
    }

    public void StartHitInteraction()
    {
        _dogView.SetHighlight(true);
    }

    public void StopHitInteraction()
    {
        _dogView.SetHighlight(false);
    }

    public void Appear()
    {
        _eventBus.Invoke(new LightOffEvent());
        _dogView.ShowAppearanceAnimation();
        _collider.enabled = true;
        _isActivated = true;
        SwitchState(new DogBarkingState(this, _dogData, _eventBus));
        StartScreamerTimer();
    }

    public void Leave()
    {
        _eventBus.Invoke(new LightOffEvent());
        _collider.enabled = false;
        _isActivated = false;
        _dogView.SetHighlight(false);
        _dogView.ShowLeavingAnimation();
        StopScreamerTimer();
    }

    private async void StartScreamerTimer()
    {
        if (_screamerTokenSource != null) return;

        _screamerTokenSource = new CancellationTokenSource();
        await ScreamerTimer(_screamerTokenSource.Token);
    }

    private void StopScreamerTimer()
    {
        _screamerTokenSource?.Cancel();
        _screamerTokenSource?.Dispose();
        _screamerTokenSource = null;
    }

    private async Task ScreamerTimer(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _currentState.Update();
            await Task.Yield();
        }
    }

    public void SwitchState(IDogState newState)
    {
        if(_currentState != null)
            _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void SetPaused(GamePauseEvent gamePause)
    {
        if(_isActivated)
        {
            if (gamePause.IsPaused)
            {
                StopScreamerTimer();
            }
            else
            {
                StartScreamerTimer();
            }
        }
    }
}