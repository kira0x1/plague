using System;

namespace Kira;

public sealed class Projectile : Component
{
    public float Speed { get; set; } = 20;

    public ProjectileAbility Ability { get; set; }

    private float lifeTime = 12;
    private TimeSince spawnTimeSince;

    public ProjectileDestroyModes projectileDestroyModes;
    public Dictionary<Guid, TimeUntil> HitObjectRecords = new Dictionary<Guid, TimeUntil>();

    public void Init(AbilityData data, ProjectileAbility ability)
    {
        Speed = data.ProjectileSpeed;
        projectileDestroyModes = data.ProjectileDestroyMode;
        Ability = ability;
        spawnTimeSince = 0;
        lifeTime = data.LifeTime;
    }

    //TODO: might be more optimal to handle this in a gameobject system
    protected override void OnUpdate()
    {
        if (spawnTimeSince > lifeTime) GameObject.Destroy();
        Transform.Position += Transform.Local.Forward * Speed * Time.Delta;

        var from = Transform.Position;
        var to = from + Transform.Local.Forward * 50;
        SceneTraceResult tr = Scene.Trace.FromTo(from, to).WithTag("enemy").HitTriggersOnly().Radius(5f).Run();

        // Gizmo.Draw.Arrow(tr.StartPosition, tr.EndPosition);

        if (tr.Hit)
        {
            var mob = tr.GameObject.Components.Get<Mob>();
            if (!mob.IsValid()) return;


            if (projectileDestroyModes == ProjectileDestroyModes.DestroyOnHit)
            {
                mob.OnHit(Ability);
                GameObject.Destroy();
                // TODO: spawn on hit effect
            }
            else
            {
                bool hasMob = HitObjectRecords.TryGetValue(tr.GameObject.Id, out TimeUntil mobHitTimeUntil);
                if (hasMob)
                {
                    if (mobHitTimeUntil)
                    {
                        mob.OnHit(Ability);
                    }
                }
                else
                {
                    HitObjectRecords.Add(tr.GameObject.Id, 5f);
                    mob.OnHit(Ability);
                }
            }
        }
    }
}