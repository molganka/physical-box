using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private GameInput _inputSystem;

    public Vector2 PlayerMoveInput { get; private set; }
    public Vector2 PlayerLookInput { get; private set; }
    public bool PlayerIsRunInput { get; private set; }
    public bool PlayerIsJumpInput { get; private set; }

    public event Action OnPlayerCrouchInput;
    public event Action<float> OnPlayerScrollDeviceInput;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _inputSystem = new GameInput();

        SubscribeBindings();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Disable();
    }

    private void SubscribeBindings()
    {
        _inputSystem.Player.Move.performed += ctx => PlayerMoveInput = ctx.ReadValue<Vector2>();
        _inputSystem.Player.Move.canceled += _ => PlayerMoveInput = Vector2.zero;

        _inputSystem.Player.Look.performed += ctx => PlayerLookInput = ctx.ReadValue<Vector2>();
        _inputSystem.Player.Look.canceled += _ => PlayerLookInput = Vector2.zero;

        _inputSystem.Player.ScrollDevice.performed += ctx => OnPlayerScrollDeviceInput?.Invoke(ctx.ReadValue<Vector2>().y);

        _inputSystem.Player.Run.performed += _ => PlayerIsRunInput = true;
        _inputSystem.Player.Run.canceled += _ => PlayerIsRunInput = false;

        _inputSystem.Player.Jump.performed += _ => PlayerIsJumpInput = true;
        _inputSystem.Player.Jump.canceled += _ => PlayerIsJumpInput = false;

        _inputSystem.Player.Crouch.performed += _ => OnPlayerCrouchInput?.Invoke();
    }
}

