using System;

namespace Kira;

using Sandbox.Citizen;

public class DamageNumberText
{
    public Transform transform;
    public TimeUntil shouldDestroy;
    private RealTimeSince numbersTextUpdateSince;

    private readonly float damage;
    private const float textUpdateFrequency = 0.05f;
    private const float StopTime = 2f;

    public DamageNumberText(Vector3 Position, float Damage)
    {
        this.damage = Damage;
        this.transform = new Transform(Position);
        Gizmo.Draw.Text($"{damage}", transform);
        numbersTextUpdateSince = 0;
        shouldDestroy = StopTime;
    }

    public void Update()
    {
        Gizmo.Draw.Text($"{damage}", transform);
        if (numbersTextUpdateSince > textUpdateFrequency)
        {
            numbersTextUpdateSince = 0;
        }
    }
}

public sealed class Mob : Component
{
    [Property]
    private float StopDistance { get; set; } = 30f;

    public MobVitals Vitals { get; set; }
    public Action<float> OnHitEvent;

    private GameObject Player { get; set; }
    private NavMeshAgent Agent { get; set; }
    private SkinnedModelRenderer Target { get; set; }
    private CitizenAnimationHelper Anim { get; set; }
    private Rigidbody RgBody { get; set; }
    private ModelCollider ModelCol { get; set; }
    private List<DamageNumberText> damageNumbersTest = new List<DamageNumberText>();

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

        RgBody = Components.GetInDescendants<Rigidbody>(true);
        ModelCol = Components.GetInDescendants<ModelCollider>(true);

        Agent = Components.Get<NavMeshAgent>();
        Target = Components.GetInDescendantsOrSelf<SkinnedModelRenderer>();
        Anim = Components.Get<CitizenAnimationHelper>();
        Player = Scene.Directory.FindByName("target").FirstOrDefault();

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
                Anim.HoldType = CitizenAnimationHelper.HoldTypes.None;
                Anim.WithVelocity(RgBody.Velocity);
                Anim.WithWishVelocity(Vector3.Zero);
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
        Anim.ProceduralHitReaction(new DamageInfo(), 150f, -Transform.Local.Forward * 100f);

        //TODO: replace this with a better damage text system
        damageNumbersTest.Add(new DamageNumberText(Transform.Position + Transform.Local.Up * 60, ability.Damage));
        OnHitEvent?.Invoke(ability.Damage);
    }

    private void OnDeath()
    {
        MobState = MobStates.Dead;
        ModelCol.Enabled = true;
        RgBody.Enabled = true;
        Agent.Enabled = false;

        GameObject.Tags.Add("ragdoll");

        Vector3 dir = Vector3.One + Vector3.Random * 500f;
        Anim.ProceduralHitReaction(new DamageInfo(), 1000f, dir * 600f);

        Anim.LookAt = Player;
        Anim.LookAtEnabled = true;
        Components.GetAll<BoxCollider>().FirstOrDefault()!.Enabled = false;
    }
}