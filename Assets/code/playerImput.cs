using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class  PlayerInputManager: MonoBehaviour
{
    private EventBus _inet;
    public void Init(EventBus bus)
    {
      _inet = bus;
    }
    public void onMovePrtessed(CallbackContext context)
    {
        if(context.performed)
        {
            _inet.TrigerMove(context.ReadValue<Vector2>());
        }
        else
        {
            var _zero = new Vector2(0f, 0f);
            _inet.TrigerMove(_zero);
        }
    }
    public void OnLook(CallbackContext context)
    {   Vector2 lookInput = context.ReadValue<Vector2>();
        if(lookInput.sqrMagnitude > 3)
        {
            _inet.TrigerLook(lookInput);
        }
        else
        {
            _inet.TrigerLook(Vector2.zero);
        }
    }
    public void OnJump(CallbackContext ctx)
    {
        if(ctx.performed)
        {
            _inet.TrigerJump();
        }
    }
    public void OnDance(CallbackContext ctx)
    {
        if(ctx.performed)
        {
            _inet.TrigerDance();
        }
    } 
    
}
