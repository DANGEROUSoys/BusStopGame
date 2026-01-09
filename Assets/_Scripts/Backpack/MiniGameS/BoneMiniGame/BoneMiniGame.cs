using System.Collections;
using UnityEngine;

namespace BoneMiniGame 
{ 
    public class BoneMiniGame : MonoBehaviour, IDisposable
    {
        [SerializeField] private BoneMover _boneMover;
        [SerializeField] private FrameMovement _frameMovement;
        [SerializeField] private WinHandler _winHandler;
        private PlayerInput _input;
        private EventBus _eventBus;
        private Coroutine _mainUpdate;
    
        public void Initialize(PlayerInput input, EventBus eventBus)
        {
            _input = input;
            _eventBus = eventBus;
    
            _boneMover.Initialize(_input, _eventBus);
            _frameMovement.Initialize();
            _winHandler.Initialize(_eventBus);

            _eventBus.Subscribe<BoneMiniGameWasComplited>(StopGame);
        }
        public void StartGame()
        {
            gameObject.SetActive(true);
            Cursor.visible = false;
    
            _mainUpdate = StartCoroutine(MainUpdate());
            _boneMover.StartMoving();
            _frameMovement.StartMoving();
            _winHandler.StartUpdate();
        }
        
        public void StopGame()
        {
            _boneMover.StopMoving();
            _frameMovement.StopMoving();
            _winHandler.StopUpdate();
    
            if (_mainUpdate != null)
                StopCoroutine(_mainUpdate);
            _mainUpdate = null;
    
            gameObject.SetActive(false);
            Cursor.visible = true;
            _eventBus.Invoke(new BackpackWasDisabled());
        }
    
        private IEnumerator MainUpdate()
        {
            while (true)
            {
                if (_input.Game.Quit.WasPressedThisFrame() || _input.Game.RightClick.WasPressedThisFrame())
                {
                    StopGame();
                }
                yield return null;
            }
        }

        public void Dispose()
        {
            
        }
    }
}