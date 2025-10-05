using StarterAssets;
using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraObject : MonoBehaviour
    {
        [field: SerializeField] public Transform Target { get; private set; }
        [field: SerializeField] public MoveInputReceiver InputReceiver { get; private set; }
    }
}
