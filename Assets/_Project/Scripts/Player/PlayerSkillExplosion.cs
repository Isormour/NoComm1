using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/Explosion", order = 1)]
public class PlayerSkillExplosion : PlayerSkill
{
    [SerializeField] ExplosionController ExplosionPrefab;
    [SerializeField] float Damage;
    public override void Execute()
    {
        base.Execute();
        Vector3 position = GameplayManager.Instance.Player.transform.position;
        ExplosionController exposionController = Instantiate(ExplosionPrefab);
        exposionController.SetParams(this.Level, this.Damage);
    }

}
