using System.Collections.Generic;
using UnityEngine;

public class InsideSphere : MonoBehaviour
{
    public List<GameObject> objectsInside = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject))
            objectsInside.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInside.Contains(other.gameObject))
            objectsInside.Remove(other.gameObject);
    }
}