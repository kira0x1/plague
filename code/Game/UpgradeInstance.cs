namespace Kira;

public enum UpgradeTypes
{
    Global,
    Weapon,
    EquipWeapon,
}

public enum WeaponUpgradeType
{
    FiringSpeed,
    ProjectileDamage,
    ReloadReduction,
    MagazineIncrease,
}

public abstract class UpgradeInstance
{
    public string Title { get; set; }
    public bool IsIconAsset { get; set; }
    public Rarity Rarity { get; set; }

    protected string AmountText => $"{Amount}{(IsPercentage ? "%" : "")}";

    public abstract string Description();

    public int Amount { get; set; }
    public string Icon { get; set; }
    public bool IsPercentage { get; set; }

    public virtual GlobalUpgradeType GlobalUpgradeType { get; set; }

    [ReadOnly]
    public abstract UpgradeTypes GetUpgradeType();
}

public class GlobalUpgradeInstance : UpgradeInstance
{
    public GlobalUpgradeType GlobalUpgrade { get; set; }

    public override GlobalUpgradeType GlobalUpgradeType => GlobalUpgrade;

    public override string Description()
    {
        string description = "";

        switch (GlobalUpgrade)
        {
            case GlobalUpgradeType.Armor:
                description = $"Increase Armor by {AmountText}";
                break;
            case GlobalUpgradeType.MoveSpeed:
                description = $"Run {AmountText} Faster";
                break;
            case GlobalUpgradeType.MaxHealth:
                description = $"Increase Max Health by {AmountText}";
                break;
            case GlobalUpgradeType.PickupRadius:
                description = $"Increase PickUp Radius by {AmountText}";
                break;
            case GlobalUpgradeType.CritChance:
                description = $"Increase Critical Strike Chance by {AmountText}";
                break;
            case GlobalUpgradeType.CritDamage:
                description = $"Increase Critical Strike Damage by {AmountText}";
                break;
        }

        return description;
    }

    public override UpgradeTypes GetUpgradeType()
    {
        return UpgradeTypes.Global;
    }
}

public class WeaponUpgradeInstance : UpgradeInstance
{
    public WeaponUpgradeType WeaponUpgrade { get; }

    public override string Description()
    {
        string description = "wep upgrade";
        switch (WeaponUpgrade)
        {
            case WeaponUpgradeType.FiringSpeed:
                description = $"Shoot {AmountText} faster";
                break;
            case WeaponUpgradeType.MagazineIncrease:
                description = $"Increase Magazine by {AmountText}";
                break;
        }

        return description;
    }

    public override UpgradeTypes GetUpgradeType()
    {
        return UpgradeTypes.Weapon;
    }
}