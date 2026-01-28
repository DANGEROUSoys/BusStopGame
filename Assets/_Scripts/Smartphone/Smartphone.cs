using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Smartphone : MonoBehaviour, IDisposable
{
    private PlayerInput _input;
    private CancellationTokenSource _updateTokenSource;
    private Animator _animator;
    private bool _smartphoneIsActive;

    public void Initialize(PlayerInput input)
    {
        _input = input;
        _animator = GetComponent<Animator>();
    }

    public async void StartSmartphoneUpdate()
    {
        if (_updateTokenSource != null) return;

        _updateTokenSource = new CancellationTokenSource();
        await SmartphoneUpdate(_updateTokenSource.Token);
    }
    public void StopSmartphoneUpdate()
    {
        _updateTokenSource?.Cancel();
        _updateTokenSource?.Dispose();
        _updateTokenSource = null;
    }

    private async void TurnOn()
    {
        _animator.SetBool("IsTurnedOn", true);
    }

    private void TurnOff()
    {
        _animator.SetBool("IsTurnedOn", false);
    }

    private async Task SmartphoneUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_input.Game.AlterInteract.WasPressedThisFrame())
            {
                if (_smartphoneIsActive == false) // Если телефон ещё НЕ включен
                {
                    TurnOn();
                    _smartphoneIsActive = true;
                }
                else
                {
                    TurnOff();
                    _smartphoneIsActive = false;
                }
            }
            await Task.Yield();
        }
    }

    public void Dispose()
    {
        _input.Dispose();
        StopSmartphoneUpdate();
        _animator = null;
    }
}
