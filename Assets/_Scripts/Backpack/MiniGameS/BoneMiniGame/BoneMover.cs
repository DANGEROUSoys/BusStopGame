using System.Collections;
using UnityEngine;

public class BoneMover : MonoBehaviour
{
    [SerializeField] private GameObject _bone;
    private PlayerInput _input;
    private EventBus _eventBus;
    private Camera _camera;
    private Coroutine _selectionMenuUpdate;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
    }
    public void StartMoving()
    {
        
    }

    public void StopMoving()
    {
        
    }
    
    private IEnumerator Moving()
    {
        while (true)
        {
            Vector2 mousePosition = _input.Game.MousePosition.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent<Background>(out Background background))
                {
                    _bone.transform.position = hit.transform.position;
                }
            }
            yield return null;
        }
    }
}
