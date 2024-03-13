namespace Kira;

public enum ProjectileDestroyModes
{
    DestroyOnHit, // destroyed on first hit
    Penetrate, // can penetrate x number of mobs
    Phase // can phase through mobs
}

[GameResource("Ability Data", "ability", "Data for Abilities", Icon = "🏹")]
public partial class AbilityData : GameResource, IAbilityFactory
{
    public string AbilityName { get; set; }
    public string Icon { get; set; }

    public float CooldownTime { get; set; } = 0.5f;
    public float ReloadTime { get; set; } = 0.1f;
    public int MagazineCapacity { get; set; } = 6;

    public float LifeTime { get; set; } = 10f;

    [Group("Damage")]
    public float BaseDamage { get; set; } = 10;
    [Group("Damage")]
    public float BaseCritChance { get; set; } = 1f;
    [Group("Damage")]
    public float BaseCritDamage { get; set; } = 1f;

    public SpellElementTypes ElementType { get; set; }
    public SpellTypes SpellType { get; set; }

    public ShootDirectionMode ShootDirection { get; set; }
    public TargetModes TargetMode { get; set; }

    [Group("Projectile"), ResourceType("prefab"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public GameObject ProjectilePrefab { get; set; }

    [Group("Projectile"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public float ProjectileSpeed { get; set; } = 100f;

    [Group("Projectile"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public ProjectileDestroyModes ProjectileDestroyMode { get; set; } = ProjectileDestroyModes.DestroyOnHit;

    #region Spawns

    [Group("Projectile Direction"), ShowIf(nameof(SpellType), SpellTypes.Projectile)]
    public bool SpawnNorth { get; set; } = true;
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

    #endregion

    public IAbility CreateAbility(PlayerAbilities caster)
    {
        return new ProjectileAbility(this, caster);
    }
}