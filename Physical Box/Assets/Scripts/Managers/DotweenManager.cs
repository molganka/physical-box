using DG.Tweening;
using UnityEngine;

public class DotweenManager : MonoBehaviour
{
    public static DotweenManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
   
    public void ChangeFOV(Camera camera, float FOV, float duration, Ease ease)
    {
        camera.DOFieldOfView(FOV, duration).SetEase(ease);
    }
}
