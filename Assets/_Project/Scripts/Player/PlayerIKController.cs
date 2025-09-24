using UnityEngine;

public class PlayerIKController : MonoBehaviour
{
    [SerializeField] Transform RHandHitIKPosition;
    [SerializeField] Transform LHandHitIKPosition;
    [SerializeField] Animator anim;
    [SerializeField] AnimationCurve weightCurve;
    float RWeight = 1.0f;
    float LWeight = 1.0f;

    private void Start()
    {

    }
    public void SetIKWeight(AvatarIKGoal goal, float weight)
    {
        if (goal == AvatarIKGoal.RightHand) RWeight = weight;
        if (goal == AvatarIKGoal.LeftHand) LWeight = weight;
    }
    private void FixedUpdate()
    {
        RWeight -= Time.deltaTime;
        LWeight -= Time.deltaTime;
    }
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPosition(AvatarIKGoal.RightHand, RHandHitIKPosition.position);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, LHandHitIKPosition.position);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weightCurve.Evaluate(RWeight));
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightCurve.Evaluate(LWeight));

    }
}
