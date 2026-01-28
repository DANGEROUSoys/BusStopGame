using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(ShadowAnomaly),typeof(ShadowAnomalyView),typeof(Collider))]
public class ShadowHandler : MonoBehaviour, IDisposable
{
    [SerializeField] private ShadowHandlerSettings _settings;
    private ShadowAnomaly _shadow;
    private EventBus _eventBus;
    private CancellationTokenSource _handlerUpdateTokenSource;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _handlerUpdateTokenSource?.Dispose();
        _handlerUpdateTokenSource = null;

        _shadow = GetComponent<ShadowAnomaly>();
        _shadow.Initialize(_eventBus);
    }
    public void Dispose()
    {
        _shadow.Dispose();
        StopShadowHandlerUpdate();
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
        float timer = 0f;
        int timeToAppearingInSeconds = Random.Range(_settings.MinTimeToAppearingInSeconds, _settings.MaxTimeToAppearingInSeconds);
        
        while (!token.IsCancellationRequested)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAppearingInSeconds)
            {
                ActivateShadow();
                StopShadowHandlerUpdate();
                timer = 0f;
            }
            await Task.Yield();
        }
    }

    private void ActivateShadow()
    {
        _shadow.Appear();
    }
}
