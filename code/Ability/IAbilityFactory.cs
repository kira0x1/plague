namespace Kira;

public enum SpellElementTypes
{
    Normal,
    Fire,
    Arcane,
    Ice,
    Earth
}

public enum SpellTypes
{
    Projectile,
    Shield
}

public interface IAbilityFactory
{
    public IAbility CreateAbility(PlayerAbilities caster);
}

public interface IAbility
{
    public float Damage { get; set; }
    public string AbiltiyName { get; set; }
    public float CooldownTime { get; set; }
    public float WindUpTime { get; set; }
    public string Icon { get; set; }
    public PlayerAbilities Caster { get; set; }
    public void CastSpell();

    public TimeSince CooldownTimeUntil { get; set; }
}