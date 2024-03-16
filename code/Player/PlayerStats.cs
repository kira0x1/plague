using System;

namespace Kira;

[Category("Kira"), Icon("favorite")]
public class PlayerStats : Component
{
    public float Health { get; set; } = 100;
    public readonly Stat MoveStat = new Stat(200);
    public readonly Stat MaxHealthStat = new Stat(100);
    public readonly Stat PickupRadiusStat = new Stat(120f);

    public readonly Stat CritChanceStat = new Stat(0.1f);
    public readonly Stat CritDamageStat = new Stat(2);

    public bool IsDead { get; set; } = false;
    public Action OnDeathEvent;

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        Health -= damage;
        if (Health <= 0)
        {
            IsDead = true;
            Health = 0;
            OnDeathEvent?.Invoke();
        }
    }

    public void AddModifier(UpgradeModifier modifier)
    {
        switch (modifier.globalUpgrade)
        {
            case GlobalUpgradeType.MoveSpeed:
                MoveStat.AddModifier(modifier);
                break;
            case GlobalUpgradeType.MaxHealth:
                float lastDiffPercentage = Health / MaxHealthStat.Value;
                MaxHealthStat.AddModifier(modifier);
                Health = lastDiffPercentage * MaxHealthStat.Value;
                break;
            case GlobalUpgradeType.PickupRadius:
                PickupRadiusStat.AddModifier(modifier);
                break;
            case GlobalUpgradeType.CritChance:
                CritChanceStat.AddModifier(modifier);
                break;
            case GlobalUpgradeType.CritDamage:
                CritDamageStat.AddModifier(modifier);
                break;
        }
    }
}