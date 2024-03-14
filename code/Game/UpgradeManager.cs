using System;

namespace Kira;

public enum GlobalUpgradeType
{
    MoveSpeed,
    MaxHealth,
    Armor,
    Damage,
    CritChance,
    CritDamage
}

public sealed class UpgradeManager : Component
{
    private PlayerInventory Inventory { get; set; }

    public List<UpgradeInstance> UpgradeDB { get; set; }

    [Property]
    public List<GlobalUpgradeInstance> GlobalUpgrades { get; set; } = new List<GlobalUpgradeInstance>();

    public List<UpgradeInstance> UpgradePool { get; set; } = new List<UpgradeInstance>();

    public Action<List<UpgradeInstance>> OnShowUpgradesEvent;

    public List<UpgradeInstance> UpgradesObtained = new List<UpgradeInstance>();

    protected override void OnAwake()
    {
        base.OnAwake();
        Inventory = Components.GetAll<PlayerInventory>().FirstOrDefault();

        UpgradeDB = new List<UpgradeInstance>();
        foreach (GlobalUpgradeInstance globalUpgrade in GlobalUpgrades)
        {
            UpgradeDB.Add(globalUpgrade);
        }

        if (!Inventory.IsValid()) return;
        Inventory.OnLevelUpEvent += OnLevelUp;
    }

    protected override void OnStart()
    {
        base.OnStart();
        ShowUpgrades();
    }

    private void ShowUpgrades()
    {
        List<UpgradeInstance> upgrades = new List<UpgradeInstance>();
        for (int i = 0; i < 3; i++)
        {
            upgrades.Add(Random.Shared.FromList(UpgradeDB));
        }

        OnShowUpgradesEvent?.Invoke(upgrades);
    }

    private void OnLevelUp()
    {
        //TODO give new weapon
        if (Inventory.Level % 5 == 0 && Inventory.Level <= 20)
        {
        }
    }

    public void OnUpgradeObtained(UpgradeInstance upgrade)
    {
        if (upgrade.GetUpgradeType() == UpgradeTypes.Global)
        {
            Log.Info($"description : {upgrade.Description()}");
            Log.Info($"Recieved global upgrade of type: {upgrade.GlobalUpgradeType}");
        }
    }
}