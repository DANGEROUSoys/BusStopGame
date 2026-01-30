using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NightHandler : MonoBehaviour, IDisposable
{
    private NightData _data;
    private PlayerInput _input;
    private PauseService _pauseService;
    private CancellationTokenSource _updateTokenSource;

    public void Initialize()
    {
        _input = ProjectContext.Instance.PlayerInput;
        _data = new NightData();
        _pauseService = new PauseService();
        
        _data.MenuIsActive = false;
    }

    public void Dispose()
    {
        StopHandlerUpdate();
        _data = null;
        _input = null;
        _pauseService = null;
    }

    public async void StartHandlerUpdate()
    {
        if (_updateTokenSource != null) return;
        
        _updateTokenSource = new CancellationTokenSource();
        await HandlerUpdate(_updateTokenSource.Token);
    }
    public void StopHandlerUpdate()
    {
        _updateTokenSource?.Cancel();
        _updateTokenSource?.Dispose();
        _updateTokenSource = null;
    }

    public async Task HandlerUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_input.Game.Quit.WasPressedThisFrame())
            {
                _data.MenuIsActive = !_data.MenuIsActive;
                _pauseService.SetPaused(_data.MenuIsActive);
            }
            await Task.Yield();
        }
    }
}
