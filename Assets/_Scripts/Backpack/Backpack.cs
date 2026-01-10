using UnityEngine;

public class Backpack : MonoBehaviour, IInteractive, IDisposable
{
    [SerializeField] private BackpackView _backpackView;
    [SerializeField] private MiniGameSelectionMenu _miniGameSelectionMenu;
    private bool _backpackIsInteracted;
    private EventBus _eventBus;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _eventBus = eventBus;
        _miniGameSelectionMenu.Initialize(input, eventBus);

    }

    public void Interact()
    {
        _eventBus.Invoke(new BackpackWasEnabled());
        _miniGameSelectionMenu.StartSelectionMenu();

        _backpackIsInteracted = true;
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

    public void Dispose()
    {
        _miniGameSelectionMenu.Dispose();
    }
}
