using UnityEngine;

public class DogBarkingState : IDogState
{
    private IStateSwitcher _stateSwitcher;
    private DogSettings _dogSettings;
    private EventBus _eventBus;
    private float _timer;

    public DogBarkingState(IStateSwitcher stateSwitcher, DogSettings dogSettings, EventBus eventBus)
    {
        _stateSwitcher = stateSwitcher;
        _dogSettings = dogSettings;
        _eventBus = eventBus;
    }

    public void Enter()
    {
        Debug.Log("Вход в стадию лая");
        _timer = 0;
    }
    public void Update()
    {
        Debug.Log("--1--");
        _timer += Time.deltaTime;
        if (_timer > _dogSettings.StageTime)
        {
            _stateSwitcher.SwitchState(new DogGrowlingState(_stateSwitcher, _dogSettings, _eventBus));
        }
    }
    public void Exit()
    {
        
    }
}