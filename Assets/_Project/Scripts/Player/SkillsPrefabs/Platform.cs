using System;
using _Project.Scripts.Camera;
using StarterAssets;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class Platform : MonoBehaviour
    {
        public CameraObject CameraObject => cameraObject;
        
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider collider;

        private MoveInputReceiver moveInputReceiver;
        private MoveInputReceiver previousInputReceiver;
        private CameraObject cameraObject;

        private float deltaDisolve = 0;
        private bool isActivePlatform = false;

        private void Awake()
        {
            moveInputReceiver = GetComponent<MoveInputReceiver>();
            cameraObject = GetComponent<CameraObject>();
        }

        private void Update()
        {
            UpdateMaterialState();
            UpdateInput();
        }

        public void TakeControl(MoveInputReceiver previousInputReceiver)
        {
            if (!isActivePlatform)
                return;
            MoveInputEvents.Instance.SetMoveInputReceiver(moveInputReceiver);
            CameraManager.Instance.SetTarget(cameraObject);
            this.previousInputReceiver = previousInputReceiver;
            previousInputReceiver.ResetAllStates();
            
        }

        public void GiveUpControl()
        {
            MoveInputEvents.Instance.SetMoveInputReceiver(previousInputReceiver);
            CameraManager.Instance.SetTarget(previousInputReceiver.GetComponent<CameraObject>());
            previousInputReceiver = null;
            moveInputReceiver.ResetAllStates();
        }

        private void UpdateInput()
        {
            
        }

        private void UpdateMaterialState()
        {
            if (deltaDisolve > 1)
            {
                collider.enabled = true;
                return;
            }

            if (isActivePlatform)
            {
                deltaDisolve += Time.deltaTime;
            }
            meshRenderer.material.SetFloat("_Delta", deltaDisolve);
        }


        public void SetSelected(bool selected)
        {
            meshRenderer.material = selected ? selectedMaterial : defaultMaterial;
        }

        public void InitPLatform()
        {
            isActivePlatform = true;
        }
    }
}
