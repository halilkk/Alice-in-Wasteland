using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] Image XPBarFill;
    float fillAmount;

    Player player;
    void Start()
    {
        player = FindFirstObjectByType<Player>(); // Get reference to the player
        player.levelManager.onExpChanged += UpdateBar; // Subscribe to exp changed event
        UpdateBar(); // Initialize the bar fill amount
    }
    void UpdateBar()
    {
        XPBarFill.fillAmount = (float)player.levelManager.currentExp / player.levelManager.currentMaxExp;
    }
    void OnDestroy()
    {
        //player.levelManager.onExpChanged -= UpdateBar; // Unsubscribe from exp changed event
    }
}