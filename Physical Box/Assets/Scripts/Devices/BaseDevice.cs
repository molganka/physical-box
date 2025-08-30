using UnityEngine;

public abstract class BaseDevice : MonoBehaviour
{
    [SerializeField] private float _swayAmount = 0.2f;
    [SerializeField] private float _swayAmountRange = 0.5f;
    [SerializeField] private float _smoothAmount = 10f;
    private float _swayMultiplier = 0.01f;

    private MeshRenderer _meshRenderer => GetComponentInChildren<MeshRenderer>();
    private Vector3 _initialPosition;

    public virtual void Awake()
    {
        _initialPosition = transform.localPosition;
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        Swaying();
    }

    private void Swaying()
    {
        Vector2 rotateInput = InputManager.Instance.PlayerLookInput * _swayAmount * _swayMultiplier;

        rotateInput.x = Mathf.Clamp(rotateInput.x, -_swayAmountRange, _swayAmountRange);
        rotateInput.y = Mathf.Clamp(rotateInput.y, -_swayAmountRange, _swayAmountRange);
        rotateInput *= -1; //invert

        Vector3 targetPosition = new Vector3(rotateInput.x, rotateInput.y, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition + targetPosition,
            _smoothAmount * Time.deltaTime);
    }

    public virtual void Show()
    {
        _meshRenderer.enabled = true;
    }

    public virtual void Hide()
    {
        _meshRenderer.enabled = false;
    }
}
