using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerRaycast : IDisposable
{
    private CancellationTokenSource _raycastTokenSource;
    private PlayerInput _input;
    private EventBus _eventBus;
    private Camera _camera;
    private IInteractive _currentInteractiveObject;

    public PlayerRaycast(PlayerInput input, EventBus eventBus, Camera camera)
    {
        _input = input;
        _eventBus = eventBus;
        _camera = camera;
        StopRaycast();

        _eventBus.Subscribe<BackpackWasEnabled>(StopRaycast);
        _eventBus.Subscribe<BackpackWasDisabled>(StartRaycast);
    }

    public void Dispose()
    {
        StopRaycast();
        _eventBus.UnSubscribe<BackpackWasEnabled>(StopRaycast);
        _eventBus.UnSubscribe<BackpackWasDisabled>(StartRaycast);
    }

    public async void StartRaycast()
    {
        if (_raycastTokenSource != null) return;

        _raycastTokenSource = new CancellationTokenSource();
        await RaycastUpdate(_raycastTokenSource.Token);
    }

    public void StopRaycast()
    {
        _raycastTokenSource?.Cancel();
        _raycastTokenSource?.Dispose();
        _raycastTokenSource = null;
    }
    
    private async Task RaycastUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Vector2 mousePosition = _input.Game.MousePosition.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent<IInteractive>(out IInteractive interactiveObject))
                {
                    if (_currentInteractiveObject != null)
                        _currentInteractiveObject.StopHitInteraction();
                    _currentInteractiveObject = interactiveObject;
                    _currentInteractiveObject.StartHitInteraction();

                    if (_input.Game.LeftClick.WasPressedThisFrame())
                    {
                        _currentInteractiveObject.Interact();
                    }
                }
                else
                {
                    if (_currentInteractiveObject != null)
                    {
                        _currentInteractiveObject.StopHitInteraction();
                        _currentInteractiveObject = null;
                    }
                }
            }
            else
            {
                if (_currentInteractiveObject != null)
                {
                    _currentInteractiveObject.StopHitInteraction();
                    _currentInteractiveObject = null;
                }
            }
            await Task.Yield();
        }
    }
}
