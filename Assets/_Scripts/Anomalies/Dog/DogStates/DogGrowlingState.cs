using UnityEngine;

public class DogGrowlingState : IDogState
{
    private IStateSwitcher _stateSwitcher;
    private DogData _dogData;
    private EventBus _eventBus;
    private float _timer;

    public DogGrowlingState(IStateSwitcher stateSwitcher, DogData dogData, EventBus eventBus)
    {
        _stateSwitcher = stateSwitcher;
        _dogData = dogData;
        _eventBus = eventBus;
    }
    public void Enter()
    {
        Debug.Log("Вход в стадию гавканья!");
        _timer = 0;
        _eventBus.Invoke(new PlayAudioEvent(_dogData.AudioSource, _dogData.GrowlingAudio));
    }
    public void Update()
    {
        Debug.Log("--2--");

        _timer += Time.deltaTime;
        if (_timer > _dogData.StageTime)
        {
            _stateSwitcher.SwitchState(new DogScreamerState(_eventBus, _dogData));
        }
    }
    public void Exit()
    {
        
    }
}
