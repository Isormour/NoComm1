using System;
using _Project.Scripts.Patterns;
using Cinemachine;
using StarterAssets;
using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraManager : MonoBehaviourSingleton<CameraManager>
    {
        private const float _threshold = 0.01f;
        
        [SerializeField] private CinemachineVirtualCamera  virtualCamera;
        public GameObject CinemachineCameraTarget;
        
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        
        private CameraObject followingObject;
        private MoveInputReceiver inputReceiver;
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        
        public void SetTarget(CameraObject target)
        {
            virtualCamera.Follow = target.Target;
            followingObject = target;
            inputReceiver = target.InputReceiver;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }


        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (inputReceiver.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = true ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += inputReceiver.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += inputReceiver.look.y * deltaTimeMultiplier;
                Debug.Log(inputReceiver.look);
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            followingObject.Target.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

    }
}
