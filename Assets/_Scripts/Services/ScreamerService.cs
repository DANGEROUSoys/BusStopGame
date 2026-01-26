
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreamerService : IDisposable
{
    private EventBus _eventBus;

    public ScreamerService(EventBus eventBus)
    {
        _eventBus = eventBus;

        _eventBus.Subscribe<DogScreamerEvent>(PlayDogScreamer);
        _eventBus.Subscribe<ShadowScreamerEvent>(PlayShadowScreamer);
    }

    private void PlayShadowScreamer()
    {
        SceneManager.LoadScene(0);
    }
    private void PlayDogScreamer()
    {
        SceneManager.LoadScene(0);
    }

    public void Dispose()
    {
        
    }
}