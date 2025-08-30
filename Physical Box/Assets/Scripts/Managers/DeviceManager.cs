using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    [SerializeField] private BaseDevice[] _devices;
    private int _currentDeviceIndex;

    private void OnEnable()
    {
        InputManager.Instance.OnPlayerDeviceChanged += ProcessChangeDevice;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPlayerDeviceChanged -= ProcessChangeDevice;
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
        _devices = new BaseDevice[transform.childCount];

        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<BaseDevice>(out BaseDevice device))
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

    private void ProcessChangeDevice(int input)
    {
        Debug.Log(input);
        --input;
        if(input >= 0 && input < _devices.Length)
        {
            _devices[_currentDeviceIndex]?.Hide();
            _devices[input].Show();
        }
    }


/*
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
    }*/
}
