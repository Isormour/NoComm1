using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    MaterialPropertyBlock block;
    MeshRenderer[] rends;
    float Duration = 1;
    float currentTime = 0;
    [SerializeField] AnimationCurve VFXCurve;
    Vector3 locScale;
    SkillExplosion explosion;
    private void Start()
    {
        currentTime = Duration;
        rends = GetComponentsInChildren<MeshRenderer>();
        block = new MaterialPropertyBlock();
        locScale = transform.localScale;
        Destroy(this.gameObject, Duration);
    }
    internal void SetParams(SkillExplosion explosion)
    {
        this.explosion = explosion;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BasicEnemy enemy = other.GetComponent<BasicEnemy>();
            DamageData damageData = new DamageData()
            {
                Damage = explosion.Damage,
                DamageSourcePosition = transform.position,
                Target = enemy.transform,
                Owner = transform
            };
            enemy.DealDamage(damageData);
        }
    }
    public void Update()
    {
        currentTime -= Time.deltaTime;

        float ScaleTime = VFXCurve.Evaluate(1 - (currentTime / Duration));
        this.transform.localScale = locScale + new Vector3(ScaleTime, ScaleTime, ScaleTime) * explosion.Size * explosion.chargeValue;

        float VFXTime = VFXCurve.Evaluate(currentTime / Duration);
        block.SetFloat("_VFXTime", VFXTime);

        foreach (var item in rends)
        {
            item.SetPropertyBlock(block);
        }
    }
}