using UnityEngine;

public class DismemberOnDeath : MonoBehaviour
{
    [SerializeField] BasicEnemy enemy;
    [SerializeField] Transform[] deparentTransforms;
    [SerializeField] Rigidbody[] rbs;
    [SerializeField] float force = 100;
    private void Start()
    {
        enemy.OnDeath.AddListener(OnDeath);
        // enemy.OnDeath += OnDeath;
        rbs = GetComponentsInChildren<Rigidbody>();
    }
    void OnDeath(DamageData damageData)
    {
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
