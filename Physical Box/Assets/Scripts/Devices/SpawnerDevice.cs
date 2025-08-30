using Unity.VisualScripting;
using UnityEngine;

public class SpawnerDevice : BaseDevice
{
    [SerializeField] private float _minObjectDistance;
    [SerializeField] private float _maxObjectDistance;
    [SerializeField] private float _scrollDistanceAmount;
    [SerializeField] private float _changingDistanceSpeed;
    private float _currentObjectDistance;
    private float _goalObjectDistance;
    private Object _currentPreviewObject;

    public override void Awake()
    {
        base.Awake();

        _currentObjectDistance = _minObjectDistance;
        _goalObjectDistance = _minObjectDistance;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnDeviceLeftMouseInteraction += SpawnObject;
        InputManager.Instance.OnDeviceMouseScrolled += ChangeObjectDistance;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDeviceLeftMouseInteraction -= SpawnObject;
        InputManager.Instance.OnDeviceMouseScrolled -= ChangeObjectDistance;
    }

    public override void Update()
    {
        base.Update();

        if (_currentPreviewObject != null)
        {
            _currentPreviewObject.transform.position = GetForwardPosition(_currentObjectDistance);
        }

        SmoothChangingDistance();
    }

    private void LateUpdate()
    {
        if(_currentPreviewObject == null)
        {
            _currentPreviewObject = SpawnManager.Instance.SpawnObject(ItemsManager.Instance.CurrentObject, GetForwardPosition(_goalObjectDistance), Quaternion.Euler(0, 0, 0));
        }
    }

    private void SpawnObject()
    {
        Debug.Log("SpawnObject prevdo");
        _currentPreviewObject?.ChangePhysicsCondition(true);
        _currentPreviewObject = null;
    }

    private void ChangeObjectDistance(sbyte scroll)
    {
        Debug.Log(scroll);
        if(scroll > 0)
        {
            _goalObjectDistance += _scrollDistanceAmount;
        }
        else
        {
            _goalObjectDistance -= _scrollDistanceAmount;
        }

        _goalObjectDistance = Mathf.Clamp(_goalObjectDistance, _minObjectDistance, _maxObjectDistance);
    }

    private void SmoothChangingDistance()
    {
        _currentObjectDistance = Mathf.Lerp(_currentObjectDistance, _goalObjectDistance, _changingDistanceSpeed * Time.deltaTime);
    }
}
