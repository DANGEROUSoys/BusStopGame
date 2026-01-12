using UnityEngine;

public class DogGrowlingState : IDogState
{
    private IStateSwitcher _stateSwitcher;
    private DogSettings _dogSettings;
    private EventBus _eventBus;
    private float _timer;

    public DogGrowlingState(IStateSwitcher stateSwitcher, DogSettings dogSettings, EventBus eventBus)
    {
        _stateSwitcher = stateSwitcher;
        _dogSettings = dogSettings;
        _eventBus = eventBus;
    }
    public void Enter()
    {
        Debug.Log("Вход в стадию гавканья!");
        _timer = 0;
    }
    public void Update()
    {
        Debug.Log("--2--");

        _timer += Time.deltaTime;
        if (_timer > _dogSettings.StageTime)
        {
            _stateSwitcher.SwitchState(new DogScreamerState(_eventBus));
        }
    }
    public void Exit()
    {
        
    }
}
