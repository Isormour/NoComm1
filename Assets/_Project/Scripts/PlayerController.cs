using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

/*
#####################################
przepis na nalesniki ala misio_emisio

Składniki: 1 kielich wydzieliny gruczołów mlecznych ssaka przeżuwającego
6 garści sproszkowanego endospermu nasion Triticum aestivum (150 g mąki) 
2 pary gamet kurzy domowej - żeńskich jajeczek połączonych z zarodkiem 
1 szczypta wykrystalizowanych minerałów z wyparowanej wody morskiej - NaCl 
3 łyżeczki ciekłego produktu tłoczenia nasion Helianthus annuus  
Odrobina substancji lipidowej do namaszczenia magicznej patelni

Przygotowanie ceremonii kulinarnej: 
Rozbij wapienne kokoony zawierające kurze gamety i wylej ich żółto-przezroczystą эссencję do rytualnej misy. 
Dolej kielich ssaczej wydzieliny i ubij trzepaczką o siedmiu promieniach do uzyskania mlecznobiałej mikstury. 
Wsyp garściami sproszkowany węglowodan ze zmielonych ziaren świętej trawy, kreśląc w powietrzu mistyczne kręgi dla uniknięcia złowrogich grudek.

Dodaj naparstek morskich kryształów oraz trzy krople złotego oleju słonecznikowego dla harmonii smaków. 
Pozostaw eliksir w stanie medytacji przez piętnaście uderzeń wahadła, aby duchy glutenu mogły osiągnąć nirwanę. 
Na żelaznej tafli rozgrzanej ogniem piekielnym (180 stopni Celsjusza) nałóż odrobinę tłuszczu na długość jednego paznokcia. 
Czerpnij chochlą porcję alchemicznej papki i rozlej ją po metalowej płaszczyźnie ruchem przypominającym taniec derwisza.

Gdy dolna strona przejdzie przemianę z płynnej w stałą (czas jednej modlitwy, ~60 sekund), 
odwróć placek drewnianą łopatką uświęconą przez kucharzy. 
Poddaj drugą stronę działaniu żywiołu ognia przez trzydzieści uderzeń serca. 
Serwuj z fermentowanym nektarem krowy, owocami rajskimi lub słodkim proszkiem z Beta vulgaris!

#####################################
*/

public class PlayerController : MonoBehaviour
{
    public StatisticsHolder StatisticsHolder { get; private set; }
    public CheckPointsManager checkPointController { get; private set; }
    [SerializeField] Animator anim;
    [SerializeField] PlayerShield leftShield;
    [SerializeField] PlayerShield rightShield;
    [SerializeField] PlayerIKController IKController;
    
    private MoveInputReceiver moveInputReceiver;


    [Header("Fuszera tutaj")]
    public AnimationCurve zajebistyTimeCurve;
    public InsideSphere AttackCheckTrigger;
    
    private void Awake()
    {
        StatisticsHolder = GetComponent<StatisticsHolder>();
        moveInputReceiver = GetComponent<MoveInputReceiver>();
        checkPointController = GetComponent<CheckPointsManager>();

        StatisticsHolder.OnDamage.AddListener(TakeHit);

        StatisticsHolder.DamageCalculator = new PlayerDamageCalculator(leftShield, rightShield);
    }
    
    public void OnCharging()
    {
        IKController.ChargeIK();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInputReceiver.isPressedShield)
        {
            anim.SetTrigger("StartGuard");
            GuardUp();
        }
        else
        {
            anim.SetTrigger("EndGuard");
            GuardDown();
        }

        if (moveInputReceiver.isPressedAttack1)
        {
            anim.SetBool("AttackPressed",true);
            Debug.Log("koduje  coś");
        }

