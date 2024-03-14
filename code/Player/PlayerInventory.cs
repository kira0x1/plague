using System;

namespace Kira;

[Category("Kira")]
public sealed class PlayerInventory : Component
{
    public int Level { get; set; } = 1;
    public int CurExp { get; set; } = 0;
    public int MaxExp { get; set; } = 10;

    public bool ShouldShowUpgrades { get; set; }

    [Property]
    public float LootRadius { get; set; } = 20f;

    public Action OnLevelUpEvent;

    public void Loot(int xp, int gold)
    {
        if (xp > 0) OnExpPickup(xp);
    }

    private void OnExpPickup(int xp)
    {
        CurExp += xp;
        int LeftOver = CurExp - MaxExp;
        // TODO lerp xp bar in ui

        if (LeftOver == 0)
        {
            OnLevelUp();
        }
        else if (LeftOver > 0)
        {
            OnLevelUp();
            OnExpPickup(LeftOver);
        }
    }

    private int CalculateMaxExp()
    {
        // TODO use exponential xp formula
        return Level * 10;
    }

    private void OnLevelUp()
    {
        // TODO do sound and particle effects
        Level++;
        CurExp = 0;
        MaxExp = CalculateMaxExp();
        OnLevelUpEvent?.Invoke();
    }
}