﻿using System;

namespace Kira;

[Category("Kira/Mob")]
public class MobVitals : Component
{
    [Property] public float Health { get; set; } = 100;
    [Property] public float MaxHealth { get; set; } = 100;

    public bool IsDead { get; set; } = false;
    public Action<GameObject> OnDeathEvent;

    public void OnHit(DamageData damageData)
    {
        if (IsDead)
            return;

        Health -= damageData.Damage;

        if (Health <= 0)
        {
            OnDeathEvent?.Invoke(GameObject);
            Health = 0;
            IsDead = true;
        }
    }
}