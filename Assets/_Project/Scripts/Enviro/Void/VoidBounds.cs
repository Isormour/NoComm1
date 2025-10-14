using UnityEngine;

public class VoidBounds : MonoBehaviour
{
    public void OggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
        {
            var player = other.GetComponent<StatisticsHolder>();
            if (player != null)
            {
                player.TakeDamage(new DamageData
                {
                    Damage = 1000,
                    Owner = this.transform,
                    Target = player.transform,
                });
            }
        } 
    }
}
