using _Project.Scripts.Patterns;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviourSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera  virtualCamera;

    public void SetTarget(Transform target)
    {
        virtualCamera.Follow = target;
    }
}
