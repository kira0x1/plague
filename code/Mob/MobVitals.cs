using System;

namespace Sandbox;

[Category("Kira/Mob")]
public class MobVitals : Component
{
    [Property] public float Health { get; set; } = 100;
    [Property] public float MaxHealth { get; set; } = 100;

    public bool IsDead { get; set; } = false;
    public Action OnDeathEvent;

    public void OnHit(float damage)
    {
        if (IsDead)
            return;

        Health -= damage;
        
        if (Health <= 0)
        {
            OnDeathEvent?.Invoke();
            Health = 0;
            IsDead = true;
        }
    }
}