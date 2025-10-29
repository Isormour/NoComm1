using UnityEngine;
using UnityEngine.Events;

public class OnPlayerTriggwerAction : MonoBehaviour
{
    public UnityEvent ELOOO;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ELOOO.Invoke();
        }
    }
}
