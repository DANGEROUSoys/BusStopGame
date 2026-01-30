using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

namespace BoneMiniGame
{
    public class FrameMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _minDelay = 1f;
        [SerializeField] private float _maxDelay = 3f;
        [SerializeField] private RectTransform _gamePlaneTransform;
        [SerializeField] private RectTransform _frameTransform;

        private float _boardersOffset;
        private bool _isMoving = false;
        private Vector2 _targetPosition;
        private Coroutine _movingUpdate;

        public void Initialize()
        {
            _boardersOffset = _frameTransform.rect.width / 2 + 10;
            _frameTransform.anchoredPosition = new Vector2(0, 0);
            SetNewTarget();
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

        private void SetNewTarget()
        {
            float randomX = Random.Range(_gamePlaneTransform.rect.xMin + _boardersOffset, _gamePlaneTransform.rect.xMax - _boardersOffset);
            float randomY = Random.Range(_gamePlaneTransform.rect.yMin + _boardersOffset, _gamePlaneTransform.rect.yMax - _boardersOffset);
            _targetPosition = new Vector2(randomX, randomY);
        }

        private IEnumerator Moving()
        {
            while (true)
            {
                _isMoving = true;
                Vector2 startPos = _frameTransform.anchoredPosition;
                float distance = Vector2.Distance(startPos, _targetPosition);
                float timeTaken = distance / _speed;
                float elapsedTime = 0f;

                while (elapsedTime <= timeTaken)
                {
                    _frameTransform.anchoredPosition = Vector2.Lerp(startPos, _targetPosition, elapsedTime / timeTaken);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _frameTransform.anchoredPosition = _targetPosition;
                _isMoving = false;

                yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
                SetNewTarget();
            }
        }
    }
}