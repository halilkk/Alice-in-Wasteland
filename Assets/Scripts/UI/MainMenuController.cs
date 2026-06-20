using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        // Make sure time is running normally when returning to main menu
        UnityEngine.Time.timeScale = 1f;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Assigned to START button
    public void OnStartButton()
    {
        AudioManager.Instance.Play("Start");
        Invoke("LoadGame", 1f);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Assigned to SETTINGS button
    public void OnSettingsButton()
    {
        AudioManager.Instance.Play("Click");
        if (settingsPanel != null)
            settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // Assigned to EXIT button
    public void OnExitButton()
    {
        AudioManager.Instance.Play("Click");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
