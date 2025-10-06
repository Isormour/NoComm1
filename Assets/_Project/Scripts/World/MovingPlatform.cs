using System;
using UnityEngine;

namespace _Project.Scripts.World
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private Vector3 previousPosition;
        private Vector3 moveOffset = Vector3.zero;

        // private void Update()
        // {
        //     moveOffset = previousPosition - target.position;
        // }
        //
        // private void LateUpdate()
        // {
        //     previousPosition = transform.position;
        // }
        //
        // private void OnTriggerStay(Collider other)
        // {
        //     if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        //         return;
        //     other.transform.position = target.position + moveOffset;
        // }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;
            other.transform.parent = target;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;
            other.transform.parent = null;
        }
    }
}
