using System;

namespace Kira;

[Category("Kira/Mob")]
public sealed class MobSpawner : Component
{
    public List<SpawnPoint> SpawnPoints { get; set; } = new List<SpawnPoint>();

    [Property]
    public List<GameObject> MobPrefabs { get; set; } = new List<GameObject>();

    public TimeSince TimeUntilSpawn { get; set; } = 0;

    [Property]
    public float SpawnRate { get; set; } = 3f;

    protected override void OnStart()
    {
        base.OnStart();
        SpawnPoints = Scene.Components.GetAll<SpawnPoint>().ToList();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (TimeUntilSpawn < SpawnRate) return;

        TimeUntilSpawn = 0;

        GetRandomMob().Clone(GetRandomSpawnPoint().Transform.Position);
    }

    private SpawnPoint GetRandomSpawnPoint()
    {
        return Random.Shared.FromList(SpawnPoints);
    }

    private GameObject GetRandomMob()
    {
        return Random.Shared.FromList(MobPrefabs);
    }
}