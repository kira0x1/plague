namespace Kira;

public abstract class BaseAbility : IAbility
{
    public float Damage { get; set; }
    public string AbiltiyName { get; set; }
    public float CooldownTime { get; set; }
    public float WindUpTime { get; set; }
    public string Icon { get; set; }
    public PlayerAbilities Caster { get; set; }
    public TimeSince CooldownTimeUntil { get; set; }

    protected AbilityData Data { get; set; }

    protected BaseAbility(AbilityData data, PlayerAbilities caster)
    {
        this.Damage = data.BaseDamage;
        this.AbiltiyName = data.AbilityName;
        this.CooldownTime = data.CooldownTime;
        this.WindUpTime = data.WindUpTime;
        this.Icon = data.Icon;
        this.CooldownTimeUntil = 0;
        this.Caster = caster;
        this.Data = data;
    }

    public virtual void CastSpell()
    {
        this.CooldownTimeUntil = 0;
    }
}