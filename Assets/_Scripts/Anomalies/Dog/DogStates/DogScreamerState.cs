using UnityEngine;

public class DogScreamerState : IDogState
{
    private EventBus _eventBus;

    public DogScreamerState(EventBus eventBus)
    {
        _eventBus = eventBus;
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