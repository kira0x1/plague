namespace Kira;

[GameResource("Loot Data", "loot", "Data for Loot")]
public partial class LootData : GameResource
{
    [Range(0, 1)]
    public float DropChance { get; set; } = 0.1f;

    [ResourceType("prefab")]
    public GameObject LootPrefab { get; set; }
}