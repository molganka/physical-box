using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _basicSpeedMove;
    [SerializeField] private float _sprintSpeedMove;
    [SerializeField] private float _horizontalMultiplier;
    [SerializeField] private float _verticalMultiplier;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _verticalRotationRange = 80f;
    [SerializeField] private float _moveSmooth;

    private float _currentSpeedMove;
    private float _horizontalRotation;
    private float _verticalRotation;

    private Vector3 _currentMoveDirection;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
        ControlSpeed();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = InputManager.Instance.PlayerMoveInput;
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = Vector3.Lerp(_currentMoveDirection, moveDirection, _moveSmooth * Time.deltaTime);
        _currentMoveDirection = moveDirection;
        transform.Translate(moveDirection * _currentSpeedMove * Time.deltaTime);
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
}
