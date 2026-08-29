using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class  PlayerInputManager: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static event Action<Vector2> _onMoveCallback;
    public static event Action<Vector2> _onLookCallback;

    public static event Action _onJump;
    public static event Action _onDance;

    public void onMovePrtessed(CallbackContext context)
    {
        if(context.performed)
        {
            _onMoveCallback?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            var _zero = new Vector2(0f, 0f);
            _onMoveCallback?.Invoke(_zero);
        }
    }
    public void OnLook(CallbackContext context)
    {   Vector2 lookInput = context.ReadValue<Vector2>();
        if(lookInput.sqrMagnitude > 3)
        {
            _onLookCallback?.Invoke(lookInput);
        }
        else
        {
            _onLookCallback?.Invoke(Vector2.zero);
        }
    }
    public void OnJump(CallbackContext ctx)
    {
        if(ctx.performed)
        {
            _onJump?.Invoke();
        }
    }
    public void OnDance(CallbackContext ctx)
    {
        if(ctx.performed)
        {
            _onDance?.Invoke();
        }
    } 
    
}
