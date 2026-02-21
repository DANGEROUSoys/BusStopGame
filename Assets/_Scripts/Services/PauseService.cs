using System.Collections.Generic;
using UnityEngine;

public class PauseService: IDisposable
{
    private EventBus _eventBus;

    public PauseService()
    {
        _eventBus = ProjectContext.Instance.EventBus;
    }

    public void Dispose()
    {
        _eventBus = null;
    }

    public void SetPaused(bool isPaused)
    {
        _eventBus.Invoke(new GamePauseEvent(isPaused));
    }
}
