using UnityEngine;

public class PlayerIKController : MonoBehaviour
{
    [SerializeField] Transform RHandHitIKPosition;
    [SerializeField] Transform LHandHitIKPosition;
    [SerializeField] Animator anim;
    [SerializeField] AnimationCurve weightCurve;
    float RWeight = 1.0f;
    float LWeight = 1.0f;
    float lookWeight = 1.0f;
    private void Start()
    {

    }


    public void SetIKWeight(AvatarIKGoal goal, float weight)
    {
        if (goal == AvatarIKGoal.RightHand) RWeight = weight;
        if (goal == AvatarIKGoal.LeftHand) LWeight = weight;
    }
    public void SetLookWeight(float lookWeight)
    {
        this.lookWeight = lookWeight;
        RWeight = Mathf.Clamp(RWeight, 0.5f, 1.0f);
        LWeight = Mathf.Clamp(LWeight, 0.5f, 1.0f);
    }
    private void FixedUpdate()
    {
        RWeight -= Time.deltaTime;
        LWeight -= Time.deltaTime;
        lookWeight -= Time.deltaTime;
        lookWeight = Mathf.Clamp(lookWeight, 0.0f, 1.0f);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtPosition(this.transform.position + new Vector3(0, -5, 0));
        anim.SetLookAtWeight(lookWeight / 4, lookWeight / 4);

        anim.SetIKPosition(AvatarIKGoal.RightHand, RHandHitIKPosition.position);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, LHandHitIKPosition.position);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weightCurve.Evaluate(RWeight));
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightCurve.Evaluate(LWeight));

    }
}
