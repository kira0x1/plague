namespace Kira;

[Category("Kira"), Icon("favorite")]
public class PlayerVitals : Component
{
    [Property]
    public float Health { get; set; } = 100;
    [Property]
    public float MaxHealth { get; set; } = 100;

    [Property]
    public float BaseMoveSpeed { get; set; } = 200;
    public float CurMoveSpeed { get; set; } = 200;

    private float BaseMaxHealth { get; set; }

    protected override void OnStart()
    {
        base.OnStart();
        BaseMaxHealth = MaxHealth;
    }

    private void AddMoveMod(UpgradeModifier modifier)
    {
        float amount = BaseMoveSpeed;
        if (!modifier.isPercentage) amount += modifier.amount;
        else amount += modifier.amount / 100 * CurMoveSpeed;

        // todo: might need to rethink this, and just loop through the modifier list and recalcualte when dirty
        BaseMoveSpeed += amount;

        CurMoveSpeed = amount;
    }

    private void AddMaxHealthMod(UpgradeModifier modifier)
    {
        // use to keep same difference by percentage so if missing 10% health we keep that same difference but with the new maxhealth
        float lastDiffPercentage = Health / MaxHealth;

        float amount = BaseMaxHealth;
        if (!modifier.isPercentage) amount += modifier.amount;
        else amount += modifier.amount / 100 * MaxHealth;
        BaseMaxHealth = amount;
        MaxHealth = amount;
        Health = lastDiffPercentage * MaxHealth;
    }

    public void AddModifier(UpgradeModifier modifier)
    {
        if (modifier.globalUpgrade == GlobalUpgradeType.MoveSpeed) AddMoveMod(modifier);
        else if (modifier.globalUpgrade == GlobalUpgradeType.MaxHealth) AddMaxHealthMod(modifier);
    }

    public void RecalculateMoveSpeed(UpgradeModifier[] modifiers)
    {
        float amount = BaseMoveSpeed;

        foreach (UpgradeModifier mod in modifiers)
        {
            if (!mod.isPercentage)
                amount += mod.amount;
            else
                amount += mod.amount / 100 * CurMoveSpeed;
        }

        CurMoveSpeed = amount;
    }
}