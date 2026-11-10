using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using Sequences = DG.Tweening.Sequence;

public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ButtonType _type;
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _scaleFactor = 1.2f;
    [SerializeField] private float _scaleDuration = 0.1f;
    [SerializeField] private float _clickDuration = 3f;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _clickSprite;
    [SerializeField] private Sprite _normalSprite;

    private EventBus _inet;
    private Tween _scaleTween;
    private Sequence _clickSequence;

    
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
        PlayClick();
    }
    private void PlayClick()
    {
        _clickSequence?.Kill();
        _button.interactable = false;

        _clickSequence = DOTween.Sequence()
                        .SetUpdate(true)
                        .SetLink(gameObject)
                        .Append(_rectTransform.DOScale(0f, _clickDuration))
                        // .OnStart(() => {Debug.Log("Fix error");})
                        // .Append(_rectTransform.DORotate(new Vector3(0, 0, -360f), _clickDuration, RotateMode.FastBeyond360).SetEase(Ease.InSine))
                        // .Join(_image.DOFade(0f, _clickDuration))
                        .OnComplete(() => {SendClickEvent();});
                        
                          
                    
                      
    }
    private void SendClickEvent()
    {
        _inet.TrigerButtonClick(_type);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        _scaleTween?.Kill();
        SwapSprite(true);
        _scaleTween = _rectTransform.DOScale(_scaleFactor, _scaleDuration)
                                 .SetEase(Ease.OutQuad)
                                 .SetUpdate(true)
                                 .SetLink(gameObject);
                                 
        // _rectTransform.DOScale(_scaleFactor, _scaleDuration);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
          _scaleTween?.Kill();
          SwapSprite(false);
          _scaleTween = _rectTransform.DOScale(1f, _scaleDuration)
                                 .SetEase(Ease.InQuad)
                                 .SetUpdate(true)
                                 .SetLink(gameObject);
        // _rectTransform.DOScale(1f, _scaleDuration);
    }
    private void SwapSprite(bool isClicked)
    {
        if (isClicked)
        {
            _image.sprite = _clickSprite;
        }
        else
        {
            _image.sprite = _normalSprite;
        }
    }

}


