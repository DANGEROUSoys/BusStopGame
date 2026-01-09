using UnityEngine;

public class Dog : MonoBehaviour, IInteractive, IDisposable
{
    [SerializeField] private DogView _dogView;

    private EventBus _eventBus;
    private Collider _collider;
    private short _currentBonesCount;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _collider = GetComponent<Collider>();
    }

    public void Interact()
    {
        short _currentBonesCount = _eventBus.Request<DogWasInteracted, short>(new DogWasInteracted());
        if (_currentBonesCount > 0)
        {
            Leave();
            _eventBus.Invoke(new DogWasFed());
        }
    }

    public void StartHitInteraction()
    {
        _dogView.SetHighlight(true);
    }

    public void StopHitInteraction()
    {
        _dogView.SetHighlight(false);
    }

    public void Appear()
    {
        _dogView.ShowAppearanceAnimation();
        _collider.enabled = true; 
    }

    private void Leave()
    {
        _collider.enabled = false;
        _dogView.SetHighlight(false);
        _dogView.ShowLeavingAnimation();
    }

    public void Dispose()
    {
        
    }
}
