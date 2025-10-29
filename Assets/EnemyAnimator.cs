using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    Vector3 lastPos;

    private void Start()
    {
        lastPos = transform.position;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, lastPos)>0.02f)
        {
            animator.SetBool("Run", true);
        }

        else
        {
            animator.SetBool("Run", false);
        }

        lastPos = transform.position;
    }
}
