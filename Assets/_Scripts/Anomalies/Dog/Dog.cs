using UnityEngine;

public class Dog : MonoBehaviour, IInteractive, IDisposable
{
    [SerializeField] private DogView _dogView;

    private EventBus _eventBus;
    private Collider _collider;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _collider = GetComponent<Collider>();
    }

    public void Interact()
    {
        short _currentBonesCount = _eventBus.Request<GetBonesCount, short>(new GetBonesCount());
        if (_currentBonesCount > 0)
        {
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
        Debug.Log("Собака появилась!");
        _dogView.ShowAppearanceAnimation();
        _collider.enabled = true; 
    }

    public void Leave()
    {
        Debug.Log("Собака ушла!");
        _collider.enabled = false;
        _dogView.SetHighlight(false);
        _dogView.ShowLeavingAnimation();
    }

    public void Dispose()
    {
        
    }
}
