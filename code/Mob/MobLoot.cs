using System;
using Kira;

namespace Sandbox;

[Category("Kira/Mob")]
public class MobLoot : Component
{
    [Property] private List<LootData> LootTable { get; set; } = new List<LootData>();

    protected override void OnStart()
    {
        base.OnStart();

        var vitals = Components.Get<MobVitals>();
        vitals.OnDeathEvent += OnDeath;
    }

    public void OnDeath()
    {
        foreach (LootData lootData in LootTable)
        {
            float rand = Random.Shared.Float(0, 1);
            Log.Info($"rand: {rand:F2}");
            if (rand >= lootData.DropChance)
            {
                continue;
            }

            lootData.LootPrefab.Clone(Transform.Position + Transform.Local.Up * 20f);
        }
    }
}