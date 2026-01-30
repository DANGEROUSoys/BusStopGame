using UnityEngine;

public struct GamePauseEvent : IEvent
{
    public bool IsPaused;

    public GamePauseEvent(bool isPaused)
    {
        IsPaused = isPaused;
    }
}
