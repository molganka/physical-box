using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    [Header("Speed")]
    [SerializeField] private float _basicSpeedMove;
    [SerializeField] private float _sprintSpeedMove;

    [Header("Rotation")]
    [SerializeField] private float _horizontalMultiplier;
    [SerializeField] private float _verticalMultiplier;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _verticalRotationRange = 80f;
    
    [Header("Move")]
    [SerializeField] private float _moveSmoothValue;

    [Header("Jump")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;

    private float _currentSpeedMove;

    private float _horizontalRotation;
    private float _verticalRotation;
    private float _velocity;
    private Vector3 _smoothMove;
    private Vector3 _currentMovement;

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
        GravityAndJump();
    }

    private void HandleMovement()
    {
        ControlSpeed();       
        Vector2 inputVector = InputManager.Instance.PlayerMoveInput;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = transform.rotation * moveDirection * _currentSpeedMove;

        _smoothMove = Vector3.Lerp(_smoothMove, moveDirection, _moveSmoothValue * Time.deltaTime);

        _currentMovement = new Vector3(_smoothMove.x, _velocity, _smoothMove.z);

        _characterController.Move(_currentMovement * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector2 rotateInput = InputManager.Instance.PlayerLookInput;

        float horizontalRotation = rotateInput.x * _horizontalMultiplier * _sensitivity;
        float verticalRotation = rotateInput.y * _verticalMultiplier * _sensitivity;

        //обновляем углы
        _horizontalRotation += horizontalRotation;
        _verticalRotation -= verticalRotation;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalRotationRange, _verticalRotationRange);

        // Применяем вращение 
        transform.localRotation = Quaternion.Euler(0, _horizontalRotation, 0);
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    private void ControlSpeed()
    {
        if (InputManager.Instance.PlayerIsSprintInput)
        {
            _currentSpeedMove = _sprintSpeedMove;
        }
        else
        {
            _currentSpeedMove = _basicSpeedMove;
        }
    }

    private void GravityAndJump()
    {
        _velocity += _gravity * Time.deltaTime;

        if (InputManager.Instance.PlayerIsJumpInput && _characterController.isGrounded)
        {
            _velocity = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
        }
    }
}
