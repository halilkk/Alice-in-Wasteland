using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthBarFill;

    Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarFill.fillAmount = player.playerHealth.currentHealth/player.playerHealth.maxhealth;
        player.playerHealth.OnHealthChanged += UpdateBar;
    }

    void UpdateBar(float currentHealth, float maxhealth)
    {
        healthBarFill.fillAmount = currentHealth / maxhealth;
    }

    void OnDestroy()
    {
        player.playerHealth.OnHealthChanged -= UpdateBar;
    }
}