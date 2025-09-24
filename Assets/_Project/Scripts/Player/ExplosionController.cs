using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    int level = 1;
    float damage = 1;
    internal void SetParams(int level, float damage)
    {
        this.level = level;
        this.damage = damage;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BasicEnemy enemy = other.GetComponent<BasicEnemy>();
            enemy.DealDamage(damage, this.transform.position);
        }
    }
}