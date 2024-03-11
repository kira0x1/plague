namespace Kira;

public enum ProjectileModes
{
    DestroyOnHit, // destroyed on first hit
    Penetrate, // can penetrate x number of mobs
    Phase // can phase through mobs
}

[GameResource("Ability Data", "ability", "Data for Abilities")]
public partial class AbilityData : GameResource, IAbilityFactory
{
    public float BaseDamage { get; set; } = 10;
    public string AbilityName { get; set; }

    public float CooldownTime { get; set; } = 0.5f;
    public float WindUpTime { get; set; } = 0.1f;

    [ResourceType("jpg")]
    public string Icon { get; set; }

    public SpellElementTypes ElementType { get; set; }
    public SpellTypes SpellType { get; set; }

    [Group("Projectile"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public GameObject ProjectilePrefab { get; set; }

    [Group("Projectile"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public float ProjectileSpeed { get; set; } = 100f;

    [Group("Projectile"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public ProjectileModes ProjectileMode { get; set; } = ProjectileModes.DestroyOnHit;

    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnNorth { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnNorthWest { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnNorthEast { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnSouth { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnSouthEast { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnSouthWest { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnEast { get; set; }
    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnWest { get; set; }

    public IAbility CreateAbility(PlayerAbilities caster)
    {
        return new ProjectileAbility(this, caster);
    }
}