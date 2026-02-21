using UnityEngine;
using UnityEngine.Windows;

public class ProjectContext :  MonoBehaviour
{
    public static ProjectContext Instance { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public EventBus EventBus { get; private set; }
    public SoundService SoundService { get; private set; }

    public void Initialize() // ����� ��������� �� ����������� ����� � ������� ����� "AWAKE"
    {
        Instance = this;
        DontDestroyOnLoad(this);
        PlayerInput = new PlayerInput();
        PlayerInput.Enable();
        EventBus = new EventBus();
        SoundService = new SoundService();
    }

    //public void OnDisable()
    //{
    //    PlayerInput.Disable();
    //    PlayerInput.Dispose();
    //    EventBus.Dispose();
    //    SoundService.Dispose();
    //    Instance = null;
    //}
}
