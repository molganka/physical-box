using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    public void SpawnObject(GameObject obj, Vector3 pos, Quaternion rotation)
    {
        Instantiate(obj, pos, rotation);
    }
}
