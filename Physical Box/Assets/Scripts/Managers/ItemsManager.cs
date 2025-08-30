using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance;

    [SerializeField] private Object _cube;
    public Object CurrentObject { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        CurrentObject = _cube;
    }
}
