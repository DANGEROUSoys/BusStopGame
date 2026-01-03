using System.Collections;
using UnityEngine;

public class BoneMiniGame : MonoBehaviour
{
    [SerializeField] private BoneMover _boneMover;
    private PlayerInput _input;
    private EventBus _eventBus;
    private Coroutine _mainUpdate;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;

        _boneMover.Initialize(_input, _eventBus);
    }
    public void StartGame()
    {
        gameObject.SetActive(true);
        Cursor.visible = false;

        _mainUpdate = StartCoroutine(MainUpdate());
        _boneMover.StartMoving();
    }
    
    public void StopGame()
    {
        _boneMover.StopMoving();

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
            if (_input.Game.Quit.WasPressedThisFrame())
            {
                StopGame();
            }
            yield return null;
        }
    }
}
