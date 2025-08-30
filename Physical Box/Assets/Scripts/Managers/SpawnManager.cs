using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    public Object SpawnObject(Object obj, Vector3 pos, Quaternion rotation)
    {
        return Instantiate(obj.gameObject, pos, rotation).GetComponent<Object>();
    }
}
