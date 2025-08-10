using DG.Tweening;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float _smoothMoveValue;
    [SerializeField] private float _basicFOV;
    [SerializeField] private float _highFOV;

    private Vector3 _targetLocalPosition;

    private Camera _camera;
    private PlayerController _playerController;
    private Animator _animator;

    [Header("Dotween")]
    [SerializeField] private float _FOVDuration;
    [SerializeField] private Ease _FOVEase;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _playerController = GetComponentInParent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
        _targetLocalPosition = transform.localPosition;       
    }

    private void Update()
    {
        if (_playerController.IsMoving)
            _animator.SetBool("IsWalking", true);
        else
            _animator.SetBool("IsWalking", false);
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
        transform.GetChild(0).localRotation = rotation;
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
