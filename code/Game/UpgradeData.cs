namespace Kira;

public interface IUpgrade
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
}

public class UpgradeData
{
    [Property]
    public string Title { get; private set; }
    [Property]
    public string Description { get; private set; }
    [Property]
    public int Amount { get; private set; }
    [Property]
    public string Icon { get; private set; }
    [Property]
    public bool IsPercentage { get; private set; }
    [Property]
    public UpgradeTypes UpgradeType { get; private set; }

    // public UpgradeInstance()
    // {
    // }
    //
    // public UpgradeInstance(string title, string description, int amount, string icon, bool isPercentage, UpgradeTypes upgradeType)
    // {
    //     this.Title = title;
    //     this.Description = description;
    //     this.Amount = amount;
    //     this.Icon = icon;
    //     this.UpgradeType = upgradeType;
    //     this.IsPercentage = isPercentage;
    // }
}