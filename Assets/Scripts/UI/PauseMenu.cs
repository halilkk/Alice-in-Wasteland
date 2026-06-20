using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject ButtonContinue;
    [SerializeField] private GameObject ButtonSettings;
    [SerializeField] private GameObject ButtonRestart;
    [SerializeField] private GameObject ButtonExit;

    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        player.playerInput.onPause += TogglePause;

        gameObject.SetActive(false);
    }

    public void TogglePause()
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
        UnityEngine.Time.timeScale = isActive ? 0f : 1f;
    }

    public void OpenSettingsMenu()
    {
        AudioManager.Instance.Play("Click");
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    public void ContinueGame()
    {
        AudioManager.Instance.Play("Click");
        TogglePause();
    }

    public void RestartGame()
    {
        AudioManager.Instance.Play("Click");
        UnityEngine.Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        if (player != null)
            player.playerInput.onPause -= TogglePause;
    }
}