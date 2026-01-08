using UnityEngine;

public class Dog : MonoBehaviour, IInteractive
{
    [SerializeField] private DogView _dogView;

    private EventBus _eventBus;
    private Inventory.Inventory _inventory; //гюбхяхлнярэ, мсфмн ондслюрэ йюй сапюрэ
    private Collider _collider;
    private short _currentBonesCount => _inventory.BonesCount;

    public void Initialize(EventBus eventBus, Inventory.Inventory inventory)
    {
        _eventBus = eventBus;
        _inventory = inventory;
        _collider = GetComponent<Collider>();
    }

    public void Interact()
    {
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
        _collider.enabled = true; // ядекюрэ бйкчвемхе йнккюидепю оняке юмхлюжхх онъбкемхъ янаюйх
    }

    private void Leave()
    {
        _collider.enabled = false;
        _dogView.SetHighlight(false);
        _dogView.ShowLeavingAnimation();
    }
}
