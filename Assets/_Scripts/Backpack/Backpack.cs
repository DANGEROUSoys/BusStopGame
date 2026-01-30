using UnityEngine;

public class Backpack : MonoBehaviour, IInteractive, IDisposable, IPauseHandler
{
    [SerializeField] private BackpackView _backpackView;
    [SerializeField] private MiniGameSelectionMenu _miniGameSelectionMenu;
    private EventBus _eventBus;
    private PlayerInput _input;

    public void Initialize()
    {
        _input = ProjectContext.Instance.PlayerInput;
        _eventBus = ProjectContext.Instance.EventBus; ;
        _miniGameSelectionMenu.Initialize(_input, _eventBus);

        _eventBus.Subscribe<GamePauseEvent>(SetPaused);
    }
    public void Dispose()
    {
        _eventBus.UnSubscribe<GamePauseEvent>(SetPaused);
        _miniGameSelectionMenu.Dispose();
    }

    public void Interact()
    {
        _eventBus.Invoke(new BackpackWasEnabled());
        _miniGameSelectionMenu.StartSelectionMenu();

        StopHitInteraction();
    }

    public void StartHitInteraction()
    {
        _backpackView.SetHighlight(true);
    }

    public void StopHitInteraction()
    {
        _backpackView.SetHighlight(false);
    }

    public void SetPaused(GamePauseEvent gamePause)
    {
        
    }
}
