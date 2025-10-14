using NUnit.Framework;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] private UIDocument _uiDocument;
    private VisualElement _root;
    public Button startButton;
    public Button optionsButton;
    public Button exitButton;
    #endregion

    private void Awake()
    {
        _root = _uiDocument.rootVisualElement;
        startButton = _root.Q<Button>("Play");
        optionsButton = _root.Q<Button>("Settings");
        exitButton = _root.Q<Button>("Quit");
    }

    private void OnEnable()
    {
        startButton.RegisterCallback<ClickEvent>(evt => OnStartButtonClicked());
        optionsButton.RegisterCallback<ClickEvent>(evt => OnOptionsButtonClicked());
        exitButton.RegisterCallback<ClickEvent>(evt => OnExitButtonClicked());
    }

    private void Update()
{
    UnityEngine.Cursor.visible = true;
    UnityEngine.Cursor.lockState = CursorLockMode.None;
}

    private void OnStartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnOptionsButtonClicked()
    {

    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked");
        // Add logic to exit the game
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
        #endif
    }
}
