using UnityEngine;

public class SpawnerDevice : BaseDevice
{
    private void OnEnable()
    {
        InputManager.Instance.OnDeviceLeftMouseInteraction += Spawn;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDeviceLeftMouseInteraction -= Spawn;
    }

    private void Spawn()
    {
        Debug.Log(SpawnManager.Instance);
        SpawnManager.Instance.SpawnObject(
            ItemsManager.Instance.CurrentObject,
            PlayerController.Instance.transform.position + 
            PlayerController.Instance.transform.forward + 
            PlayerCameraController.Instance.CameraTransform.forward,
            ItemsManager.Instance.CurrentObject.transform.rotation);
    }
}
