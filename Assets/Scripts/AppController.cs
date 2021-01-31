using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : Singleton<AppController>
{
    public enum AppState
    {
        Starting,
        MainMenu,
        Game
    }

    public AppState _currentState { get; private set; }

    public void SetCurrentState(AppState State)
    {
        _currentState = State;
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case AppState.Starting:
                break;
            case AppState.MainMenu:
                break;
            case AppState.Game:
                break;
        }
    }

        void Awake()
    {
        _currentState = AppState.Starting;
        // Animation de départ, mur qui s'écroule ?
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        _currentState = AppState.MainMenu;
        GameController.Instance.InitializeGame();
        UIController.Instance.SetCurrentUI(UIController.UIType.MainMenu);
    }
}
