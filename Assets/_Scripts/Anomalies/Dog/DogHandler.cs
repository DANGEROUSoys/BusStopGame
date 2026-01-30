using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Dog),typeof(DogView),typeof(Collider))]
public class DogHandler : MonoBehaviour, IDisposable, IPauseHandler
{
    [SerializeField] private DogHandlerSettings _settings;
    private Dog _dog;
    private EventBus _eventBus;
    private CancellationTokenSource _handlerUpdateTokenSource;
    private bool _isFirstAppearing;
    private float _timer;

    public void Initialize()
    {
        _eventBus = ProjectContext.Instance.EventBus;
        _handlerUpdateTokenSource?.Dispose();
        _handlerUpdateTokenSource = null;
        _isFirstAppearing = true;
        _timer = 0f;

        _dog = GetComponent<Dog>();
        _dog.Initialize(_eventBus);

        _eventBus.Subscribe<DogWasFed>(DeactivateDog);
        _eventBus.Subscribe<GamePauseEvent>(SetPaused);
    }
    public void Dispose()
    {
        _eventBus.UnSubscribe<DogWasFed>(DeactivateDog);
        _eventBus.UnSubscribe<GamePauseEvent>(SetPaused);
        _dog.Dispose();
        StopDogHandlerUpdate();
        _eventBus = null;
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
            _timer += Time.deltaTime;
            if (_timer >= timeToAppearingInSeconds)
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
    }
    private void DeactivateDog()
    {
        _dog.Leave();
        StartDogHandlerUpdate();
    }

    public void SetPaused(GamePauseEvent gamePause)
    {
        if (_dog.IsActivated)
        {
            if (gamePause.IsPaused)
            {
                StopDogHandlerUpdate();
            }
            else
            {
                StartDogHandlerUpdate();
            }
        }
    }
}