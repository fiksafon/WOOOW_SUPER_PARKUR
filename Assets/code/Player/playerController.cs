using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class playerController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;    
    [SerializeField] private PlayerMoveControl _playerMoveControl;
    [SerializeField] private Transform _Legs;
    [SerializeField] private LayerMask _groundLayer;
    private float _Timer = 0f;
    private float _ChekDelay = 0.2f;
    private float _groundCheckDistance = 0.3f;
    private bool _isGrounded = false;

    private EventBus _inet;

    private void OnEnable()
    {
      _inet =  GameManager.Instance.Inet;
      _inet._onMoveCallback += _onMovePressed;
      _inet._onJump += OnJumpPressed;
       
    }
    private void OnDisable()
    {
      _inet._onMoveCallback -= _onMovePressed;
      _inet._onJump -= OnJumpPressed;
    }
    private void FixedUpdate()
    {
      _Timer += Time.fixedDeltaTime;
      if(_Timer >= _ChekDelay) 
      {
          CheckForGrounded();
          _Timer = 0f;
      }

    }

    private void OnLookPressed(Vector2 lookInput)
    {
           
    }
    private void _onMovePressed(Vector2 moveInput)
    {
        _playerMoveControl.OnMovePressed(moveInput);
    }
     private void OnJumpPressed()
    {
        _playerMoveControl.onJumpPressed();
    }
    private void CheckForGrounded()
    {
      if(Physics2D.Raycast(_Legs.position, Vector2.down, _groundCheckDistance, _groundLayer))
      {
       if(!_isGrounded)
       {
        _isGrounded = true;
       }
      }
      else
      {
        if(_isGrounded)
        {
          _isGrounded = false;
        }
      }
      NotifiyGrounded();
    }
    private void NotifiyGrounded()
    {
      _playerMoveControl.SetGrounded(_isGrounded);
    }
    
}
