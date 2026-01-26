using UnityEngine;

public class DogBarkingState : IDogState
{
    private IStateSwitcher _stateSwitcher;
    private DogData _dogData;
    private EventBus _eventBus;
    private float _timer;

    public DogBarkingState(IStateSwitcher stateSwitcher, DogData dogData, EventBus eventBus)
    {
        _stateSwitcher = stateSwitcher;
        _dogData = dogData;
        _eventBus = eventBus;
    }

    public void Enter()
    {
        Debug.Log("Вход в стадию лая");
        _timer = 0;
        _eventBus.Invoke(new PlaySoundEvent(_dogData.AudioSource, _dogData.BarkingAudio));
    }
    public void Update()
    {
        Debug.Log("--1--");
        _timer += Time.deltaTime;
        if (_timer > _dogData.StageTime)
        {
            _stateSwitcher.SwitchState(new DogGrowlingState(_stateSwitcher, _dogData, _eventBus));
        }
    }
    public void Exit()
    {
        
    }
}