using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(ShadowAnomaly),typeof(ShadowAnomalyView),typeof(Collider))]
public class ShadowHandler : MonoBehaviour, IDisposable, IPauseHandler
{
    [SerializeField] private ShadowHandlerSettings _settings;
    private ShadowAnomaly _shadow;
    private EventBus _eventBus;
    private CancellationTokenSource _handlerUpdateTokenSource;
    float _timer;
    int _timeToAppearingInSeconds;

    public void Initialize()
    {
        _eventBus = ProjectContext.Instance.EventBus;
        _handlerUpdateTokenSource?.Dispose();
        _handlerUpdateTokenSource = null;
        _timer = 0;
        _timeToAppearingInSeconds = Random.Range(_settings.MinTimeToAppearingInSeconds, _settings.MaxTimeToAppearingInSeconds);
        _shadow = GetComponent<ShadowAnomaly>();
        _shadow.Initialize(_eventBus);

        _eventBus.Subscribe<GamePauseEvent>(SetPaused);
    }
    public void Dispose()
    {
        _eventBus.UnSubscribe<GamePauseEvent>(SetPaused);
        _shadow.Dispose();
        StopShadowHandlerUpdate();
        _eventBus = null;
    }

    public async void StartShadowHandlerUpdate()
    {
        if (_handlerUpdateTokenSource != null) return;

        _handlerUpdateTokenSource = new CancellationTokenSource();
        await HandlerUpdate(_handlerUpdateTokenSource.Token);
    }
    private void StopShadowHandlerUpdate()
    {
        _handlerUpdateTokenSource?.Cancel();
        _handlerUpdateTokenSource?.Dispose();
        _handlerUpdateTokenSource = null;
    }

    private async Task HandlerUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToAppearingInSeconds)
            {
                ActivateShadow();
                StopShadowHandlerUpdate();
                _timer = 0f;
            }
            await Task.Yield();
        }
    }

    private void ActivateShadow()
    {
        _shadow.Appear();
    }

    public void SetPaused(GamePauseEvent gamePause)
    {
        if (gamePause.IsPaused)
        {
            StopShadowHandlerUpdate();
            _shadow.StopScreamerTimer();
            _shadow.StopTimerStopping();
        }
        else
        {
            StartShadowHandlerUpdate();
            _shadow.StartScreamerTimer();
        }
    }
}
