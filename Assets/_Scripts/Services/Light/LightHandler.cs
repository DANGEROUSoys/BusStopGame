using UnityEngine;

public class LightHandler : MonoBehaviour, IDisposable
{
    [SerializeField] private Animator _animator;
    private EventBus _eventBus;

    public void Initialize()
    {
        _eventBus = ProjectContext.Instance.EventBus;

        _eventBus.Subscribe<LightOffEvent>(PlayFlashingLights);
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<LightOffEvent>(PlayFlashingLights);
        _eventBus = null;
    }

    private void PlayFlashingLights()
    {
        _animator.SetTrigger("FlashingLights");
    }
}
