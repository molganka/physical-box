using UnityEngine;

public class Object : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        ChangePhysicsCondition(false);
    }

    public void ChangePhysicsCondition(bool enable)
    {
        if (enable)
        {
            _collider.enabled = true;
            _rigidbody.useGravity = true;
        }
        else
        {
            _collider.enabled = false;
            _rigidbody.useGravity = false;  
        }
    }
}
