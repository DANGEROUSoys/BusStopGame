using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider),typeof(AudioSource))]
public class Dog : MonoBehaviour, IStateSwitcher, IInteractive, IDisposable
{
    [SerializeField] private DogView _dogView;
    [SerializeField] private DogSettings _dogSettings;

    private DogData _dogData;
    private EventBus _eventBus;
    private Collider _collider;
    private AudioSource _audioSource;
    private CancellationTokenSource _screamerTokenSource;
    private IDogState _currentState;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _dogData = new DogData(_dogView, _dogSettings, _collider, _audioSource);
        StopScreamerTimer();
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
        StartScreamerTimer();
    }

    public void Leave()
    {
        _eventBus.Invoke(new LightOffEvent());
        _collider.enabled = false;
        _dogView.SetHighlight(false);
        _dogView.ShowLeavingAnimation();
        StopScreamerTimer();
    }

    private async void StartScreamerTimer()
    {
        if (_screamerTokenSource != null) return;
        SwitchState(new DogBarkingState(this, _dogData, _eventBus));

        _screamerTokenSource = new CancellationTokenSource();
        await ScreamerTimer(_screamerTokenSource.Token);
    }
    private async void StopScreamerTimer()
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

    public void Dispose()
    {
        StopScreamerTimer();
        _collider = null;
        _eventBus = null;
    }

}
