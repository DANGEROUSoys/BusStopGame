
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreamerService : IDisposable
{
    private EventBus _eventBus;

    public ScreamerService()
    {
        _eventBus = ProjectContext.Instance.EventBus;

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
        _eventBus.UnSubscribe<DogScreamerEvent>(PlayDogScreamer);
        _eventBus.UnSubscribe<ShadowScreamerEvent>(PlayShadowScreamer);
        _eventBus = null;
    }
}