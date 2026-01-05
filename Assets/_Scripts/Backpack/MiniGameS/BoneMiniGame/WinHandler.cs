using System.Collections;
using UnityEngine;

namespace BoneMiniGame
{
    public class WinHandler : MonoBehaviour
    {
        [SerializeField] private Collider2D _boneCollider;
        [SerializeField] private Collider2D _frameCollider;
        [SerializeField] private Bar _progressBar;
        [SerializeField] private float _timeForWin;

        private float _currentProgress;
        private float _progressPerFrame;
        private EventBus _eventBus;
        private Coroutine _handlerUpdate;

        public void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;

            _currentProgress = 0;
            _progressBar.SetProgress(_currentProgress);
        }

        public void StartUpdate()
        {
            _currentProgress = 0;
            _progressBar.SetProgress(_currentProgress);

            if (_timeForWin != 0)
                _progressPerFrame = 1 / _timeForWin;
            else
                _progressPerFrame = 1;

            _handlerUpdate = StartCoroutine(HandlerUpdate());
        }

        public void StopUpdate()
        {
            if (_handlerUpdate != null)
                StopCoroutine(_handlerUpdate);
            _handlerUpdate = null;
        }
        private bool IsInside(Collider2D outer, Collider2D inner)
        {
            Bounds outerBounds = outer.bounds;
            Bounds innerBounds = inner.bounds;

            return outerBounds.Contains(innerBounds.min) && outerBounds.Contains(innerBounds.max);
        }

        private IEnumerator HandlerUpdate()
        {
            while (true)
            {
                if (IsInside(_frameCollider, _boneCollider))
                {
                    _currentProgress += Time.deltaTime * _progressPerFrame;
                    _progressBar.SetProgress(_currentProgress);
                }
                if (_currentProgress >= 1)
                {
                    StopUpdate();
                    _eventBus.Invoke(new BoneMiniGameWasComplited());
                    
                    Debug.Log("Игрок прошёл мини-игру. Нужно отправить эвент об этом!");
                }
                yield return null;
            }
        }
    }
}