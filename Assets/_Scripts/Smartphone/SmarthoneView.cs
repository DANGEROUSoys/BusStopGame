using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SmarthoneView : MonoBehaviour, IDisposable
{
    private SmartphoneData _data;

    public void Initialize(SmartphoneData smartphoneData)
    {
        _data = smartphoneData;
    }

    public void PlayTurnOnAnimation()
    {
        _data.VisualObject.SetActive(true);
        _data.Animator.SetBool("IsTurnedOn", true);
    }
    public void PlayTurnOffAnimation()
    {
        _data.Animator.SetBool("IsTurnedOn", false);
    }
    public void PlayButtonAudio()
    {
        _data.EventBus.Invoke(new PlayAudioEvent(_data.AudioSource, _data.ButtonAudio));
    }

    public void Dispose()
    {
        _data = null;
    }
}
