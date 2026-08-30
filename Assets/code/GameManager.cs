using UnityEngine;

public class GameManager : MonoBehaviour
{
 public static GameManager Instance{get; private set;}
 [SerializeField] private EventBus _inet;
 [SerializeField] private PlayerInputManager _inputmen;

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
 }

}
