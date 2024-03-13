using System;

namespace Kira;

public enum TargetModes
{
    ShootAny, // Shoots all the time
    ShootClosest,
    ShootHighestHealth,
}

public enum ShootDirectionMode
{
    ShootFacing,
    ShootBack,
    FourWay,
    SixWay
}

public record AttackRecord
{
    public Guid id;
    public TimeUntil nextAttack;

    public AttackRecord(Guid id, float attackTime = 10f)
    {
        this.id = id;
        this.nextAttack = attackTime;
    }
}

public class ProjectileAbility : BaseAbility
{
    public GameObject ProjectilePrefab { get; set; }

    protected override void OnCastSpell()
    {
        if (!ProjectilePrefab.IsValid())
        {
            Log.Warning("Projectile Prefab is Null!");
            return;
        }

        if (Data.SpawnNorth) SpawnProjectile(Caster.North.Transform);
        if (Data.SpawnNorthEast) SpawnProjectile(Caster.NorthEast.Transform);
        if (Data.SpawnNorthWest) SpawnProjectile(Caster.NorthWest.Transform);

        if (Data.SpawnSouth) SpawnProjectile(Caster.South.Transform);
        if (Data.SpawnSouthEast) SpawnProjectile(Caster.SouthEast.Transform);
        if (Data.SpawnSouthWest) SpawnProjectile(Caster.SouthWest.Transform);

        if (Data.SpawnEast) SpawnProjectile(Caster.East.Transform);
        if (Data.SpawnWest) SpawnProjectile(Caster.West.Transform);
    }

    private void SpawnProjectile(GameTransform transform)
    {
        //TODO: pooling system
        var prj = ProjectilePrefab.Clone(transform.Position, transform.Rotation);
        prj.Components.Get<Projectile>().Init(Data, this);
    }

    public ProjectileAbility(AbilityInstance data, PlayerAbilities caster) : base(data, caster)
    {
        this.ProjectilePrefab = data.ProjectilePrefab;
    }
}