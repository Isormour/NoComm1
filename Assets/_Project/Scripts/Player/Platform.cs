using System;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider collider;

        private float deltaDisolve = 0;
        private bool isActivePlatform = false;

        private void Update()
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
