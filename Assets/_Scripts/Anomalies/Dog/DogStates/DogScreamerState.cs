using UnityEngine;

public class DogScreamerState : IDogState
{
    private EventBus _eventBus;
    private DogData _dogData;

    public DogScreamerState(EventBus eventBus, DogData dogData)
    {
        _eventBus = eventBus;
        _dogData = dogData;
    }
    public void Enter()
    {
        Debug.Log("Игрок проиграл от: --СОБАКИ--.");
        _eventBus.Invoke(new DogScreamerEvent());
    }
    public void Update()
    {
        
    }
    public void Exit()
    {
        
    }
}