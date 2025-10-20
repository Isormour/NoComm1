using _Project.Scripts.Player;
using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/ThrowShield", order = 1)]
public class SkillThrowShield : Skill
{
    public GameObject ShieldPrefab;
    public ThrowedShield prefabExistingright;
    public ThrowedShield prefabExistingleft;
    bool isRight;
    public override bool Execute()
    {
        if (prefabExistingright == null || prefabExistingleft == null)
        {
            int xd = UnityEngine.Random.Range(0, 2);
            if (xd == 0)
            {
                PlayerAnchors.Instance.animator.SetTrigger("SkillThrowShield_right");

            }

            else
            {
                PlayerAnchors.Instance.animator.SetTrigger("SkillThrowShield_right");
                //PlayerAnchors.Instance.animator.SetTrigger("SkillThrowShield_left");
            }

            Debug.Log("EXECUTE");
            return true;
        }

        else {

            return false;
        }
           
    }

    public override bool StartCharge()
    {
        throw new System.NotImplementedException();
    }

    public override bool UpdateCharge()
    {
        throw new System.NotImplementedException();
    }

    public override bool ReleaseCharge()
    {
        throw new System.NotImplementedException();
    }
}
