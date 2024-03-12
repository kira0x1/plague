using Kira;

namespace Sandbox;

public class AbilitySystem : GameObjectSystem
{
    public AbilitySystem(Scene scene) : base(scene)
    {
        Listen(Stage.FinishUpdate, 10, SpawnProjectiles, "SpawnProjectiles");
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

        Scene.Components.GetAll<PlayerAbilities>().FirstOrDefault()!.UpdateAbilities();
    }
}