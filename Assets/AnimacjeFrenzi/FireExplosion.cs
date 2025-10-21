using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    public InsideSphere beka;


    private void Awake()
    {
        transform.position = PlayerAnchors.Instance.transform.position + PlayerAnchors.Instance.transform.forward * Mathf.PI;
    }
    bool Done = false;
    private void FixedUpdate()
    {
        if (Done) this.enabled = false; 
        foreach (GameObject xd in beka.objectsInside)
        {
            if (xd == null) continue; // zabezpieczenie przed nullami

            if (xd.CompareTag("Enemy"))
            {
                StatisticsHolder enemy;

                if (xd.TryGetComponent<StatisticsHolder>(out enemy))
                {
                    DamageData damageData = new DamageData()
                    {
                        Damage = 15f,
                        DamageSourcePosition = transform.position,
                        Target = enemy.transform,
                        Owner = PlayerAnchors.Instance.transform,
                    };
                    enemy.TakeDamage(damageData);

                }



                continue;
            }
            Rigidbody xdd;
            if (xd.TryGetComponent<Rigidbody>(out xdd))
            {
                Vector3 forceDir =  xd.transform.position - transform.position;
                xdd.AddForce(forceDir*3f, ForceMode.VelocityChange);
            }



        }
        Done = true;
    }

    

}
