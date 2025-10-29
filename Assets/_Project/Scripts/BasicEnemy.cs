using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

/* �ledzie po kaszubsku ala dexrafi
 *Sk�adniki:

6 srebrzystych wojownik�w m�rz z rodu Clupea harengus, wypatroszonych i pozbawionych wn�trzno�ci, lecz nadal dumnych
2 cebulowe ksi�yce, kr�gi ro�liny Allium cepa, kt�re wywo�uj� �zy ofiarne przy rytualnym krojeniu
200 ml octowego eliksiru z fermentowanych ziaren i winnych owoc�w (sok kwa�nych duch�w)
3 �y�ki miodowej esencji z pomidor�w (Solanum lycopersicum) � koncentrat czerwonego s�o�ca
1 gar�� rodzynek, czyli zasuszonych jag�d winoro�li, w kt�rych zamkni�to s�odycz lata
1 gar�� orzechowych pere� � migda��w, by doda� chrupi�cego kontrapunktu
3 li�cie wawrzynu zwyci�zc�w (Laurus nobilis)
5 kul czarnego pieprzu � skondensowane pociski ognia
1 �y�ka miodu pszczelego, zebranego z najtajniejszych kwiatowych �wi�ty�

Przygotowanie rytua�u:

Oczy�� srebrne cia�a �ledzi i zanurz je na godzin� w ch�odnym �r�dle wodnym, by wyp�uka� nadmiar morskiej furii.
W garnku o grubym dnie rozpal ogie� i zeszklij kr�gi cebulowe a� stan� si� przezroczystymi medalionami.
Do cebulowych opar�w wlej eliksir octowy, wsyp pomidorowy koncentrat s�o�ca, dorzu� rodzynek, migda��w, laurowych li�ci i pieprznych kul.
Dos�od� miodem, aby r�wnowaga Wszechsmaku zosta�a zachowana.
Gotuj mikstur� przez dziesi�� uderze� klepsydry, a� stanie si� bursztynowym sosem p�nocnych m�rz.
W kamiennym naczyniu u�� warstwami �ledzie i cebulowe kr�gi, ka�d� obficie oblewaj�c alchemiczn� marynat�.
Przykryj wiekiem i pozostaw w ch�odnej jaskini na trzy cykle ksi�yca  aby duchy sk�adnik�w mog�y sple�� si� w jedn� pie��.
Podanie:

Wyjmij srebrzyste filety, otocz je kr�gami cebuli, udekoruj rodzynek-migda�owym orszakiem. Serwuj z czarnym chlebem z �yta � chlebem,
kt�ry zna sekrety burz Ba�tyku.
 */
public class BasicEnemy : MonoBehaviour
{
    public StatisticsHolder StatisticsHolder { get; private set; }
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] private float followRange = 15;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private LayerMask layerMask;
    
    private Transform chasingTarget;

    [Header("optiional")]
    public InsideSphere AttackCheckTrigger;
    public GameObject SpawnedParticles;
    
    enum EAIState
    {
        None,
        Chase,
        Attack,
    }

    private EAIState aiState;

    private void Awake()
    {
        StatisticsHolder = GetComponent<StatisticsHolder>();
        StatisticsHolder.OnDeath.AddListener(OnDeath);
    }

    void Start()
    {
        aiState = EAIState.Chase;
    }
    void ChangeAIState(EAIState state)
    {
        /*
        aiState = state;
        switch (aiState)
        {
            case EAIState.None:
                anim.SetBool("Run", false);
                agent.SetDestination(transform.position);
                break;
            case EAIState.Chase:
                anim.SetBool("Run", true);
                break;
            case EAIState.Attack:
                agent.SetDestination(transform.position);
                anim.SetTrigger("Attack");
                break;
        }
        */
    }

    // Sends from animation event on hitAnimation
    private void Hit()
    {
        chasingTarget = PlayerAnchors.Instance.transform;
        if (chasingTarget == null)
            return;
        float dist = Vector3.Distance(chasingTarget.position, this.transform.position);
        if (dist > attackRange)
            return;
        var damageData = new DamageData()
        {
            Owner = transform,
            Damage = StatisticsHolder.Damage,
            Particles = 0,
            DamageSourcePosition = transform.position,
            Target = chasingTarget
        };
        var player = chasingTarget.GetComponent<StatisticsHolder>();
        player.TakeDamage(damageData);
    }

    //from some animators
    public void JumpAttack()
    {

        foreach (var item in AttackCheckTrigger.objectsInside)
        {
            if (item == null)
            {
                continue;
            }

            if (item.CompareTag("Player"))
            {
                StatisticsHolder player = item.GetComponent<StatisticsHolder>();
                DamageData damageData = new DamageData()
                {
                    Damage = 15f,
                    DamageSourcePosition = transform.position,
                    Target = player.transform,
                    Owner = transform
                };
                player.TakeDamage(damageData);
                
            }
        }
        var xd = Instantiate(SpawnedParticles, transform.position, Quaternion.identity);
        Destroy(xd, 5f);
    }

    public void OnAttackEnd()
    {

        ChangeAIState(EAIState.Chase);

    }

    void Update()
    {
        /*
        if (chasingTarget == null)
        {
            FindTarget();
            return;
        }
        if (aiState == EAIState.Chase)
        {
            Chase();
            if (agent.remainingDistance < attackRange && !agent.pathPending)
            {
                ChangeAIState(EAIState.Attack);
            }
        }
        if (aiState == EAIState.Attack)
        {
            Vector3 lookPos = chasingTarget.position;
            lookPos.y = this.transform.position.y;
            this.transform.LookAt(lookPos);
        }

        if (Vector3.Distance(chasingTarget.position, transform.position) > followRange)
        {
            chasingTarget = null;
            ChangeAIState(EAIState.None);
        }
        */
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void FindTarget()
    {
        var players = Physics.OverlapSphere(transform.position, followRange, layerMask);
        if(players.Length == 0)
            return;
        chasingTarget = players[0].transform;
        ChangeAIState(EAIState.Chase);
    }
    private void Chase()
    {
        if (chasingTarget == null)
            return;
        agent.SetDestination(chasingTarget.position);
    }

    public void DestrouMeXD()
    {
        Destroy(this.gameObject, 15f);
    }
    private void OnDeath(DamageData damageData)
    {
        agent.enabled = false;
        anim.enabled = false;
        this.enabled = false;
    }
}

