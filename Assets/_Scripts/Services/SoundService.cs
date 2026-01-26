public class SoundService : IDisposable
{
    private EventBus _eventBus;

    public SoundService(EventBus eventBus)
    {
        _eventBus = eventBus;

        _eventBus.Subscribe<PlaySoundEvent>(PlayAudio);
    }

    public void Dispose()
    {
        _eventBus.UnSubscribe<PlaySoundEvent>(PlayAudio);
    }

    private void PlayAudio(PlaySoundEvent soundEvent)
    {
        soundEvent.AudioSource.clip = soundEvent.Clip;
        soundEvent.AudioSource.Play();
    }
}
