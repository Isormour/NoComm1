using System;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] MeshRenderer meshRenderer;


        public void SetSelected(bool selected)
        {
            meshRenderer.material = selected ? selectedMaterial : defaultMaterial;
        }
    }
}
