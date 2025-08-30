using DG.Tweening;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float _smoothMoveValue;
    [SerializeField] private float _basicFOV;
    [SerializeField] private float _highFOV;

    [Header("Dotween")]
    [SerializeField] private float _FOVDuration;
    [SerializeField] private Ease _FOVEase;

    public static PlayerCameraController Instance;

    private Vector3 _targetLocalPosition;

    private Camera _camera;

    public Transform CameraTransform { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        _camera = GetComponentInChildren<Camera>();
        _targetLocalPosition = transform.localPosition;
        CameraTransform = transform.GetChild(0);
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
        CameraTransform.localRotation = rotation;
    }

    public void SetHighFOV()
    {
        DotweenManager.Instance.ChangeFOV(_camera, _highFOV, _FOVDuration, _FOVEase);
    }

    public void SetBasicFOV()
    {
        DotweenManager.Instance.ChangeFOV(_camera, _basicFOV, _FOVDuration, _FOVEase);
    }
}
