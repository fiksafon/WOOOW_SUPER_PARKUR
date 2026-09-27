using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
   [SerializeField] private float _walkSpeed = 5f;
   [SerializeField] private Rigidbody2D _rb;
   [SerializeField] private float jumpForce = 5f;
   [SerializeField] private PlayerAnimator _playerAnimator;
    private Vector2 _currentMoveVector;
    private float _currentSpeed = 5f;
    private bool _isGrounded = false;


    public void OnMovePressed(Vector2 moveInput)
    {
        float _moveX = moveInput.x;
        _currentMoveVector = transform.right * _moveX;
        
    }

    private void FixedUpdate()
    {
        Move();
        
    }

    public void Move()
    {

        if(_currentMoveVector.sqrMagnitude > 0.1f)
        {
            _currentSpeed = _walkSpeed;
           
        }
        else
        {
            _currentSpeed = 0f;
        }
            
        
        Vector2 move = _currentMoveVector * _currentSpeed * Time.fixedDeltaTime;

        _playerAnimator.UpdateAnimation(_currentSpeed);
        // _rb.MovePosition(_rb.position + move);
        _rb.linearVelocity = new Vector2(move.x, _rb.linearVelocity.y);
    }

     public void onJumpPressed()
    {
        Debug.Log("Jump pressed");
        if(!_isGrounded)
        {
            return;
        }
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    public void SetGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
        Debug.Log("Grounded: " + _isGrounded);
    }
}
