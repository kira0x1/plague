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
    public float ReloadTime { get; set; }
    public int MagazineCapacity { get; set; }

    public string Icon { get; set; }
    public PlayerAbilities Caster { get; set; }
    public ShootDirectionMode ShootDirection { get; set; }
    public TargetModes TargetMode { get; set; }
    public void CastSpell();

    public RealTimeSince CooldownTimeUntil { get; set; }
    public TimeSince ReloadTimeUntil { get; set; }

    public int AmmoCount { get; set; }
    public bool IsReloading { get; set; }
}