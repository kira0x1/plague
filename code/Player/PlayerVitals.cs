namespace Kira;

[Category("Kira"), Icon("favorite")]
public class PlayerVitals : Component
{
    public float Health { get; set; } = 100;
    public readonly Stat MoveStat = new Stat(200);
    public readonly Stat MaxHealthStat = new Stat(100);


    public void AddModifier(UpgradeModifier modifier)
    {
        if (modifier.globalUpgrade == GlobalUpgradeType.MoveSpeed)
        {
            MoveStat.AddModifier(modifier);
        }
        else if (modifier.globalUpgrade == GlobalUpgradeType.MaxHealth)
        {
            float lastDiffPercentage = Health / MaxHealthStat.Value;
            MaxHealthStat.AddModifier(modifier);
            Health = lastDiffPercentage * MaxHealthStat.Value;
        }
    }
}