using System.Collections;
using UnityEngine;

public class BoneMover : MonoBehaviour
{
    [SerializeField] private RectTransform _boneTransform;
    [SerializeField] private RectTransform _gamePlaneTransform;
    private PlayerInput _input;
    private EventBus _eventBus;
    private Coroutine _movingUpdate;
    private Canvas _canvas;
    private RectTransform _canvasRect;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
        _canvas = _boneTransform.GetComponentInParent<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
    }
    public void StartMoving()
    {
        _movingUpdate = StartCoroutine(Moving());
    }

    public void StopMoving()
    {
        if (_movingUpdate != null) 
            StopCoroutine(_movingUpdate);
        _movingUpdate = null;
    }
    
    private IEnumerator Moving()
    {
        while (true)
        {
            Vector2 mousePosition = _input.Game.MousePosition.ReadValue<Vector2>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                mousePosition,
                _canvas.worldCamera,
                out Vector2 localPoint);

            _boneTransform.anchoredPosition = ClampPosition(localPoint, _gamePlaneTransform);
            yield return null;
        }
    }

    private Vector2 ClampPosition(Vector2 position, RectTransform frame)
    {
        Vector3[] corners = new Vector3[4];
        frame.GetLocalCorners(corners);

        Vector2 itemSize = _boneTransform.rect.size * _boneTransform.localScale;
        Vector2 halfSize = itemSize * 0.5f;

        float minX = corners[0].x + halfSize.x + 10;
        float maxX = corners[2].x - halfSize.x - 10;
        float minY = corners[0].y + halfSize.y + 10;
        float maxY = corners[2].y - halfSize.y - 10;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
}
