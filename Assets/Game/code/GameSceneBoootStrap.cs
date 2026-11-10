using UnityEngine;

public class GameSceneBoootStrap : MonoBehaviour
{
    [SerializeField] private playerController _playerController;
    [SerializeField] private PlayerCameraController _playerCameraController;
    private EventBus _eventBus;

    private void Awake()
    {
        _eventBus = GameManager.Instance.Inet;
        _playerCameraController.Initialize(_eventBus);
        _playerController.Initialize(_playerCameraController, _eventBus);
    }
}
