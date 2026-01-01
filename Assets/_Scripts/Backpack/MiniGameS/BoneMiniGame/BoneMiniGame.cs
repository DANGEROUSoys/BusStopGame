using UnityEngine;

public class BoneMiniGame : MonoBehaviour
{
    [SerializeField] private BoneMover _boneMover;
    private PlayerInput _input;
    private EventBus _eventBus;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
    }
    public void StartGame()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        _boneMover.StartMoving();
    }
    
    public void StopGame()
    {
        
    }
}
