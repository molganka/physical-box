using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float _smoothMoveValue;
    [SerializeField] private float _basicFOV;
    [SerializeField] private float _highFOV;

    private Vector3 _targetLocalPosition;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetLocalPosition, _smoothMoveValue * Time.deltaTime);
    }

    public void ChangeSmoothYPosition(float yPos)
    {
        _targetLocalPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
    }

    public void ChangeRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
    }

    public void SetHighFOV()
    {
        DotweenManager.Instance.ChangeFOV(_camera, _highFOV);
    }

    public void SetBasicFOV()
    {
        DotweenManager.Instance.ChangeFOV(_camera, _basicFOV);
    }
}
