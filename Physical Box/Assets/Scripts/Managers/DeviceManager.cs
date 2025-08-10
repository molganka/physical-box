using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    [SerializeField] private Device[] _devices;
    private int _currentDeviceIndex;
    private int _mouseScrollCountToChange;

    private void OnEnable()
    {
        InputManager.Instance.OnPlayerScrollDeviceInput += ProcessScrollDevice;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPlayerScrollDeviceInput -= ProcessScrollDevice;
    }

    private void Awake()
    {
        InitializeDevices();
    }

    private void Start()
    {
        _devices[0].Show();
    }

    private void InitializeDevices()
    {
        _devices = new Device[transform.childCount];

        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Device>(out Device device))
            {
                _devices[i] = device;
                device.Hide();
                i++;
            }
            else
            {
                Debug.LogError("child not device!");
            }
        }
    }

    private void ProcessScrollDevice(float scrollDirection)
    {
        Debug.Log(scrollDirection);
        if (scrollDirection > 0)
        {
            if (_mouseScrollCountToChange == 1)
            {
                ChangeToNextDevice();
                _mouseScrollCountToChange = 0;
            }
            else
            {
                _mouseScrollCountToChange = 1;
            }
        }
        else
        {
            if(_mouseScrollCountToChange == -1)
            {
                ChangeToPreviousDevice();
                _mouseScrollCountToChange = 0;
            }
            else
            {
                _mouseScrollCountToChange = -1;
            }
        }
    }

    private void ChangeToNextDevice()
    {
        _devices[_currentDeviceIndex]?.Hide();

        if(_currentDeviceIndex+1 < _devices.Length)
        {
            _devices[_currentDeviceIndex + 1].Show();
            _currentDeviceIndex++;
        }
        else
        {
            _devices[0].Show();
            _currentDeviceIndex = 0;
        }
    }

    private void ChangeToPreviousDevice()
    {
        _devices[_currentDeviceIndex]?.Hide();

        if (_currentDeviceIndex > 0)
        {
            _devices[_currentDeviceIndex - 1].Show();
            _currentDeviceIndex--;
        }
        else
        {
            _devices[_devices.Length - 1].Show();
            _currentDeviceIndex = _devices.Length-1;
        }
    }
}
