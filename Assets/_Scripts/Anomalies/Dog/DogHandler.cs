using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DogHandler : MonoBehaviour, IDisposable
{
    [SerializeField] private DogHandlerSettings _settings;
    private Dog _dog;
    private EventBus _eventBus;
    private CancellationTokenSource _handlerUpdateTokenSource;
    private bool _isFirstAppearing;
    private bool _isDogActivated;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _handlerUpdateTokenSource?.Dispose();
        _handlerUpdateTokenSource = null;
        _isFirstAppearing = true;
        _isDogActivated = false;

        _dog = GetComponent<Dog>();
        _dog.Initialize(_eventBus);

        _eventBus.Subscribe<DogWasFed>(DeactivateDog);
    }
    public void Dispose()
    {
        _dog.Dispose();
        StopDogHandlerUpdate();

        _eventBus.UnSubscribe<DogWasFed>(DeactivateDog);
    }

    public async void StartDogHandlerUpdate()
    {
        if (_handlerUpdateTokenSource != null) return;

        _handlerUpdateTokenSource = new CancellationTokenSource();
        await HandlerUpdate(_handlerUpdateTokenSource.Token);
    }
    private void StopDogHandlerUpdate()
    {
        _handlerUpdateTokenSource?.Cancel();
        _handlerUpdateTokenSource?.Dispose();
        _handlerUpdateTokenSource = null;
    }

    private async Task HandlerUpdate(CancellationToken token)
    {
        float timer = 0f;
        int timeToAppearingInSeconds;
        if (_isFirstAppearing) // <-- для первого появления
        {
            _isFirstAppearing = false;
            timeToAppearingInSeconds = Random.Range(_settings.MinTimeToFirstAppearingInSeconds, _settings.MaxTimeToFirstAppearingInSeconds);
        }
        else // <-- для последующих появлений
        {
            timeToAppearingInSeconds = Random.Range(_settings.MinTimeToAppearingInSeconds, _settings.MaxTimeToAppearingInSeconds);
        }
        
        while (!token.IsCancellationRequested)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAppearingInSeconds)
            {
                ActivateDog();
                StopDogHandlerUpdate();
            }
            await Task.Yield();
        }
    }

    private void ActivateDog()
    {
        _dog.Appear();
        _isDogActivated = true;
    }
    private void DeactivateDog()
    {
        _dog.Leave();
        _isDogActivated = false;
        StartDogHandlerUpdate();
    }
}