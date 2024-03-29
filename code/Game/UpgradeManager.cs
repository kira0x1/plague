﻿using System;

namespace Kira;

public enum GlobalUpgradeType
{
    None,
    MoveSpeed,
    MaxHealth,
    Armor,
    Damage,
    CritChance,
    CritDamage,
    PickupRadius
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
    public string Icon { get; set; }
    public const float UnCommonChance = 0.35f;
    public const float RareChance = 0.18f;
    public const float EpicChance = 0.08f;

    public GlobalUpgradeInstance Common { get; set; }
    public GlobalUpgradeInstance UnCommon { get; set; }
    public GlobalUpgradeInstance Rare { get; set; }
    public GlobalUpgradeInstance Epic { get; set; }

    private bool IsDirty = true;

    public GlobalUpgradeInstance RollForUpgrade()
    {
        float rng = Random.Shared.Float(0, 1);

        if (IsDirty)
        {
            Common.Icon = Icon;
            Common.Rarity = Rarity.Common;

            UnCommon.Icon = Icon;
            UnCommon.Rarity = Rarity.UnCommon;

            Rare.Icon = Icon;
            Rare.Rarity = Rarity.Rare;

            Epic.Icon = Icon;
            Epic.Rarity = Rarity.Epic;
            IsDirty = false;
        }

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
    public GlobalUpgradeContainer PickUpRadiusUpgrades { get; set; }
    public GlobalUpgradeContainer FireRateUpgrades { get; set; }
    public GlobalUpgradeContainer DamageUpgrades { get; set; }
    public GlobalUpgradeContainer CritChanceUpgrades { get; set; }
    public GlobalUpgradeContainer CritDamageUpgrades { get; set; }
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

    private PlayerStats Stats { get; set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        Inventory = Scene.Components.GetAll<PlayerInventory>().FirstOrDefault();
        Stats = Scene.Components.GetAll<PlayerStats>().FirstOrDefault();
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
            var randomUpgrade = Random.Shared.FromList(UpgradePool);
            UpgradePool.Remove(randomUpgrade);
            upgrades.Add(randomUpgrade);
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
        UpgradePool.Add(GlobalUpgradeDB.PickUpRadiusUpgrades.RollForUpgrade());
        UpgradePool.Add(GlobalUpgradeDB.CritChanceUpgrades.RollForUpgrade());
        UpgradePool.Add(GlobalUpgradeDB.CritDamageUpgrades.RollForUpgrade());
    }

    public void OnUpgradeObtained(UpgradeInstance upgrade)
    {
        if (upgrade.GetUpgradeType() == UpgradeTypes.Global)
        {
            UpgradeModifier modifier = new UpgradeModifier(upgrade.Amount, upgrade.IsPercentage, upgrade.GlobalUpgradeType);
            Stats.AddModifier(modifier);
        }
    }
}