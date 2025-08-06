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

    [Header("Camera")]
    [SerializeField] private float _FOVDuration;
    [SerializeField] private Ease _FOVEase;
    public void ChangeFOV(Camera camera, float FOV)
    {
        camera.DOFieldOfView(FOV, _FOVDuration).SetEase(_FOVEase);
    }
}
