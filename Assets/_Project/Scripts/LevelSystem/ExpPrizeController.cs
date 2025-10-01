using System;
using UnityEngine;

public class ExpPrizeController : MonoBehaviour
{
    [SerializeField] private float exp;
    [SerializeField] private BasicEnemy basicEnemy;
    
    private void Start()
    {
        basicEnemy.StatisticsHolder.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(DamageData damageData)
    {
        var levelController = damageData.Owner.GetComponent<LevelController>();
        levelController.AddExp(exp);
    }
}
