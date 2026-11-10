using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   [SerializeField] private Animator _playerAnimator;

   private const string _moveX = "SpeedX";
   private const string _moveY = "SpeedY";
   private const string _State = "State";

   public void UpdateAnimation(float moveX, float moveY)
    {
        if(_playerAnimator == null)
        {
            return;
        }
        _playerAnimator.SetFloat(_moveX, moveX);
        _playerAnimator.SetFloat(_moveY, moveY);
    }
    public void SetState(float state)
    {
        if(_playerAnimator == null)
        {
            return;
        }
        _playerAnimator.SetFloat(_State, state);
    }
}
