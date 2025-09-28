using System;
using UnityEngine;

public class ExpPrizeController : MonoBehaviour
{
    [SerializeField] private float exp;
    [SerializeField] private BasicEnemy basicEnemy;

    private void Awake()
    {
        basicEnemy.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(DamageData damageData)
    {
        var levelController = damageData.Owner.GetComponent<LevelController>();
        levelController.AddExp(exp);
    }
}
