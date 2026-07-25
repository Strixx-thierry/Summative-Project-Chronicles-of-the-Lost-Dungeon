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
    [SerializeField] private GameObject[] classHighlights;   // amber frame per class button

    private int chosenClass = 0;

    public void OnStartClicked()
    {
        // Always open Create Character, pre-filled, so you can keep or change name/class
        var data = SaveManager.Instance.Data;
        if (nameInput != null && !string.IsNullOrWhiteSpace(data.playerName)) nameInput.text = data.playerName;
        OnSelectClass(data.selectedClass);
        ShowOnly(namePanel);
    }

    public void OnSelectClass(int index)
    {
        chosenClass = index;
        if (classHighlights == null) return;
        for (int i = 0; i < classHighlights.Length; i++)
            if (classHighlights[i] != null) classHighlights[i].SetActive(i == index);
    }

    public void OnNameConfirm()
    {
        string name = nameInput != null ? nameInput.text : "";
        if (string.IsNullOrWhiteSpace(name)) name = "Adventurer";
        SaveManager.Instance.SetName(name);
        SaveManager.Instance.SetClass(chosenClass);
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
