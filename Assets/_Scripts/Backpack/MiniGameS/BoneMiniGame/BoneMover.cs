using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BoneMiniGame
{
    public class BoneMover : MonoBehaviour
    {
        [SerializeField] private float _mouseSensitivity;
        [SerializeField] private RectTransform _boneTransform;
        [SerializeField] private RectTransform _gamePlaneTransform;
        private PlayerInput _input;
        private EventBus _eventBus;
        private Coroutine _movingUpdate;

        public void Initialize(PlayerInput input, EventBus eventBus)
        {
            _input = input;
            _eventBus = eventBus;
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
                if (NightData.Instance.MenuIsActive == false)
                {  // Игра НЕ на паузе
                    Vector2 mouseDelta = _input.Game.MouseMovement.ReadValue<Vector2>();

                    Vector2 newPosition = _boneTransform.anchoredPosition + mouseDelta * _mouseSensitivity;

                    _boneTransform.anchoredPosition = ClampPosition(newPosition, _gamePlaneTransform);
                }
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
}