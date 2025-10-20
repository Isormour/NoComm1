using _Project.Scripts.Player;
using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/ThrowShield", order = 1)]
public class SkillThrowShield : Skill
{
    public GameObject ShieldPrefab;
    public ThrowedShield prefabExisting;
    public override bool Execute()
    {
        if (prefabExisting == null)
        {
            PlayerAnchors.Instance.animator.SetTrigger("SkillThrowShield");
            return true;
        }

        else {

            return false;
        
        }
           
    }

    public void Delay()
    {
        var pos = PlayerAnchors.Instance.rightShield.transform.position;
        var rot = PlayerAnchors.Instance.rightShield.transform.rotation;
        //var scaly = PlayerAnchors.Instance.rightShield.transform.localScale;
        prefabExisting = Instantiate(ShieldPrefab, pos, rot).GetComponent<ThrowedShield>();
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
