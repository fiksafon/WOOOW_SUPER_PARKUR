using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
 public static GameManager Instance{get; private set;}
 [SerializeField] private EventBus _inet;
 [SerializeField] private PlayerInputManager _inputmen;
 [SerializeField] private UIManager _uiManager;
 private GameState _gameState = GameState.Systems;
 public GameState GameState => _gameState; 

 public EventBus Inet => _inet;

 private void Awake()
 {
    if(Instance != null)
    {
        Destroy(this);
    }
    Instance = this;
    DontDestroyOnLoad(this);
    
    _inputmen.Init(_inet);
    _uiManager.Init(_inet);
    _inet._onGameStateRequest += OnStateRequest;
    OnStateRequest(GameState.MainMenu);
 }

 private void OnStateRequest(GameState state)
 {
    if(_gameState == state) return;

    switch(state)
    {
      case GameState.playing:
        LoadToScene(ScenList._gameSceneName);
        break;

      case GameState.MainMenu:
        LoadToScene(ScenList._mainSceneName);
        break;
      case GameState.paused:
      if(_gameState == GameState.gameOver||_gameState == GameState.MainMenu) return;
        break;
      case GameState.gameOver:
        break;
    }
    _inet.NotifyGameStateChange(state);
     _gameState = state;
 }


 private void LoadToScene(string sceneName)
 {
    SceneManager.LoadScene(sceneName);
 }


}

public enum GameState
{
  playing,
  paused,
  gameOver,
  MainMenu,
  Systems,

}
//привет constantin
