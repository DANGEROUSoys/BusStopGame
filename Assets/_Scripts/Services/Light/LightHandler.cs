using UnityEngine;

public class LightHandler : MonoBehaviour, IDisposable
{
    [SerializeField] private Animator _animator;
    private EventBus _eventBus;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;

        _eventBus.Subscribe<LightOffEvent>(PlayFlashingLights);
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<LightOffEvent>(PlayFlashingLights);
    }

    private void PlayFlashingLights()
    {
        _animator.SetTrigger("FlashingLights");
    }
}
