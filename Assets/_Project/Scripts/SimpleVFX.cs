using UnityEngine;

public class SimpleVFX : MonoBehaviour
{
    [SerializeField] AudioSource AS;
    [SerializeField] ParticleSystem PS;
    [SerializeField] float DestroyTime = -1;
    public void Play(int particles = 20)
    {
        PS.Emit(particles);
        AS.pitch = Random.Range(0.7f, 1.0f);
        AS.Play();
        if (DestroyTime > 0) Destroy(this.gameObject, 1);
    }
}