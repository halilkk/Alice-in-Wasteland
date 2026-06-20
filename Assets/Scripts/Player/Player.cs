using CodeStage.AdvancedFPSCounter;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player:MonoBehaviour
{
    // binded all the player related controllers and stats here, so that they can easily access each
    // other without needing to find them in the scene or pass them around as parameters

    public PlayerStats playerStats;
    public PlayerInput playerInput;
    public PlayerHealthController playerHealth;
    public PlayerIdle playerIdle;
    public AbilityManager abilityManager;
    public LevelManager levelManager;
    PlayerMovementController playerMovement;

    public GameObject GameOverScreen;
    public GameObject PlayerSprite;
    public CinemachineImpulseSource impulseSource;

    public EnemyManager enemyManager;

    [SerializeField] public TextMeshProUGUI GodMode;

    void Awake()
    {
        playerStats = new PlayerStats(this);
        playerMovement = new PlayerMovementController(this);
        playerInput = new PlayerInput(this);
        playerHealth = new PlayerHealthController(this);
        playerIdle = new PlayerIdle(this);
        abilityManager = new AbilityManager(this);
        levelManager = new LevelManager(this);
    }


    private void FixedUpdate() 
    {
        playerMovement.FixedUpdate();
        playerIdle.Update();
    }
}