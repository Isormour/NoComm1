using System;
using System.Collections.Generic;
using _Project.Scripts.Camera;
using JetBrains.Annotations;
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
        [SerializeField] private float platformSpeed = 5;

        private MoveInputReceiver moveInputReceiver;
        private MoveInputReceiver previousInputReceiver;
        private CameraObject cameraObject;

        private float deltaDisolve = 0;
        private bool isActivePlatform = false;
        private bool hasControll = false;

        private List<Vector3> movingPoints = new List<Vector3>();
        private bool isOverControl = false;
        private int currentId = 0;
        private float delta;
        

        private void Awake()
        {
            moveInputReceiver = GetComponent<MoveInputReceiver>();
            cameraObject = GetComponent<CameraObject>();
        }

        private void Update()
        {
            UpdateMaterialState();
            if (isOverControl)
            {
                UpdateInput();
            }
            else
            {
                UpdateMoving();
            }
        }

        public void TakeControl(MoveInputReceiver previousInputReceiver)
        {
            if (!isActivePlatform)
                return;
            MoveInputEvents.Instance.SetMoveInputReceiver(moveInputReceiver);
            CameraManager.Instance.SetTarget(cameraObject);
            this.previousInputReceiver = previousInputReceiver;
            previousInputReceiver.ResetAllStates();
            movingPoints.Clear();
            isOverControl = true;
            currentId = 0;
            movingPoints.Add(transform.position);
        }

        public void GiveUpControl()
        {
            MoveInputEvents.Instance.SetMoveInputReceiver(previousInputReceiver);
            CameraManager.Instance.SetTarget(previousInputReceiver.GetComponent<CameraObject>());
            previousInputReceiver = null;
            moveInputReceiver.ResetAllStates();
            isOverControl = false;
            movingPoints.Add(transform.position);
        }

        private void UpdateInput()
        {
            float x, y, z;
            x = GetOutput(moveInputReceiver.move.x < -0.1, moveInputReceiver.move.x > 0.1);
            y = GetOutput(moveInputReceiver.sprint, moveInputReceiver.jump);
            z = GetOutput(moveInputReceiver.move.y < -0.1, moveInputReceiver.move.y > 0.1);

            if (moveInputReceiver.isPressedInterract)
            {
                GiveUpControl();
                return;
            }
            
            transform.position += new Vector3(x, y, z) * (platformSpeed * Time.deltaTime);
            delta += Time.deltaTime;
            if (delta > 0.1f)
            {
                movingPoints.Add(transform.position);
                delta = 0;
            }
        }

        private float GetOutput(bool negative, bool positive)
        {
            if (negative == positive)
                return 0;
            if (negative)
                return -1;

            return 1;
        }

        private void UpdateMoving()
        {
            if (movingPoints.Count == 0)
                return;
            if (currentId >= movingPoints.Count)
                currentId = 0;

            transform.position = Vector3.MoveTowards(transform.position, movingPoints[currentId],
                platformSpeed * Time.deltaTime);
            
            var distance = Vector3.Distance(transform.position, movingPoints[currentId]);
            if (distance < 0.1f)
                currentId++;
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
