using UnityEngine;

public class ParentSetter : MonoBehaviour
{
    [SerializeField] Vector3 localpos;
    [SerializeField] Vector3 localrot;
    [SerializeField] Transform parent;
    void Start()
    {
        this.transform.SetParent(parent);
        this.transform.localPosition = localpos;
        this.transform.localRotation = Quaternion.Euler(localrot);
    }
}
