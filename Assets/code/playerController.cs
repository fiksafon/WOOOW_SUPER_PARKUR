using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class playerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AudioSource _footstepAudioSource;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private CameraController _cameraController;
    private float _currentSpeed = 0;
    private const string _moveParameter = "MoveSpeed";

    private const string _DanceParameter = "Dance";

    [SerializeField]private float _lookSpeed = 2f;
     
     private const string _groundTag = "Ground";
    private bool _isGrounded;

    private Vector3 _currentMoveVector;

    private float _targetLookY;

    private float _moveX = 0f;
    private float _moveZ = 0f;
 

    private void OnEnable()
    {
        PlayerInputManager._onMoveCallback += OnMovePressed;
        PlayerInputManager._onLookCallback += OnLookPressed;
        PlayerInputManager._onDance += onDancePressed;
        PlayerInputManager._onJump += onJumpPressed;
    }
    private void OnDisable()
    {
        PlayerInputManager._onMoveCallback -= OnMovePressed;
        PlayerInputManager._onLookCallback -= OnLookPressed;
        PlayerInputManager._onDance -= onDancePressed;
        PlayerInputManager._onJump -= onJumpPressed;
    }
    private void OnMovePressed(Vector2 moveInput)
    {
        _moveX = moveInput.x;
        _moveZ = moveInput.y;
        Debug.Log($"x - {_moveX}   z - {_moveZ}");

       /// _currentMoveVector = new Vector3(moveX, 1f , moveZ);
        
    }
    private void OnLookPressed(Vector2 lookInput)
    {
        
     _targetLookY = lookInput.x;

        
    }
    private void FixedUpdate()
    {
        Move();
        
    }
    private void Rotate()
    {
        transform.Rotate(Vector3.up * _targetLookY * _lookSpeed * Time.deltaTime);
    }
    private void Update()
    {
        UpdateAnimation();
        Rotate();
    }
    private void Move()
    {
        _currentMoveVector = transform.right * _moveX + transform.forward * _moveZ;
        if(_currentMoveVector.sqrMagnitude > 0.1f)
        {
            _currentSpeed = _walkSpeed;
            if(!_footstepAudioSource.isPlaying)
            {
                _footstepAudioSource.Play();   
            }
        }
        else
        {
            _currentSpeed = 0f;
            if(_footstepAudioSource.isPlaying)
            {
                _footstepAudioSource.Stop();
            }
        }
            
        
        Vector3 move = _currentMoveVector * _currentSpeed * Time.fixedDeltaTime;

        

        _rb.MovePosition(_rb.position + move);
    }
    private void UpdateAnimation()
    {
        _playerAnimator.SetFloat(_moveParameter, _currentSpeed);
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag(_groundTag))
        {
            _isGrounded = true;
        }
    }
    private void onJumpPressed()
    {
        if(!_isGrounded)
        {
            return;
        }
        _isGrounded = false;
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void onDancePressed()
    {
        _playerAnimator.SetTrigger(_DanceParameter);
    }
}

