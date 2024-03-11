namespace Kira;

public enum SpawnDirection
{
    North,
    NorthEast,
    NorthWest,
    South,
    SouthEast,
    SouthWest,
    East,
    West
}

[Category("Kira")]
public class PlayerAbilities : Component
{
    [Property, Group("Spawns")] public GameObject North { get; set; }
    [Property, Group("Spawns")] public GameObject NorthWest { get; set; }
    [Property, Group("Spawns")] public GameObject NorthEast { get; set; }
    [Property, Group("Spawns")] public GameObject South { get; set; }
    [Property, Group("Spawns")] public GameObject SouthWest { get; set; }
    [Property, Group("Spawns")] public GameObject SouthEast { get; set; }
    [Property, Group("Spawns")] public GameObject West { get; set; }
    [Property, Group("Spawns")] public GameObject East { get; set; }

    [Property]
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<AbilityData> StartAbilities { get; set; } = new List<AbilityData>();
    public List<IAbility> Abilities { get; set; } = new List<IAbility>();

    public Dictionary<SpawnDirection, GameObject> SpawnDirections = new Dictionary<SpawnDirection, GameObject>();

    protected override void OnAwake()
    {
        base.OnAwake();

        SpawnDirections[SpawnDirection.North] = North;
        SpawnDirections[SpawnDirection.NorthEast] = NorthEast;
        SpawnDirections[SpawnDirection.NorthWest] = NorthWest;
        SpawnDirections[SpawnDirection.South] = South;
        SpawnDirections[SpawnDirection.SouthWest] = SouthWest;
        SpawnDirections[SpawnDirection.SouthEast] = SouthEast;
        SpawnDirections[SpawnDirection.East] = East;
        SpawnDirections[SpawnDirection.West] = West;
    }

    protected override void OnStart()
    {
        base.OnStart();

        foreach (AbilityData startAbility in StartAbilities)
        {
            Abilities.Add(startAbility.CreateAbility(this));
        }
    }

    public void UpdateAbilities()
    {
        foreach (IAbility ability in Abilities)
        {
            if (ability.CooldownTimeUntil > ability.CooldownTime)
            {
                ability.CastSpell();
            }
        }
    }
}