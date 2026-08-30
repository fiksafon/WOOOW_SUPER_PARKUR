using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public event Action<Vector2> _onMoveCallback;
    public event Action<Vector2> _onLookCallback;

    public  event Action _onJump;
    public event Action _onDance;

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
    
}