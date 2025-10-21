using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/SkillCastFireExplosionFromShield", order = 2137)]
public class SkillCastFireExplosionFromShield : Skill
{
    public GameObject ExplosionPrefab;
    public ThrowedShield prefabExistingright;
    public override bool Execute()
    {
        PlayerAnchors.Instance.animator.SetTrigger("SkillCastExplosion");
        return true;

    }

    public override bool ReleaseCharge()
    {
        throw new System.NotImplementedException();
    }

    public override bool StartCharge()
    {
        throw new System.NotImplementedException();
    }

    public override bool UpdateCharge()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
