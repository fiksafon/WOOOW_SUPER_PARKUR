using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public event Action<Vector2> _onMoveCallback;
    public event Action<Vector2> _onLookCallback;

    public  event Action _onJump;
    public event Action _onDance;
    public event Action _onAtack;
    public event Action<bool> _onRun;
    public event Action<ButtonType> _OnButtonClick;

    public void TrigerMove(Vector2 kostya)
    {
        _onMoveCallback?.Invoke(kostya);
    }
    public void TrigerLook(Vector2 Lyosha)
    {
        _onLookCallback?.Invoke(Lyosha);
    }
     public void TrigerJump()
    {
        _onJump?.Invoke();
    }
     public void TrigerDance()
    {
        _onDance?.Invoke();
    }
    public void TrigerAtack()
    {
        _onAtack?.Invoke();
    }
    public void TrigerRun(bool isRun)
    {
        _onRun?.Invoke(isRun);
    }
    public void TrigerButtonClick(ButtonType button)
    {
        _OnButtonClick?.Invoke(button);
    }
    
}