using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class playerController : MonoBehaviour
{ 
    [SerializeField] private PlayerMoveControl _playerMoveControl;
    [SerializeField] private Transform _Legs;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private CheckColider _checkColider;
    private float _Timer = 0f;
    private float _ChekDelay = 0.2f;
    private float _groundCheckDistance = 0.3f;
    private bool _isGrounded = false;
    private PlayerCameraController _playerCameraController;
    private EventBus _inet;


    public void Initialize(PlayerCameraController cameraController, EventBus bus)
    {
        _playerCameraController = cameraController;
      _inet =  bus;
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
    private void Update()
    {
      if(_playerCameraController == null)
      {
        return;
      }
        _playerCameraController.GetPlayerPosition(transform.position);

        if (_checkColider.HasInteractable)
        {
            GetInteractable(_checkColider.CurrentInteractable);
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
      bool Hit = Physics2D.Raycast(_Legs.position, Vector2.down, _groundCheckDistance, _groundLayer);
      if(Hit != _isGrounded)
      {
        _isGrounded = Hit;
      }
     
      NotifiyGrounded();
    }
    private void NotifiyGrounded()
    {
      _playerMoveControl.SetGrounded(_isGrounded);
    }
    private void GetInteractable(IInteraktbl interactable)
    {
       interactable.Interact(gameObject);
    }
    
}

















//Kostya