using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GameObject WinScreen;

    private void Update()
    {
        float t = enemyManager.GameTime;
        int minutes = (int)(t / 60f);
        int seconds = (int)(t % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if(t >= 300f) // If the game time reaches 5 minutes, trigger the win condition
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Time.timeScale = 0;
        // Show win screen or trigger win condition here
        WinScreen.SetActive(true);
    }
}
