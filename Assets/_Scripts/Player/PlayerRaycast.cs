using System.Collections;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    private PlayerInput _input;
    private EventBus _eventBus;
    private Coroutine _raycastUpdate;
    private Camera _camera;

    private IInteractive _currentInteractiveObject;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
        _camera = Camera.main;
        _currentInteractiveObject = null;

        _eventBus.Subscribe<BackpackWasEnabled>(StopRaycast);
        _eventBus.Subscribe<BackpackWasDisabled>(StartRaycast);
    }

    private void OnDisable()
    {
        _eventBus.UnSubscribe<BackpackWasEnabled>(StopRaycast);
        _eventBus.UnSubscribe<BackpackWasDisabled>(StartRaycast);
    }

    public void StartRaycast()
    {
        _raycastUpdate = StartCoroutine(RaycastUpdate());
    }

    public void StopRaycast()
    {
        StopCoroutine(_raycastUpdate);
    }
    
    private IEnumerator RaycastUpdate()
    {
        while (true)
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
            yield return null;
        }
    }
}
