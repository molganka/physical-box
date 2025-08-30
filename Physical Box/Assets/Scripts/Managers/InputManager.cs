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
    public event Action<int> OnPlayerDeviceChanged;
    public event Action<sbyte> OnDeviceMouseScrolled;
    public event Action OnDeviceLeftMouseInteraction;
    public event Action OnDeviceRightMouseInteraction;

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

        _inputSystem.Player.Run.performed += _ => PlayerIsRunInput = true;
        _inputSystem.Player.Run.canceled += _ => PlayerIsRunInput = false;

        _inputSystem.Player.Jump.performed += _ => PlayerIsJumpInput = true;
        _inputSystem.Player.Jump.canceled += _ => PlayerIsJumpInput = false;

        _inputSystem.Player.Crouch.performed += _ => OnPlayerCrouchInput?.Invoke();

        _inputSystem.Player.ChangeDeviceTo1.performed += _ => OnPlayerDeviceChanged?.Invoke(1);
        _inputSystem.Player.ChangeDeviceTo2.performed += _ => OnPlayerDeviceChanged?.Invoke(2);
        _inputSystem.Player.ChangeDeviceTo3.performed += _ => OnPlayerDeviceChanged?.Invoke(3);
        _inputSystem.Player.ChangeDeviceTo4.performed += _ => OnPlayerDeviceChanged?.Invoke(4);
        _inputSystem.Player.ChangeDeviceTo5.performed += _ => OnPlayerDeviceChanged?.Invoke(5);
        _inputSystem.Player.ChangeDeviceTo6.performed += _ => OnPlayerDeviceChanged?.Invoke(6);
        _inputSystem.Player.ChangeDeviceTo7.performed += _ => OnPlayerDeviceChanged?.Invoke(7);
        _inputSystem.Player.ChangeDeviceTo8.performed += _ => OnPlayerDeviceChanged?.Invoke(8);

        _inputSystem.Device.MouseScroll.performed += _ => OnDeviceMouseScrolled?.Invoke((sbyte)_.ReadValue<Vector2>().y);

        _inputSystem.Device.LeftMouseInteraction.performed += _ => OnDeviceLeftMouseInteraction?.Invoke(); 
        _inputSystem.Device.RightMouseInteraction.performed += _ => OnDeviceRightMouseInteraction?.Invoke(); 
    }
}

