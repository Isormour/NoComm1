using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/Explosion", order = 1)]
public class PlayerSkillExplosion : Skill
{
    [SerializeField] ExplosionController ExplosionPrefab;
    [SerializeField] ParticleSystem ChargeVFXPrefab;
    [field: SerializeField] public float Damage { private set; get; } = 10;
    [field: SerializeField] public float Size { private set; get; } = 1;

    public float chargeValue { private set; get; } = 0;

    private ParticleSystem currentChargeVFX;
    private PlayerController playerController;

    public override void InitSkillData(SkillData skillData)
    {
        base.InitSkillData(skillData);
        playerController = skillData.Owner.GetComponent<PlayerController>();
    }

    public override void StartCharge()
    {
        base.StartCharge();
        chargeValue = 0;
        if (currentChargeVFX == null)
        {
            CreateVFXInstance();
        }
        ChangeChargeVRXParams(0, new Vector2(0.1f, 0.2f));

    }

    public override void UpdateCharge()
    {
        chargeValue += Time.deltaTime;
        float min = Mathf.Clamp(0.1f * (1 + chargeValue), 0.1f, 1.0f);
        float max = Mathf.Clamp(0.2f * (1 + chargeValue), 0.2f, 2.0f);

        ChangeChargeVRXParams(100 * chargeValue, new Vector2(min, max));

        playerController.OnCharging();
        skillData.StatisticsHolder.ChangeAmountMana(-Cost * Time.deltaTime);
    }
    public override void ReleaseCharge()
    {
        Vector3 position = playerController.transform.position;
        position += new Vector3(0, 0.5f, 0);// fix y position
        ExplosionController exposionController = Instantiate(ExplosionPrefab);
        exposionController.SetParams(this);
        exposionController.transform.position = position;
        ChangeChargeVRXParams(0, new Vector2(0.1f, 0.2f));
    }
    private void CreateVFXInstance()
    {
        currentChargeVFX = Instantiate(ChargeVFXPrefab);
        currentChargeVFX.transform.SetParent(playerController.transform);
        currentChargeVFX.transform.localPosition = new Vector3(0, 0.75f, 0);
    }
    private void ChangeChargeVRXParams(float rate, Vector2 sizeMinMax)
    {
        ParticleSystem.EmissionModule psEmission = currentChargeVFX.emission;
        psEmission.rateOverTime = rate;
        ParticleSystem.MainModule psMain = currentChargeVFX.main;
        psMain.startSizeMultiplier = Mathf.Clamp((chargeValue * 0.1f), 0, 3);
    }
}
