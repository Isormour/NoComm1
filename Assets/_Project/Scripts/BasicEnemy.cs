using System;
using UnityEngine;
using UnityEngine.AI;

/* œledzie po kaszubsku ala dexrafi
 *Sk³adniki:

6 srebrzystych wojowników mórz z rodu Clupea harengus, wypatroszonych i pozbawionych wnêtrznoœci, lecz nadal dumnych
2 cebulowe ksiê¿yce, krêgi roœliny Allium cepa, które wywo³uj¹ ³zy ofiarne przy rytualnym krojeniu
200 ml octowego eliksiru z fermentowanych ziaren i winnych owoców (sok kwaœnych duchów)
3 ³y¿ki miodowej esencji z pomidorów (Solanum lycopersicum) – koncentrat czerwonego s³oñca
1 garœæ rodzynek, czyli zasuszonych jagód winoroœli, w których zamkniêto s³odycz lata
1 garœæ orzechowych pere³ – migda³ów, by dodaæ chrupi¹cego kontrapunktu
3 liœcie wawrzynu zwyciêzców (Laurus nobilis)
5 kul czarnego pieprzu – skondensowane pociski ognia
1 ³y¿ka miodu pszczelego, zebranego z najtajniejszych kwiatowych œwi¹tyñ

Przygotowanie rytua³u:

Oczyœæ srebrne cia³a œledzi i zanurz je na godzinê w ch³odnym Ÿródle wodnym, by wyp³ukaæ nadmiar morskiej furii.
W garnku o grubym dnie rozpal ogieñ i zeszklij krêgi cebulowe a¿ stan¹ siê przezroczystymi medalionami.
Do cebulowych oparów wlej eliksir octowy, wsyp pomidorowy koncentrat s³oñca, dorzuæ rodzynek, migda³ów, laurowych liœci i pieprznych kul.
Dos³odŸ miodem, aby równowaga Wszechsmaku zosta³a zachowana.
Gotuj miksturê przez dziesiêæ uderzeñ klepsydry, a¿ stanie siê bursztynowym sosem pó³nocnych mórz.
W kamiennym naczyniu u³ó¿ warstwami œledzie i cebulowe krêgi, ka¿d¹ obficie oblewaj¹c alchemiczn¹ marynat¹.
Przykryj wiekiem i pozostaw w ch³odnej jaskini na trzy cykle ksiê¿yca  aby duchy sk³adników mog³y spleœæ siê w jedn¹ pieœñ.
Podanie:

Wyjmij srebrzyste filety, otocz je krêgami cebuli, udekoruj rodzynek-migda³owym orszakiem. Serwuj z czarnym chlebem z ¿yta – chlebem,
który zna sekrety burz Ba³tyku.
 */
public class BasicEnemy : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] float range;
    [SerializeField] float maxHealth;
    float currentHealth;
    [field: SerializeField] public float damage { private set; get; } = 1;

    public Action<BasicEnemy, Vector3, float> OnDeath;
    enum EAIState
    {
        None,
        Chase,
        Attack,
    }

    EAIState aiState;
    void Start()
    {
        currentHealth = maxHealth;
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
    internal void DealDamage(float damage, Vector3 sourcePosition)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            agent.enabled = false;
            anim.enabled = false;
            OnDeath?.Invoke(this, sourcePosition, damage);
            this.enabled = false;
        }
    }
}
