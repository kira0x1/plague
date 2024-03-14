using System;

namespace Kira;

using Sandbox.Citizen;

public sealed class Mob : Component
{
    [Property]
    private float StopDistance { get; set; } = 30f;

    public MobVitals Vitals { get; set; }
    public Action<float> OnHitEvent;

    private PlayerVitals Player { get; set; }
    private NavMeshAgent Agent { get; set; }
    private SkinnedModelRenderer Target { get; set; }
    private CitizenAnimationHelper Anim { get; set; }
    private Rigidbody RgBody { get; set; }
    private List<DamageNumberText> damageNumbersTest = new List<DamageNumberText>();
    private ModelPhysics ModelPhys { get; set; }

    private bool HasRemovedPhys { get; set; }
    private bool HasRemovedBodyPhys { get; set; }
    private TimeUntil removePhysTime;
    private TimeUntil removeBodyPhysTime;

    public enum MobStates
    {
        Chasing,
        Stunned,
        Dead,
    }

    public MobStates MobState { get; set; }

    protected override void OnAwake()
    {
        base.OnAwake();

        RgBody = Components.Get<Rigidbody>(true);
        ModelPhys = Components.Get<ModelPhysics>(true);

        Agent = Components.Get<NavMeshAgent>();
        Target = Components.GetInDescendantsOrSelf<SkinnedModelRenderer>();
        Anim = Components.Get<CitizenAnimationHelper>();
        Player = Scene.Components.GetAll<PlayerVitals>().FirstOrDefault();


        Vitals = Components.Get<MobVitals>();
        Vitals.OnDeathEvent += OnDeath;
        OnHitEvent += Vitals.OnHit;
    }

    protected override void OnUpdate()
    {
        switch (MobState)
        {
            case MobStates.Chasing:
                UpdateChase();
                UpdateText();
                break;
            case MobStates.Stunned:
                UpdateText();
                break;
            case MobStates.Dead:
                if (!HasRemovedPhys && removePhysTime)
                {
                    RgBody.MotionEnabled = false;
                    RgBody.Destroy();
                    HasRemovedPhys = true;
                }

                if (!HasRemovedBodyPhys && removeBodyPhysTime)
                {
                    ModelPhys.Renderer.Enabled = false;
                    ModelPhys.Enabled = false;
                    HasRemovedBodyPhys = true;
                }

                break;
        }
    }

    private void UpdateChase()
    {
        Anim.WithVelocity(Agent.Velocity);
        Anim.WithWishVelocity(Agent.WishVelocity);

        Anim.HoldType = CitizenAnimationHelper.HoldTypes.Swing;

        float distance = Vector3.DistanceBetween(Transform.Position, Player.Transform.Position);

        if (distance > StopDistance)
        {
            Agent.MoveTo(Player.Transform.Position);
        }
        else
        {
            Agent.Stop();
        }
    }


    private void UpdateText()
    {
        foreach (DamageNumberText dmgText in damageNumbersTest)
        {
            if (dmgText.shouldDestroy)
            {
                continue;
            }

            dmgText.Update();
            dmgText.transform.Position += Transform.Local.Up * 30f * Time.Delta;
        }
    }

    public void OnHit(BaseAbility ability)
    {
        if (MobState == MobStates.Dead) return;
        Anim.ProceduralHitReaction(new DamageInfo(), 150f, -Transform.Local.Forward * 100f);

        //TODO: replace this with a better damage text system
        damageNumbersTest.Add(new DamageNumberText(Transform.Position + Transform.Local.Up * 60, ability.Damage));
        OnHitEvent?.Invoke(ability.Damage);
    }

    private void OnDeath(GameObject mob)
    {
        MobState = MobStates.Dead;
        removePhysTime = 1;
        removeBodyPhysTime = 1.2f;

        Components.GetAll<BoxCollider>().FirstOrDefault()!.Enabled = false;
        RgBody.Enabled = true;
        ModelPhys.Enabled = true;

        Agent.Stop();
        Agent.Enabled = false;
        Anim.Enabled = false;
        GameObject.Tags.Add("ragdoll");

        // RgBody.ApplyForceAt(Transform.Position + Vector3.Up * 45, -Transform.Local.Forward * 1500);
    }
}