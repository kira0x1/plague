namespace Kira;

public enum UpgradeTypes
{
    FiringSpeed,
    ProjectileDamage,
    ReloadReduction,
    MagazineIncrease
}

[GameResource("Upgrade Data", "upgrade", "Data for Upgrades", Icon = "upgrade")]
public partial class UpgradeData : GameResource
{
    public string UpgradeTitle { get; set; } = "Upgrade";
    public string UpgradeDescription { get; set; } = "This is an upgrade :woah:";
    public int UpgradeAmount { get; set; } = 1;
    public string UpgradeIcon { get; set; }
    public bool IsPercentage { get; set; }
    public UpgradeTypes UpgradeType { get; set; }

    public UpgradeInstance CreateInstace()
    {
        return new UpgradeInstance(this);
    }
}

public class UpgradeInstance
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int Amount { get; private set; }
    public string Icon { get; private set; }
    public bool IsPercentage { get; private set; }
    public UpgradeTypes UpgradeType { get; private set; }

    public UpgradeInstance(UpgradeData data)
    {
        this.Title = data.UpgradeTitle;
        this.Amount = data.UpgradeAmount;
        this.Icon = data.UpgradeIcon;
        this.UpgradeType = data.UpgradeType;
        this.IsPercentage = data.IsPercentage;
    }
}