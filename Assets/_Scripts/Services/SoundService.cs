public class SoundService : IDisposable
{
    private EventBus _eventBus;

    public SoundService()
    {
        _eventBus = ProjectContext.Instance.EventBus;

        _eventBus.Subscribe<PlayAudioEvent>(PlayAudio);
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<PlayAudioEvent>(PlayAudio);
        _eventBus = null;
    }

    private void PlayAudio(PlayAudioEvent soundEvent)
    {
        soundEvent.AudioSource.clip = soundEvent.Clip;
        soundEvent.AudioSource.Play();
    }
}
