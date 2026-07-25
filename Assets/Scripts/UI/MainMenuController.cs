using UnityEngine;
using TMPro;

// Main menu buttons and popup panels (settings, level select, name entry)
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private TMP_InputField nameInput;

    public void OnStartClicked()
    {
        // First time: ask for a name, then start. Otherwise begin a new run at level 1.
        if (!SaveManager.Instance.HasName) ShowOnly(namePanel);
        else SceneLoader.Load(SceneLoader.Level1);
    }

    public void OnNameConfirm()
    {
        string name = nameInput != null ? nameInput.text : "";
        if (string.IsNullOrWhiteSpace(name)) name = "Adventurer";
        SaveManager.Instance.SetName(name);
        SceneLoader.Load(SceneLoader.Level1);
    }

    public void OnContinueClicked()
    {
        if (!SaveManager.Instance.HasName) { ShowOnly(namePanel); return; }
        SceneLoader.LoadLevel(SaveManager.Instance.Data.highestUnlocked);
    }

    public void OnLevelSelectClicked() => ShowOnly(levelSelectPanel);
    public void OnSettingsClicked() => ShowOnly(settingsPanel);
    public void OnBackClicked() => ShowOnly(mainMenuPanel);

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Show one panel, hide the others
    void ShowOnly(GameObject panel)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(panel == mainMenuPanel);
        if (settingsPanel != null) settingsPanel.SetActive(panel == settingsPanel);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(panel == levelSelectPanel);
        if (namePanel != null) namePanel.SetActive(panel == namePanel);
    }
}
