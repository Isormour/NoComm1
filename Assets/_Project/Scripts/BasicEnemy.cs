using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] float range;
    [field: SerializeField] public float damage { private set; get; } = 1;
    enum EAIState
    {
        None,
        Chase,
        Attack,
    }

    EAIState aiState;
    void Start()
    {
        aiState = EAIState.Chase;
        player = GameplayManager.Instance.Player;
    }
    void ChangeAIState(EAIState state)
    {
        aiState = state;
        switch (aiState)
        {
            case EAIState.None:
                break;
            case EAIState.Chase:
                anim.SetBool("Run", true);
                anim.SetBool("Attack", false);
                break;
            case EAIState.Attack:
                agent.SetDestination(this.transform.position);
                anim.SetBool("Run", false);
                anim.SetBool("Attack", true);
                break;
            default:
                break;
        }
    }

    // Sends from animation event on hitAnimation
    void Hit()
    {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        if (dist < range)
            player.TakeHit(this);
    }
    public void OnAttackEnd()
    {
        ChangeAIState(EAIState.Chase);
    }
    // Update is called once per frame
    void Update()
    {
        if (aiState == EAIState.Chase)
        {
            Chase();
            if (agent.remainingDistance < range && !agent.pathPending)
            {
                ChangeAIState(EAIState.Attack);
            }
        }
        if (aiState == EAIState.Attack)
        {
            Vector3 lookPos = player.transform.position;
            lookPos.y = this.transform.position.y;
            this.transform.LookAt(lookPos);
        }
    }
    void Chase()
    {
        agent.SetDestination(player.transform.localPosition);

    }

    internal void DealDamage(float damage, Vector3 position)
    {

    }
}
