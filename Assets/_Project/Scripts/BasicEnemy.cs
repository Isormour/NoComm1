using System;
using UnityEngine;
using UnityEngine.AI;

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
