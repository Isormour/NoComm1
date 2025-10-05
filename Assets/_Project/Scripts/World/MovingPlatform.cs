using System;
using UnityEngine;

namespace _Project.Scripts.World
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                other.transform.SetParent(target);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.transform.SetParent(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                other.transform.SetParent(null);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.transform.SetParent(null);
            }
        }
    }
}
