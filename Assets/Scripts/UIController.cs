using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public enum UIType
    {
        MainMenu,
        Game,
        GameOver
    }

    private UIType _currentUI;

    [SerializeField]
    private MainMenuUI _mainMenuUI;

    [SerializeField]
    private GameUI _gameUI;

    [SerializeField]
    private GameOverUI _gameOverUI;

    public void SetCurrentUI(UIType UI)
    {
        _currentUI = UI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _mainMenuUI.HideUI();
        _gameUI.HideUI();
        _gameOverUI.HideUI();

        switch (_currentUI)
        {
            case UIType.MainMenu:
                _mainMenuUI.ShowUI();
                break;
            case UIType.Game:
                _gameUI.ShowUI();
                break;
            case UIType.GameOver:
                _gameOverUI.ShowUI();
                break;
        }
    }
}
