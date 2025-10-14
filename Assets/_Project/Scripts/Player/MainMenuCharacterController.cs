using UnityEngine;

public class MainMenuCharacterController : MonoBehaviour
{
    private Animator animator;
    private int _animIDSpeed;
    void Awake()
    {
        animator = GetComponent<Animator>();

        _animIDSpeed = Animator.StringToHash("Speed");

        animator.SetFloat(_animIDSpeed, 0);

        animator.Play("Idle Walk Run Blend");
    }
}
