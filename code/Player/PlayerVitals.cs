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

    public void AddMoveModifier(UpgradeModifier modifier)
    {
        float amount = BaseMoveSpeed;

        if (!modifier.isPercentage) amount += modifier.amount;
        else amount += modifier.amount / 100 * CurMoveSpeed;

        CurMoveSpeed = amount;
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