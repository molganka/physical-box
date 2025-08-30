using Unity.VisualScripting;
using UnityEngine;

public class SpawnerDevice : BaseDevice
{
    private Object _currentPreviewObject;

    private void OnEnable()
    {
        InputManager.Instance.OnDeviceLeftMouseInteraction += SpawnObject;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDeviceLeftMouseInteraction -= SpawnObject;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (_currentPreviewObject != null)
        {
            _currentPreviewObject.transform.position =
                PlayerController.Instance.transform.position +
                PlayerController.Instance.transform.forward +
                PlayerCameraController.Instance.transform.forward;
        }
    }

    private void LateUpdate()
    {
        if(_currentPreviewObject == null)
        {
            _currentPreviewObject = SpawnManager.Instance.SpawnObject(ItemsManager.Instance.CurrentObject, Vector3.zero, Quaternion.Euler(0, 0, 0));
        }
    }

    public override void Show()
    {
        base.Show();
        
    }

    private void SpawnObject()
    {
        Debug.Log("SpawnObject prevdo");
        _currentPreviewObject.ChangePhysicsCondition(true);
        _currentPreviewObject = null;
    }
}
