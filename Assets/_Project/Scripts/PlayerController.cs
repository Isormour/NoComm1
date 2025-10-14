using System;
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
