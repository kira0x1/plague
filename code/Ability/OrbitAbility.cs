namespace Kira;

public class OrbitAbility : BaseAbility
{
    public GameObject AbiltiyPrefab { get; set; }

    public OrbitAbility(AbilityInstance data, PlayerAbilities caster) : base(data, caster)
    {
        this.AbiltiyPrefab = data.ProjectilePrefab;
    }

    protected override void OnCastSpell()
    {
        if (!AbiltiyPrefab.IsValid())
        {
            Log.Warning("Orbit Ability Prefab is Null!");
            return;
        }

        SpawnOrbit(Caster.SpawnDirections[SpawnDirection.North].Transform);
    }

    private void SpawnOrbit(GameTransform transform)
    {
        //TODO: pooling system
        Vector3 startPos = transform.Position + transform.Rotation.Forward * Data.OrbitRadius;

        var prj = AbiltiyPrefab.Clone(startPos, transform.Rotation);
        prj.BreakFromPrefab();
        prj.Components.Get<OrbitController>().Init(Data, this, Caster.GameObject);
    }
}