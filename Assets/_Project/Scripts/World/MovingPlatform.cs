using System;
using UnityEngine;

namespace _Project.Scripts.World
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform target;

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
