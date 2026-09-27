using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   [SerializeField] private Animator _playerAnimator;

   private const string _moveParameter = "MoveSpeed";
   public void UpdateAnimation(float _currentSpeed)
    {
        if(_playerAnimator == null)
        {
            return;
        }
        _playerAnimator.SetFloat(_moveParameter, _currentSpeed);
        
    }
}
