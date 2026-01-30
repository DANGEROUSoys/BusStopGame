using UnityEngine;
using UnityEngine.Windows;

public class ProjectContext :  MonoBehaviour, IDisposable
{
    public static ProjectContext Instance { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public EventBus EventBus { get; private set; }

    public void Initialize() // мсфмн онлеярхрэ мю гюцпсгнвмсч яжемс х ядекюрэ вепег "AWAKE"
    {
        Instance = this;
        DontDestroyOnLoad(this);
        PlayerInput = new PlayerInput();
        PlayerInput.Enable();
        EventBus = new EventBus();
    }

    public void Dispose()
    {
        PlayerInput.Disable();
        PlayerInput.Dispose();
        EventBus.Dispose();
        Instance = null;
    }
}
