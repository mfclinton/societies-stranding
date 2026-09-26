using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUI : MonoBehaviour
{
    private Button playButton;
    private Button exitButton;

    private Button settingsButton;
    private Button backButton;

    private VisualElement mainMenuButtons;
    private VisualElement settingsMenu;

    private Label highScoreLabel;

    private void OnEnable()
    {
        // Find the root visual element
        var root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("PlayButton");
        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClick);

        exitButton = root.Q<Button>("ExitButton");
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClick);

        settingsButton = root.Q<Button>("SettingsButton");
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClick);

        backButton = root.Q<Button>("BackButton");
        backButton.RegisterCallback<ClickEvent>(OnBackButtonClick);

        mainMenuButtons = root.Q<VisualElement>("MainMenuButtons");
        settingsMenu = root.Q<VisualElement>("SettingsMenu");
        
        highScoreLabel = root.Q<Label>("HighScoreLabel");
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreLabel.text = $"High Score: {highScore}";
        highScoreLabel.visible = 0 < highScore;
    }

    private void OnDisable()
    {
        // Unregister the click event listeners when this object is disabled
        if (playButton != null)
        {
            playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClick);
        }

        if (exitButton != null)
        {
            exitButton.UnregisterCallback<ClickEvent>(OnExitButtonClick);
        }

        if (settingsButton != null)
        {
            settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClick);
        }

        if (backButton != null)
        {
            backButton.UnregisterCallback<ClickEvent>(OnBackButtonClick);
        }

    }

    private void OnPlayButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene(1);
    }

    private void OnExitButtonClick(ClickEvent evt)
    {
#if UNITY_EDITOR
        // If in Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If in the built game, quit the game
        Application.Quit();
#endif
    }

    private void OnSettingsButtonClick(ClickEvent evt)
    {
        // Hide main menu buttons and show settings menu
        mainMenuButtons.style.display = DisplayStyle.None;
        settingsMenu.style.display = DisplayStyle.Flex;
    }

    private void OnBackButtonClick(ClickEvent evt)
    {
        // Hide settings menu and show main menu buttons
        settingsMenu.style.display = DisplayStyle.None;
        mainMenuButtons.style.display = DisplayStyle.Flex;
    }

}
