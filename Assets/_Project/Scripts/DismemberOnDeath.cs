using UnityEngine;

public class DismemberOnDeath : MonoBehaviour
{
    [SerializeField] BasicEnemy enemy;
    [SerializeField] Transform[] deparentTransforms;
    [SerializeField] Rigidbody[] rbs;
    [SerializeField] float force = 100;
    private void Start()
    {
        enemy.StatisticsHolder.OnDeath.AddListener(OnDeath);
        // enemy.OnDeath += OnDeath;
        rbs = GetComponentsInChildren<Rigidbody>();

        foreach (var item in GetComponentsInChildren<Collider>())
        {
            if (item.gameObject != this.gameObject)
            {
                item.enabled = false;
            }
        }
    }
    private void OnDeath(DamageData damageData)
    {
        foreach (var item in GetComponentsInChildren<Collider>())
        {
                item.enabled = true;
        }

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.linearVelocity = Vector3.zero; 
            item.angularVelocity = Vector3.zero;
        }
        foreach (var item in deparentTransforms)
        {
            item.SetParent(this.transform);
            if (item.TryGetComponent(out CharacterJoint joint))
            {
                Destroy(joint);
            }
        }

        foreach (var item in rbs)
        {
            Vector3 direction = item.transform.position - damageData.DamageSourcePosition;
            direction.y = Mathf.Clamp(direction.y, 0, 100);
            item.AddForce(direction * force * damageData.Damage, ForceMode.Impulse);
        }
    }
}
