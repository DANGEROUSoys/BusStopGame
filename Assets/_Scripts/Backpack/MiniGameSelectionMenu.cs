using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MiniGameSelectionMenu : MonoBehaviour, IDisposable
{
    [SerializeField] private GameObject _activeBoneMiniGameButton;
    [SerializeField] private GameObject _inactiveBoneMiniGameButton;
    [SerializeField] private GameObject _activeEnergyDrinkMiniGameButton;
    [SerializeField] private GameObject _inactiveEnergyDrinkMiniGameButton;
    [SerializeField] private GameObject _menu;
    [SerializeField] private BoneMiniGame.BoneMiniGame _boneMiniGame;
    private PlayerInput _input;
    private EventBus _eventBus;
    private CancellationTokenSource _menuUpdateTokenSource;

    public void Initialize(PlayerInput input, EventBus eventBus)
    {
        _input = input;
        _eventBus = eventBus;
        _menuUpdateTokenSource?.Dispose();
        _menuUpdateTokenSource = null;

        _boneMiniGame.Initialize(_input, _eventBus);
    }

    public void Dispose()
    {
        _menuUpdateTokenSource?.Cancel();
        _menuUpdateTokenSource?.Dispose();
        _menuUpdateTokenSource = null;
    }

    public async void StartSelectionMenu()
    {
        _menu.SetActive(true);
        ShowButtons();

        if (_menuUpdateTokenSource != null) return;
        _menuUpdateTokenSource = new CancellationTokenSource();
        await SelectionMenuUpdate(_menuUpdateTokenSource.Token);
    }

    public void StopSelectionMenu()
    {
        CloseSelectionMenu();
        _eventBus.Invoke(new BackpackWasDisabled());
    }

    public void CloseSelectionMenu()
    {
        _menu.SetActive(false);

        _menuUpdateTokenSource?.Cancel();
        _menuUpdateTokenSource?.Dispose();
        _menuUpdateTokenSource = null;
    }

    public void StartBoneMiniGame()
    {
        CloseSelectionMenu();
        _boneMiniGame.StartGame();
    }
    
    private async Task SelectionMenuUpdate(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_input.Game.Quit.WasPressedThisFrame())
            {
                StopSelectionMenu();
            }
            await Task.Yield();
        }
    }

    private void ShowButtons()
    {
        short bones = _eventBus.Request<GetBonesCount, short>(new GetBonesCount());
        short energyDrinks = _eventBus.Request<GetEnergyDrinksCount, short>(new GetEnergyDrinksCount());
        short maxBones = _eventBus.Request<GetMaxBonesCount, short>(new GetMaxBonesCount());
        short maxEnergyDrinks = _eventBus.Request<GetMaxEnergyDrinksCount, short>(new GetMaxEnergyDrinksCount());

        if(bones < maxBones)
        {
            _activeBoneMiniGameButton.SetActive(true);
            _inactiveBoneMiniGameButton.SetActive(false);
        }
        else
        {
            _activeBoneMiniGameButton.SetActive(false);
            _inactiveBoneMiniGameButton.SetActive(true);
        }

        if(energyDrinks < maxEnergyDrinks)
        {
            _activeEnergyDrinkMiniGameButton.SetActive(true);
            _inactiveEnergyDrinkMiniGameButton.SetActive(false);
        }
        else
        {
            _activeEnergyDrinkMiniGameButton.SetActive(false);
            _inactiveEnergyDrinkMiniGameButton.SetActive(true);
        }
    }
}
