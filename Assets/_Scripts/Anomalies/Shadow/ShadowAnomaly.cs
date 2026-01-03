using System.Collections;
using UnityEngine;

public class ShadowAnomaly : MonoBehaviour, IInteractive
{
    // ЛУЧШЕ ПОТОМ ПЕРЕДЕЛАТЬ ЛОГИКУ ЧЕРЕЗ ПАТТЕРН STATE MACHINE
    [SerializeField] private float _timeBeforeScreamer;
    [SerializeField] private float _stoppingTimerSpeed;
    [SerializeField] private ShadowAnomalyView _shadowAnomalyView;
    private float _currentTime;

    private Coroutine _screamerTimer;
    private Coroutine _timerStopping;

    public void Initialize()
    {
        _currentTime = 0;
        _screamerTimer = null;
        _shadowAnomalyView.Initialize(_timeBeforeScreamer);
    }

    public void StartScreamerTimer()
    {
        if (_screamerTimer == null)
            _screamerTimer = StartCoroutine(ScreamerTimer());
    }

    public void StopScreamerTimer()
    {
        if (_screamerTimer != null)
            StopCoroutine(_screamerTimer);
        _screamerTimer = null;
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

    public void StartTimerStopping()
    {
        if (_timerStopping == null)
            _timerStopping = StartCoroutine(TimerStopping());
    }
    public void StopTimerStopping()
    {
        if (_timerStopping != null)
            StopCoroutine(_timerStopping);
        _timerStopping = null;
    }

    private IEnumerator ScreamerTimer()
    {
        while (true)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _timeBeforeScreamer)
            {
                Debug.Log("Игрок проиграл от тени. Нужно послать эвент об этом!");
                StopScreamerTimer();
            }
            _shadowAnomalyView.ChangeShadowScale(_currentTime);

            //Debug.Log(_currentTime);
            yield return null;
        }
    }
    private IEnumerator TimerStopping()
    {
        while (true)
        {
            _currentTime -= Time.deltaTime * _stoppingTimerSpeed;

            if(_currentTime < 0)
                _currentTime = 0;
            _shadowAnomalyView.ChangeShadowScale(_currentTime);
            
            Debug.Log(_currentTime);
            yield return null;
        }
    }

    public void Interact()
    {
        
    }

    public void StopInteract()
    {
        
    }
}