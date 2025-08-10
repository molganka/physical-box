using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float _basicSpeedMove;
    [SerializeField] private float _runSpeedMove;
    [SerializeField] private float _crouchSpeedMove;
    [SerializeField] private float _speedMoveSmoothValue;

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
    private float _goalSpeedMove;

    private float _horizontalRotation;
    private float _verticalRotation;
    private float _velocity;
    private Vector3 _smoothMove;
    private Vector3 _currentMovement;

    private bool _isMoving;
    private bool _isRunning;
    private bool _isCrouching;
    private bool _isOnGround;

    private CharacterController _characterController;
    private PlayerCameraController _camera;

    public bool IsMoving { get { return _isMoving; } }
    public bool IsRunning { get { return _isRunning; } }
    public bool IsCrouching { get { return _isCrouching; } }
    public bool IsOnGround { get { return _isOnGround; } }

    private void OnEnable()
    {
        InputManager.Instance.OnPlayerCrouchInput += ChangeBodyPosition;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPlayerCrouchInput -= ChangeBodyPosition;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<PlayerCameraController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetCrouchPosition(false);
    }

    private void Update()
    {
        UpdateSprintState();
        UpdateGroundState();
        HandleRotation();
        HandleMovement();
        Gravity();
        CheckJump();
        ControlSpeed();
        ControlFOV();
    }

    private void HandleMovement()
    {
        ControlSpeed();
        Vector2 inputVector = InputManager.Instance.PlayerMoveInput;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = transform.rotation * moveDirection * _currentSpeedMove;
        

        _smoothMove = Vector3.Lerp(_smoothMove, moveDirection, _moveSmoothValue * Time.deltaTime);
        _currentMovement = new Vector3(_smoothMove.x, _velocity, _smoothMove.z);
        CollisionFlags flags = _characterController.Move(_currentMovement * Time.deltaTime);

        //если сверху было касание то нужно начать падать
        if ((flags & CollisionFlags.Above) > 0 && _velocity > 0)
        {
            _velocity = 0;
        }

        //записываем было ли движение (ось Y не считается)
        if (moveDirection != Vector3.zero)
            _isMoving = true;
        else
            _isMoving = false;
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
        _camera.ChangeRotation(Quaternion.Euler(_verticalRotation, 0f, 0f));
    }

    private void ControlSpeed()
    {
        if (_isCrouching)
        {
            _goalSpeedMove = _crouchSpeedMove;
        }
        else if (_isRunning)
        {
            _goalSpeedMove = _runSpeedMove;
        }
        else
        {
            _goalSpeedMove = _basicSpeedMove;
        }

        _currentSpeedMove = Mathf.Lerp(_currentSpeedMove, _goalSpeedMove, _speedMoveSmoothValue * Time.deltaTime);
    }

    private void ControlFOV()
    {
        //если мы движемся и бежим то увеличивать fov
        if (_isMoving && _isRunning)
            _camera.SetHighFOV();
        else
            _camera.SetBasicFOV();
    }

    private void UpdateSprintState()
    {
        if (InputManager.Instance.PlayerIsRunInput && !_isCrouching)
            _isRunning = true;
        else
            _isRunning = false;
    }

    private void UpdateGroundState()
    {
        _isOnGround = _characterController.isGrounded; 
    }

    private void Gravity()
    {
        if (_isOnGround)
            _velocity = _gravity * 0.01f;
        else
            _velocity += _gravity * Time.deltaTime;
    }

    private void CheckJump()
    {
        if (InputManager.Instance.PlayerIsJumpInput && !_isCrouching && _isOnGround)
        {
            _velocity = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
        }
    }

    [Header("Body Position")]
    [SerializeField] private float _standBodyHeight;
    [SerializeField] private float _crouchBodyHeight;
    [SerializeField] private float _crouchBodyCenterY;
    [SerializeField] private float _standCameraPositionY;
    [SerializeField] private float _crouchCameraPositionY;
    private void ChangeBodyPosition()
    {
        SetCrouchPosition(!_isCrouching);
    }

    private void SetCrouchPosition(bool onCrouch)
    {
        if (!_isOnGround) return;

        if (onCrouch)
        {
            _characterController.height = _crouchBodyHeight;
            _characterController.center = new Vector3(0, _crouchBodyCenterY, 0);
            _camera.ChangeSmoothYPosition(_crouchCameraPositionY);
            _isCrouching = true;
        }
        else
        {
            if (!CheckStandUp())
            {
                _characterController.height = _standBodyHeight;
                _characterController.center = Vector3.zero;
                _camera.ChangeSmoothYPosition(_standCameraPositionY);
                _isCrouching = false;
            }
        }
    }

    private bool CheckStandUp()
    {
        return Physics.Raycast(transform.position, Vector3.up, _standBodyHeight / 2);
    }
}
