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
    [SerializeField] Animator anim;
    [SerializeField] PlayerShield leftShield;
    [SerializeField] PlayerShield rightShield;
    [SerializeField] SimpleVFX hitVFX;
    [SerializeField] PlayerIKController IKController;
    [field: SerializeField] public SkillSLot[] skillSlots { private set; get; }
    public PlayerData playerStats { private set; get; }
    public float MaxMana => playerStats.maxMana;
    public float MaxHealth => playerStats.maxHealth;
    public float CurrentMana => playerStats.currentMana;
    public float CurrentHealth => playerStats.currentHealth;

    private void Start()
    {
        playerStats = new PlayerData(10, 10);
    }
    internal void TakeHit(BasicEnemy basicEnemy)
    {
        Vector3 DirToEnemy = basicEnemy.transform.position - this.transform.position;
        float angle = Vector3.SignedAngle(this.transform.forward, DirToEnemy, Vector3.up);
        if (Mathf.Abs(angle) > 90)
        {
            TakeDamage(basicEnemy.damage, 20);
            return;
        }

        bool isRightSide = angle < 0;
        PlayerShield hitShield = isRightSide ? leftShield : rightShield;

        switch (hitShield.shieldState)
        {
            case PlayerShield.EShieldState.PerfectGuard:
                RestoreMana(1);
                hitShield.OnHitBlock(true);
                IKController.SetIKWeight(isRightSide ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand, 1);
                break;
            case PlayerShield.EShieldState.Guard:
                hitShield.OnHitBlock(false);
                RestoreMana(1);
                TakeDamage(basicEnemy.damage / 3, 3);
                IKController.SetIKWeight(isRightSide ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand, 1);
                break;
            default:
                TakeDamage(basicEnemy.damage, 20);
                break;
        }
    }
    public void OnCharging()
    {
        IKController.ChargeIK();
    }
    private void RestoreMana(float v)
    {
        playerStats.ChangeAmountMana(v);
    }

    private void TakeDamage(float Damage, int particles)
    {
        playerStats.ChangeAmountHealth(-Damage);
        hitVFX.Play(particles);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("StartGuard");
            GuardUp();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetTrigger("EndGuard");
            GuardDown();
        }
        foreach (var item in skillSlots)
        {
            item.CheckSkillInput();
        }
    }

    private void GuardUp()
    {
        if (leftShield.shieldState != PlayerShield.EShieldState.Thrown) leftShield.GuardUp();
        if (rightShield.shieldState != PlayerShield.EShieldState.Thrown) rightShield.GuardUp();
    }
    private void GuardDown()
    {
        if (leftShield.shieldState != PlayerShield.EShieldState.Thrown) leftShield.GuardDown();
        if (rightShield.shieldState != PlayerShield.EShieldState.Thrown) rightShield.GuardDown();
    }

    internal void SetSkillInSlot(int index, PlayerSkill playerSkill)
    {
        skillSlots[index].SetSkill(playerSkill);
    }
}