        else
        {
            anim.SetBool("AttackPressed", false);
        }

        
    }


     public void AttackStart()
    {
        Debug.Log("evencik1");
        GameTimeManager.Instance.ManipulateTime(zajebistyTimeCurve, 1f);
    }


    public void AttackHit()
    {

        foreach (var item in AttackCheckTrigger.objectsInside)
        {
            if (item == null)
            {
                continue;
            }

            if (item.CompareTag("Enemy"))
            {

                StatisticsHolder enemy = item.GetComponent<StatisticsHolder>();
                DamageData damageData = new DamageData()
                {
                    Damage = 25f,
                    DamageSourcePosition = transform.position,
                    Target = enemy.transform,
                    Owner = transform
                };
                enemy.TakeDamage(damageData);
            }
        }



        //find closesst enemy in sphere.
        // StatisticsHolder enemy = other.GetComponent<StatisticsHolder>();

        //zapierdol najbliższemu
        /*
        if (other.CompareTag("Enemy"))
        {
            StatisticsHolder enemy = other.GetComponent<StatisticsHolder>();
            DamageData damageData = new DamageData()
            {
                Damage = explosion.Damage,
                DamageSourcePosition = transform.position,
                Target = enemy.transform,
                Owner = explosion.SkillData.Owner.transform
            };
            enemy.TakeDamage(damageData);
        }
        */
        Debug.Log("evencik2");
    }


    public SkillThrowShield ThrowShieldSkill;
    public GameObject ShieldPrefab;

    //from animator
    public void ThrowShieldRight()
    {
        ShieldPrefab = ThrowShieldSkill.ShieldPrefab;
        ThrowedShield xd = ShieldPrefab.GetComponent<ThrowedShield>();
        xd.isRight = true;
        PlayerAnchors.Instance.rightShield.GetComponent<MeshRenderer>().enabled = false;
      
        var pos = PlayerAnchors.Instance.rightShield.transform.position;
        var rot = PlayerAnchors.Instance.rightShield.transform.rotation;
        //var scaly = PlayerAnchors.Instance.rightShield.transform.localScale;
        ThrowShieldSkill.prefabExistingright = Instantiate(ShieldPrefab, pos, rot).GetComponent<ThrowedShield>();
        StartCoroutine(ReturnShieldRightAfter(5.039996f));
    }

    public void ThrowShieldLeft()
    {
        ShieldPrefab = ThrowShieldSkill.ShieldPrefab;
        ThrowedShield xd = ShieldPrefab.GetComponent<ThrowedShield>();
        xd.isRight = false;
        PlayerAnchors.Instance.leftShield.GetComponent<MeshRenderer>().enabled = false;

        var pos = PlayerAnchors.Instance.leftShield.transform.position;
        var rot = PlayerAnchors.Instance.leftShield.transform.rotation;
        //var scaly = PlayerAnchors.Instance.rightShield.transform.localScale;
        ThrowShieldSkill.prefabExistingleft = Instantiate(ShieldPrefab, pos, rot).GetComponent<ThrowedShield>();

        StartCoroutine(ReturnShieldLeftAfter(5.039996f));
    }

    //return Shield;
    public void ReturnShieldRight()
    {
        PlayerAnchors.Instance.rightShield.GetComponent<MeshRenderer>().enabled = true;
        Destroy(ThrowShieldSkill.prefabExistingright.gameObject);
        ThrowShieldSkill.prefabExistingright = null;
    }

    public void ReturnShieldLeft()
    {
        PlayerAnchors.Instance.leftShield.GetComponent<MeshRenderer>().enabled = true;
        Destroy(ThrowShieldSkill.prefabExistingleft.gameObject);
        ThrowShieldSkill.prefabExistingleft = null;
    }



    IEnumerator ReturnShieldRightAfter(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnShieldRight();
    }

    IEnumerator ReturnShieldLeftAfter(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnShieldLeft();
    }

    public void OnTriggerEnter(Collider other)
    {

    }



    private void TakeHit(DamageData damageData)
    {
        if (damageData.AngleToEnemy > 90)
            return;
        
        bool isRightSide = damageData.AngleToEnemy < 0;
        IKController.SetIKWeight(isRightSide ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand, 1);
    }

    private void GuardUp()
    {
        leftShield.GuardUp();
        rightShield.GuardUp();
    }
    private void GuardDown()
    {
        leftShield.GuardDown();
        rightShield.GuardDown();
    }
}
