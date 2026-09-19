using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [SerializeField] private ButtonType _type;
    [SerializeField] private Button _button;
    private EventBus _inet;
    
    private void OnEnable()
    {
        _inet = GameManager.Instance.Inet;
        _button.onClick.AddListener(OnClick);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }
    private void OnClick()
    {
        _inet.TrigerButtonClick(_type);
    }

}
