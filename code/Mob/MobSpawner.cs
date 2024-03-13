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
    public int MaxEnemiesAlive { get; set; } = 5;

    [Property]
    public float SpawnRate { get; set; } = 3f;

    public int TotalEnemiesSpawned { get; private set; }
    public int CurEnemiesAlive { get; private set; }

    private Dictionary<GameObject, TimeUntil> DespawnTimers { get; set; } = new Dictionary<GameObject, TimeUntil>();

    protected override void OnStart()
    {
        base.OnStart();
        SpawnPoints = Scene.Components.GetAll<SpawnPoint>().ToList();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (TimeUntilSpawn > SpawnRate && CurEnemiesAlive < MaxEnemiesAlive)
        {
            SpawnMob();
        }


        foreach (KeyValuePair<GameObject, TimeUntil> despawnTimer in DespawnTimers)
        {
            if (despawnTimer.Value)
            {
                despawnTimer.Key.Destroy();
            }
        }
    }

    private void SpawnMob()
    {
        var mobGo = GetRandomMob().Clone(GetRandomSpawnPoint().Transform.Position);
        mobGo.BreakFromPrefab();
        var mobVitals = mobGo.Components.Get<MobVitals>();

        if (mobVitals.IsValid())
        {
            CurEnemiesAlive++;
            TotalEnemiesSpawned++;
            mobVitals.OnDeathEvent += OnMobDeath;
        }

        TimeUntilSpawn = 0;
    }

    private void OnMobDeath(GameObject mob)
    {
        CurEnemiesAlive--;
        DespawnTimers.Add(mob, 20);
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