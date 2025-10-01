using UnityEngine;

public class DamageData
{
    public Transform Owner;
    public Transform Target;
    
    public Vector3 DirectionToEnemy => Owner.position - Target.position;
    public float AngleToEnemy => Vector3.SignedAngle(Target.forward, DirectionToEnemy, Vector3.up);

    public StatisticsHolder OwnerStatisticsHolder => Owner.GetComponent<StatisticsHolder>();
    public StatisticsHolder TargetStatisticsHolder => Target.GetComponent<StatisticsHolder>();
    public float Damage;
    public float Mana;
    public int Particles = 20;
    public Vector3 DamageSourcePosition;
}
