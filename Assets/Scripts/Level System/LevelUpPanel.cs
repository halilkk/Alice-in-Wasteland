using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    private const int MAX_SLOTS = 4;

    Player player;

    [Header("Level Up Panel")]
    [SerializeField] private Image icon1;
    [SerializeField] private TMP_Text name1;
    [SerializeField] private Image icon2;
    [SerializeField] private TMP_Text name2;
    [SerializeField] private Button buttton1;
    [SerializeField] private Button buttton2;

    [Header("Ability Slot Icons")]
    [SerializeField] private Image[] slotIcons; // 4 slot ikonu

    [SerializeField] private List<AbilityData> abilities;
    private AbilityData selected1;
    private AbilityData selected2;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        player.levelManager.onLevelUp += () => ShowPanel();

        HidePanel();
        ShowPanel();
    }

    void SelectAbilities()
    {
        List<AbilityData> pool = BuildAvailablePool();

        if (pool.Count == 0)
        {
            // Seçilecek yetenek yok, paneli açma
            HidePanel();
            return;
        }

        selected1 = GetWeightedRandom(pool);
        pool.Remove(selected1);

        if (pool.Count > 0)
        {
            selected2 = GetWeightedRandom(pool);
        }
        else
        {
            // Sadece 1 seçenek var, ikinci butonu gizle
            selected2 = null;
            buttton2.gameObject.SetActive(false);
        }

        buttton2.gameObject.SetActive(selected2 != null);
        SetIcons();
    }

    List<AbilityData> BuildAvailablePool()
    {
        List<AbilityData> pool = new List<AbilityData>();
        bool slotsAreFull = player.abilityManager.activeAbilities.Count >= MAX_SLOTS;

        foreach (AbilityData data in abilities)
        {
            System.Type abilityType = data.prefab.GetComponent<BaseAbility>().GetType();
            BaseAbility existing = player.abilityManager.activeAbilities.Find(a => a.GetType() == abilityType);

            if (existing != null)
            {
                if (!existing.IsMaxed)
                    pool.Add(data);
            }
            else
            {
                if (!slotsAreFull)
                    pool.Add(data);
            }
        }

        return pool;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        UnityEngine.Time.timeScale = 0f;

        SelectAbilities();
        UpdateSlotUI();

        AudioManager.Instance.Play("LevelUp");

        buttton1.onClick.RemoveAllListeners();
        buttton2.onClick.RemoveAllListeners();
        buttton1.onClick.AddListener(() => SelectAndHide(selected1));
        if (selected2 != null)
            buttton2.onClick.AddListener(() => SelectAndHide(selected2));
    }

    void HidePanel()
    {
        gameObject.SetActive(false);
        UnityEngine.Time.timeScale = 1f;
    }

    void SelectAndHide(AbilityData data)
    {
        AudioManager.Instance.Play("AbilitySelect");

        player.abilityManager.AddAbility(data.prefab);
        UpdateSlotUI();
        HidePanel();
    }

    void SetIcons()
    {
        if (selected1 != null) { icon1.sprite = selected1.icon; name1.text = selected1.abilityName; }
        if (selected2 != null) { icon2.sprite = selected2.icon; name2.text = selected2.abilityName; }
    }

    void UpdateSlotUI()
    {
        if (slotIcons == null) return;

        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (i < player.abilityManager.activeAbilities.Count)
            {
                BaseAbility ability = player.abilityManager.activeAbilities[i];
                AbilityData data = abilities.Find(a => a.prefab.GetComponent<BaseAbility>().GetType() == ability.GetType());
                if (data != null)
                {
                    slotIcons[i].sprite = data.icon;
                    slotIcons[i].color = Color.white;
                }
            }
        }
    }

    private AbilityData GetWeightedRandom(List<AbilityData> pool)
    {
        float total = 0f;
        foreach (var a in pool)
            total += a.chanceMultiplier;

        float random = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var a in pool)
        {
            cumulative += a.chanceMultiplier;
            if (random <= cumulative)
                return a;
        }

        return pool[pool.Count - 1];
    }
}
