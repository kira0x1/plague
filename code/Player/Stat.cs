namespace Kira;

public class Stat
{
    private float value;

    public float Value
    {
        get
        {
            if (!IsDirty) return value;

            value = BaseValue;

            foreach (UpgradeModifier mod in Modifiers)
            {
                if (!mod.isPercentage) value += mod.amount;
                else value += mod.amount / 100 * value;
            }

            IsDirty = false;
            return value;
        }

        set => this.value = value;
    }

    public float BaseValue { get; set; }
    public bool IsDirty { get; set; }
    public List<UpgradeModifier> Modifiers { get; set; } = new List<UpgradeModifier>();

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        IsDirty = true;
    }

    public void AddModifier(UpgradeModifier mod)
    {
        Modifiers.Add(mod);
        IsDirty = true;
    }
}