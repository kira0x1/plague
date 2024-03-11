using System;

namespace Kira;

public sealed class Projectile : Component
{
    public float Speed { get; set; } = 20;

    public ProjectileAbility Ability { get; set; }

    private const float lifeTime = 12;
    private TimeSince spawnTimeSince;

    public ProjectileModes projectileModes;
    public Dictionary<Guid, TimeUntil> HitObjectRecords = new Dictionary<Guid, TimeUntil>();

    public void Init(AbilityData data, ProjectileAbility ability)
    {
        Speed = data.ProjectileSpeed;
        projectileModes = data.ProjectileMode;
        Ability = ability;
        spawnTimeSince = 0;
    }

    //TODO: might be more optimal to handle this in a gameobject system
    protected override void OnUpdate()
    {
        if (spawnTimeSince > lifeTime) GameObject.Destroy();
        Transform.Position += Transform.Local.Forward * Speed * Time.Delta;

        var from = Transform.Position;
        var to = from + Transform.Local.Forward * 50;
        SceneTraceResult tr = Scene.Trace.FromTo(from, to).WithTag("enemy").Radius(5f).Run();

        // Gizmo.Draw.Arrow(tr.StartPosition, tr.EndPosition);

        if (tr.Hit)
        {
            var mob = tr.GameObject.Components.Get<Mob>();
            if (!mob.IsValid()) return;

            mob.OnHit(Ability);

            if (projectileModes == ProjectileModes.DestroyOnHit)
            {
                GameObject.Destroy();
                // TODO: spawn on hit effect
            }
        }
    }
}