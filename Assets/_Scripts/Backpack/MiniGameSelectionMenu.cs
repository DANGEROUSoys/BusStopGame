using System.Collections;
using UnityEngine;

public class MiniGameSelectionMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private BoneMiniGame _boneMiniGame;
    private PlayerInput _input;
    private EventBus _eventBus;
    private Coroutine _selectionMenuUpdate;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
    }
    public void StartSelectionMenu()
    {
        _menu.SetActive(true);

        if (_selectionMenuUpdate == null)
            _selectionMenuUpdate = StartCoroutine(SelectionMenuUpdate());
    }

    public void StopSelectionMenu()
    {
        CloseSelectionMenu();
        _eventBus.Invoke(new BackpackWasDisabled());
    }

    public void CloseSelectionMenu()
    {
        _menu.SetActive(false);

        if (_selectionMenuUpdate != null)
            StopCoroutine(_selectionMenuUpdate);
        _selectionMenuUpdate = null;
    }

    public void StartBoneMiniGame()
    {
        CloseSelectionMenu();
        _boneMiniGame.StartGame();
    }
    
    private IEnumerator SelectionMenuUpdate()
    {
        while (true)
        {
            if (_input.Game.Quit.WasPressedThisFrame())
            {
                StopSelectionMenu();
            }
            yield return null;
        }
    }
}
