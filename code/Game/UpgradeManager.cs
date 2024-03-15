using System;

namespace Kira;

public enum GlobalUpgradeType
{
    None,
    MoveSpeed,
    MaxHealth,
    Armor,
    Damage,
    CritChance,
    CritDamage
}

public enum Rarity
{
    Common,
    UnCommon,
    Rare,
    Epic
}

public class GlobalUpgradeContainer
{
    public const float UnCommonChance = 0.35f;
    public const float RareChance = 0.18f;
    public const float EpicChance = 0.08f;

    public GlobalUpgradeInstance Common { get; set; }
    public GlobalUpgradeInstance UnCommon { get; set; }
    public GlobalUpgradeInstance Rare { get; set; }
    public GlobalUpgradeInstance Epic { get; set; }

    public GlobalUpgradeInstance RollForUpgrade()
    {
        float rng = Random.Shared.Float(0, 1);

        Common.Rarity = Rarity.Common;
        UnCommon.Rarity = Rarity.UnCommon;
        Rare.Rarity = Rarity.Rare;
        Epic.Rarity = Rarity.Epic;

        if (rng <= EpicChance) return Epic;
        if (rng <= RareChance) return Rare;
        if (rng <= UnCommonChance) return UnCommon;
        return Common;
    }
}

public class GlobalUpgradeDB
{
    public GlobalUpgradeContainer MovementUpgrades { get; set; }
    public GlobalUpgradeContainer MaxHealthUpgrades { get; set; }
}

public class UpgradeModifier
{
    public float amount;
    public bool isPercentage = false;
    public GlobalUpgradeType globalUpgrade;

    public UpgradeModifier(float amount, bool isPercentage, GlobalUpgradeType globalUpgrade = GlobalUpgradeType.None)
    {
        this.amount = amount;
        this.isPercentage = isPercentage;
        this.globalUpgrade = globalUpgrade;
    }
}

public sealed class UpgradeManager : Component
{
    private PlayerInventory Inventory { get; set; }

    public List<UpgradeInstance> UpgradeDB { get; set; }
    [Property] public GlobalUpgradeDB GlobalUpgradeDB { get; set; }

    public List<UpgradeInstance> UpgradePool { get; set; } = new List<UpgradeInstance>();
    public Action<List<UpgradeInstance>> OnShowUpgradesEvent;

    public List<UpgradeInstance> UpgradesObtained = new List<UpgradeInstance>();
    public List<UpgradeModifier> Modifiers = new List<UpgradeModifier>();

    private PlayerVitals Vitals { get; set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        Inventory = Scene.Components.GetAll<PlayerInventory>().FirstOrDefault();
        Vitals = Scene.Components.GetAll<PlayerVitals>().FirstOrDefault();

        // UpgradeDB = new List<UpgradeInstance>();
        // foreach (GlobalUpgradeContainer globalUpgrade in GlobalUpgrades)
        // {
        //     UpgradeDB.Add(globalUpgrade);
        // }
    }

    protected override void OnStart()
    {
        base.OnStart();

        Inventory.OnLevelUpEvent += OnLevelUp;
        ShowUpgrades();
    }

    private void ShowUpgrades()
    {
        List<UpgradeInstance> upgrades = new List<UpgradeInstance>();
        UpgradePool.Clear();


        PopulateGlobalUpgradePool();

        for (int i = 0; i < 3; i++)
        {
            upgrades.Add(Random.Shared.FromList(UpgradePool));
        }

        OnShowUpgradesEvent?.Invoke(upgrades);
    }

    private void OnLevelUp()
    {
        //TODO give new weapon
        if (Inventory.Level % 5 == 0 && Inventory.Level <= 20)
        {
            ShowUpgrades();
        }
    }

    /// <summary>
    /// Populats the upgrades pool with global upgrades
    /// </summary>
    private void PopulateGlobalUpgradePool()
    {
        UpgradePool.Add(GlobalUpgradeDB.MovementUpgrades.RollForUpgrade());
        UpgradePool.Add(GlobalUpgradeDB.MaxHealthUpgrades.RollForUpgrade());
    }

    public void OnUpgradeObtained(UpgradeInstance upgrade)
    {
        if (upgrade.GetUpgradeType() == UpgradeTypes.Global)
        {
            UpgradeModifier modifier = new UpgradeModifier(upgrade.Amount, upgrade.IsPercentage, upgrade.GlobalUpgradeType);
            Modifiers.Add(modifier);
            Vitals.AddModifier(modifier);
        }
    }
}