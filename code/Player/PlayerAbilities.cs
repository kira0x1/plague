using System.Text.Json.Nodes;

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
public sealed class PlayerAbilities : Component
{
    [Property] public List<int> StartAbilities { get; set; } = new List<int>();

    [Property, Group("Abilities Enabled")]
    public List<bool> AbilitiesEnabled { get; set; } = new List<bool> { true, true, true, true };


    public List<IAbility> Abilities { get; set; } = new List<IAbility>();
    private AbilityDB AbilityDB { get; set; }

    private PlayerStats Stats { get; set; }
    public float GlobalCritChance => Stats.CritChanceStat.Value;
    public float GlobalCritDamage => Stats.CritDamageStat.Value;

    public override int ComponentVersion => 1;

    [JsonUpgrader(typeof(PlayerAbilities), 1)]
    private static void StringPropertyUpgrader(JsonObject json)
    {
        json.Remove("StartAbilities", out var newNode);
        json["StartAbilities"] = newNode;
    }

    #region Spawns

    [Property, Group("Spawns")] public GameObject North { get; set; }
    [Property, Group("Spawns")] public GameObject NorthWest { get; set; }
    [Property, Group("Spawns")] public GameObject NorthEast { get; set; }
    [Property, Group("Spawns")] public GameObject South { get; set; }
    [Property, Group("Spawns")] public GameObject SouthWest { get; set; }
    [Property, Group("Spawns")] public GameObject SouthEast { get; set; }
    [Property, Group("Spawns")] public GameObject West { get; set; }
    [Property, Group("Spawns")] public GameObject East { get; set; }

    public readonly Dictionary<SpawnDirection, GameObject> SpawnDirections = new Dictionary<SpawnDirection, GameObject>();

    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();

        Stats = Components.Get<PlayerStats>();

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

        AbilityDB = Scene.Components.GetAll<AbilityDB>().FirstOrDefault();
        if (!AbilityDB.IsValid())
        {
            Log.Warning("AbiltityDB not found");
            return;
        }

        Abilities = new List<IAbility>();
        foreach (int abilityIndex in StartAbilities)
        {
            var ability = AbilityDB.Abilities[abilityIndex];
            var instance = ability.CreateAbility(this);
            Abilities.Add(instance);
        }

        for (int i = 0; i < Abilities.Count && i < AbilitiesEnabled.Count; i++)
        {
            var isAbilityEnabled = AbilitiesEnabled[i];
            var ability = Abilities[i];
            ability.AbilityEnabled = isAbilityEnabled;
        }
    }

    public void UpdateAbilities()
    {
        foreach (IAbility ability in Abilities)
        {
            bool canCastSpell = ability.CooldownTimeUntil > ability.CooldownTime;

            if (canCastSpell)
            {
                ability.CastSpell();
            }
        }
    }
}