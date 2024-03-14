using System;
using System.Numerics;

namespace Kira;

[Category("Kira")]
public sealed class OrbitController : Component
{
    public float Speed { get; set; } = 20;

    public OrbitAbility Ability { get; set; }

    private float lifeTime = 12;
    private TimeSince spawnTimeSince;

    public ProjectileDestroyModes projectileDestroyModes;
    public Dictionary<Guid, TimeUntil> HitObjectRecords = new Dictionary<Guid, TimeUntil>();
    private GameObject player;

    public void Init(AbilityInstance data, OrbitAbility ability, GameObject player)
    {
        this.player = player;
        GameObject.SetParent(player);
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

        Transform.Position = Transform.Position.RotateAround(player.Transform.Position, Rotation.FromAxis(Vector3.Up, 360 * Time.Delta));


        float radius = 80;

        SceneTraceResult tr = Scene.Trace.Sphere(radius, Transform.Position, Transform.Position).WithTag("enemy").HitTriggersOnly().Radius(120).Run();
        // Gizmo.Draw.LineSphere(tr.StartPosition, 50f);
        // Gizmo.Draw.Arrow(tr.StartPosition, tr.EndPosition, 20f, 20f);
        if (tr.Hit) HandleHit(tr);
    }

    private void HandleHit(SceneTraceResult tr)
    {
        var mob = tr.GameObject.Components.Get<Mob>();
        if (!mob.IsValid()) return;


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
            HitObjectRecords.Add(tr.GameObject.Id, 1f);
            mob.OnHit(Ability);
        }
    }
}