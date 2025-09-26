using UnityEngine;

public class DismemberOnDeath : MonoBehaviour
{
    [SerializeField] BasicEnemy enemy;
    [SerializeField] Transform[] deparentTransforms;
    [SerializeField] Rigidbody[] rbs;
    [SerializeField] float force = 100;
    private void Start()
    {
        enemy.OnDeath += OnDeath;
        rbs = GetComponentsInChildren<Rigidbody>();
    }
    void OnDeath(BasicEnemy enemy, Vector3 sourcePosition, float damage)
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
            Vector3 direction = item.transform.position - sourcePosition;
            direction.y = Mathf.Clamp(direction.y, 0, 100);
            item.AddForce(direction * force * damage, ForceMode.Impulse);
        }
    }
}
