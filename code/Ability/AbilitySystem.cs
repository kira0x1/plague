using Kira;

namespace Sandbox;

public class AbilitySystem : GameObjectSystem
{
    public AbilitySystem(Scene scene) : base(scene)
    {
        Listen(Stage.PhysicsStep, 10, HandleLootPickup, "HandleLootPickup");
        Listen(Stage.FinishUpdate, 10, SpawnProjectiles, "SpawnProjectiles");
    }

    private void HandleLootPickup()
    {
        PlayerInventory playerInventory = Scene.Components.GetAll<PlayerInventory>().FirstOrDefault();
        if (!playerInventory.IsValid()) return;


        Vector3 lootPos = playerInventory.Transform.Position + playerInventory.Transform.Local.Up * 50;
        var lootDrops = Scene.Components.GetAll<Loot>();

        foreach (Loot lootDrop in lootDrops)
        {
            float distance = Vector3.DistanceBetween(lootDrop.Transform.Position, lootPos);

            if (lootDrop.IsPickedUp)
            {
                if (distance <= 15f)
                {
                    playerInventory.Loot(lootDrop.xp, lootDrop.gold);
                    lootDrop.GameObject.Destroy();
                    continue;
                }

                lootDrop.Transform.Position = Vector3.Lerp(lootDrop.Transform.Position, lootPos, 50 * RealTime.Delta, true);
            }
            else if (distance <= playerInventory.LootRadius)
            {
                lootDrop.Components.Get<Rigidbody>().Enabled = false;
                lootDrop.IsPickedUp = true;
            }
        }
    }

    private void SpawnProjectiles()
    {
        var mobs = Scene.Components.GetAll<Mob>();
        bool hasAlive = false;

        foreach (Mob mob in mobs)
        {
            if (mob.MobState != Mob.MobStates.Dead)
            {
                hasAlive = true;
                break;
            }
        }

        if (!hasAlive)
        {
            return;
        }

        var playerAbilities = Scene.Components.GetAll<PlayerAbilities>().FirstOrDefault();
        if (playerAbilities.IsValid())
        {
            playerAbilities.UpdateAbilities();
        }
    }
}