using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button _btn;
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private BtnType _Type;
    /// Animation Settings
    [SerializeField] private Animator _animator;
    private const string _openTrigger = "Open";
    private const string _closeTrigger = "Close";
    private void OnEneble()
    {
        _btn.onClick.AddListener(OnClicked);
        _textField.text = _Type.ToString();
    }
    private void OnDisable()
    {
        _btn.onClick.RemoveListener(OnClicked);
    }
    private void OnClicked()
    {
        if(_Type == BtnType.Open)
        {
            _animator.SetTrigger(_openTrigger);
        }
        else if(_Type == BtnType.close)
        {
            _animator.SetTrigger(_closeTrigger);
        }
    }
}
 public enum BtnType
{
    close,
    Open
}