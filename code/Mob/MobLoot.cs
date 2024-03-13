using System;
using Kira;

namespace Sandbox;

public struct LootTableData
{
    [Property]
    public GameObject LootPrefab { get; set; }
    [Property, Range(0, 1)]
    public float DropChance { get; set; }
}

[Category("Kira/Mob")]
public class MobLoot : Component
{
    // ReSharper disable once CollectionNeverUpdated.Local
    [Property] private List<LootTableData> LootTable { get; set; } = new List<LootTableData>();

    protected override void OnStart()
    {
        base.OnStart();

        var vitals = Components.Get<MobVitals>();
        vitals.OnDeathEvent += OnDeath;
    }

    public void OnDeath(GameObject mob)
    {
        foreach (LootTableData lootData in LootTable)
        {
            float rand = Random.Shared.Float(0, 1);

            if (rand > lootData.DropChance)
            {
                continue;
            }

            lootData.LootPrefab.Clone(Transform.Position + Transform.Local.Up * 20f);
        }
    }
}