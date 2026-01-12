using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ShadowAnomaly : MonoBehaviour, IInteractive, IDisposable
{
    [SerializeField] private float _timeBeforeScreamer;
    [SerializeField] private float _stoppingTimerSpeed;
    [SerializeField] private ShadowAnomalyView _shadowAnomalyView;
    private EventBus _eventBus;
    private float _currentTime;

    private CancellationTokenSource _screamerTokenSource;
    private CancellationTokenSource _stoppingTokenSource;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _currentTime = 0;
        _screamerTokenSource?.Dispose();
        _stoppingTokenSource?.Dispose();
        _screamerTokenSource = null;
        _stoppingTokenSource = null;
        _shadowAnomalyView.Initialize(_timeBeforeScreamer);
    }

    public async void StartScreamerTimer()
    {
        if (_screamerTokenSource != null) return;
        
        _screamerTokenSource = new CancellationTokenSource();
        await ScreamerTimer(_screamerTokenSource.Token);
    }

    public void StopScreamerTimer()
    {
        _screamerTokenSource?.Cancel();
        _screamerTokenSource?.Dispose();
        _screamerTokenSource = null;
    }

    public void StartHitInteraction()
    {
        StopScreamerTimer();
        StartTimerStopping();
    }
    public void StopHitInteraction()
    {
        StopTimerStopping();
        StartScreamerTimer();
    }

    public async void StartTimerStopping()
    {
        if (_stoppingTokenSource != null) return;
        
        _stoppingTokenSource = new CancellationTokenSource();
        await TimerStopping(_stoppingTokenSource.Token);
    }

    public void StopTimerStopping()
    {
        _stoppingTokenSource?.Cancel();
        _stoppingTokenSource?.Dispose();
        _stoppingTokenSource = null;
    }

    private async Task ScreamerTimer(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _timeBeforeScreamer)
            {
                Debug.Log("Игрок проиграл от: ТЕНИ.");
                _eventBus.Invoke(new ShadowScreamerEvent());
                StopScreamerTimer();
            }
            
            _shadowAnomalyView.ChangeShadowScale(_currentTime);
            await Task.Yield();
        }
    }

    private async Task TimerStopping(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _currentTime -= Time.deltaTime * _stoppingTimerSpeed;

            if (_currentTime < 0)
                _currentTime = 0;
            
            _shadowAnomalyView.ChangeShadowScale(_currentTime);
            await Task.Yield();
        }
    }

    public void Interact()
    {
        
    }

    public void Dispose()
    {
        StopTimerStopping();
        StopScreamerTimer();
    }
}