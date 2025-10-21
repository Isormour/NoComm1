using UnityEngine;

public class ThortorPiotrulo : MonoBehaviour
{
    public Rigidbody body;
    public Transform TRASNS;

    void Update()
    {
        Debug.Log(body.linearVelocity.y);
        Debug.Log(TRASNS.position.y,transform.gameObject);

    }
}
