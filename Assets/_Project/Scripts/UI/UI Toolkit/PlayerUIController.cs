using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIController : MonoBehaviour
{
    [Header("Death Screen")]
    [SerializeField] UIDocument deathScreenUIDocument;
    StatisticsHolder playerStats;

    private VisualElement deathScreenRoot;
    private Label deathScreenLabel;

    private VisualElement deathMenuRoot;
    private Button respawnButton;
    private Button exitButton;
    void Awake()
    {
        playerStats = GetComponentInParent<StatisticsHolder>();
        playerStats.OnDeath.AddListener(OnDeathUI);

        deathScreenRoot = deathScreenUIDocument.rootVisualElement.Q<VisualElement>("Root");
        deathScreenLabel = deathScreenRoot.Q<Label>("DeathText");

        deathScreenRoot.AddToClassList("NotVisible");

        deathMenuRoot = deathScreenUIDocument.rootVisualElement.Q<VisualElement>("DeathMenu");
        respawnButton = deathMenuRoot.Q<Button>("Respawn");
        exitButton = deathMenuRoot.Q<Button>("BackToMenu");

        
    }

    private void OnEnable()
    {
        respawnButton.RegisterCallback<ClickEvent>(ev =>
        {
            HideDeathMenu();
            HideDeathScreen();
            playerStats.GetComponent<CheckPointsManager>().RespawnPlayer();
            playerStats.ChangeAmountHealth(100);
            ShowDeathScreen();
            deathScreenRoot.AddToClassList("NotVisible");
            ShowLabel();
        });

        exitButton.RegisterCallback<ClickEvent>(ev =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        });
        

        HideDeathScreen();
        HideDeathMenu();
    }

    private void OnDisable()
    {
        respawnButton.UnregisterCallback<ClickEvent>(ev => { });
        exitButton.UnregisterCallback<ClickEvent>(ev => { }); 
    }

    public void OnDeathUI(DamageData damageData)
    {
        StartCoroutine(ShowDeathScreen());
    }
    
    internal IEnumerator ShowDeathScreen()
    {
        yield return new WaitForSeconds(3f);

        deathScreenRoot.style.display = DisplayStyle.Flex;
        deathScreenRoot.RemoveFromClassList("NotVisible");

        yield return new WaitForSeconds(3f);

        HideLabel();
        ShowDeathMenu();

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
    internal void HideDeathScreen() => deathScreenRoot.style.display = DisplayStyle.None;
    internal void ShowDeathMenu() => deathMenuRoot.style.display = DisplayStyle.Flex;
    internal void HideDeathMenu() => deathMenuRoot.style.display = DisplayStyle.None;
    internal void HideLabel() => deathScreenLabel.style.display = DisplayStyle.None;
    internal void ShowLabel() => deathScreenLabel.style.display = DisplayStyle.Flex;
    


    

    
}
