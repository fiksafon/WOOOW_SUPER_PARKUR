using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIState CurrentState { get; private set; } = UIState.Systems;
    private EventBus _inet;

    public void Init(EventBus inet)
    {
        _inet = inet;
        _inet._OnButtonClick += OnUiButton;
        _inet._onGameStateChanged += OnGameStateChanged;
    }                       
    private void OnUiButton(ButtonType button)
    {
        switch(button)
        {
            case ButtonType.start:
                _inet.RequestGameState(GameState.playing);
                break;

            case ButtonType.info:
                break;

            case ButtonType.settings:
                break;

            case ButtonType.exit:
                Application.Quit();
                break;

            case ButtonType.mainMenu:
                ChangeState(UIState.MainMenu);
                break;
        }
    }
    private void ChangeState(UIState newState)
    {
        if(CurrentState == newState) return;
        switch(newState)
        {
            case UIState.playing:
                // Handle transition to playing state
                break;

            case UIState.MainMenu:
                // Handle transition to main menu state
                break;

            case UIState.pause:
                // Handle transition to pause state
                break;

            case UIState.gameOver:
                // Handle transition to game over state
                break;
        }
        CurrentState = newState;
        // Handle state change logic here (e.g., show/hide UI panels)
    }
    private void OnDestroy()
    {
        if(_inet != null)
        {
            _inet._OnButtonClick -= OnUiButton;
        }
    }
    private void OnGameStateChanged(GameState newState)
    {
        switch(newState)
        {
            case GameState.playing:
                ChangeState(UIState.playing);
                break;

            case GameState.MainMenu:
                ChangeState(UIState.MainMenu);
                break;

            case GameState.paused:
                ChangeState(UIState.pause);
                break;

            case GameState.gameOver:
                ChangeState(UIState.gameOver);
                break;
        }
    }                                                                                                                                                                                     
}
public enum UIState
{
    Systems,
    MainMenu,
    playing,
    pause,
    gameOver
}
