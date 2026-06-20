using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityDarkShield : BaseAbility
{
    [Header("Settings")]
    [SerializeField] private float cooldown;
    [SerializeField] private float damage;
    [SerializeField] private float cooldownReductionPerLevel;
    [SerializeField] private float damageIncreasePerLevel;


    public GameObject shieldPrefab;
    private GameObject activeShield;
    public GameObject ShieldBlow;
    public CircleCollider2D shieldCollider;
    public bool isShieldActive = true;

    private float cdtimer = 0f;
    private bool timer = false;

    public override void Activate()
    {
        if (activeShield == null)
        {
            activeShield = Instantiate(shieldPrefab, player.PlayerSprite.transform);
            activeShield.transform.localPosition = Vector3.zero; // Position the shield at the player's location
        }
        ShieldBlow.SetActive(false);
    }
    public override void Upgrade(GameObject prefab)
    {
        if (currentLevel >= maxLevel) return;
        currentLevel++;
        cooldown -= cooldownReductionPerLevel;
        damage += damageIncreasePerLevel;
    }

    protected override void Update()
    {
        base.Update();
        if (timer)
        {
            activeShield.SetActive(isShieldActive);
            cdtimer += Time.deltaTime;
            if (cdtimer >= cooldown)
            {
                isShieldActive = true;
                timer = false;
                cdtimer = 0f;
                activeShield.SetActive(isShieldActive);
            }
        }
    }
    public void ShieldTrigger()
    {
        StartCoroutine(Explode());
        isShieldActive = false;
        timer = true;
        AudioManager.Instance.Play("Shield Blow");
    }

    IEnumerator Explode()
    {   ShieldBlow.SetActive(true);
        shieldCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        ShieldBlow.SetActive(false);
        shieldCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

}