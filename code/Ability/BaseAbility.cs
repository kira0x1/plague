using System;

namespace Kira;

public abstract class BaseAbility : IAbility
{
    public float Damage { get; set; }
    public string AbiltiyName { get; set; }

    public float CooldownTime { get; set; }
    public RealTimeSince CooldownTimeUntil { get; set; }

    public string Icon { get; set; }
    public PlayerAbilities Caster { get; set; }

    public ShootDirectionMode ShootDirection { get; set; }
    public TargetModes TargetMode { get; set; }
    public bool AbilityEnabled { get; set; } = true;

    public float ReloadTime { get; set; }
    public int MagazineCapacity { get; set; }
    public int AmmoCount { get; set; }
    public bool IsReloading { get; set; }
    public TimeSince ReloadTimeUntil { get; set; }

    protected AbilityInstance Data { get; set; }


    protected BaseAbility(AbilityInstance data, PlayerAbilities caster)
    {
        this.AbiltiyName = data.AbilityName;
        this.Damage = data.BaseDamage;
        this.Icon = data.Icon;

        this.CooldownTime = data.CooldownTime;
        this.CooldownTimeUntil = 0;

        this.MagazineCapacity = data.MagazineCapacity;
        this.AmmoCount = MagazineCapacity;
        this.ReloadTime = data.ReloadTime;
        this.IsReloading = false;
        this.ReloadTimeUntil = 0;

        this.Caster = caster;
        this.ShootDirection = data.ShootDirection;
        this.TargetMode = data.TargetMode;

        this.Data = data;
    }

    public void CastSpell()
    {
        if (!AbilityEnabled) return;

        if (IsReloading)
        {
            return;
        }

        if (AmmoCount <= 0 && !IsReloading)
        {
            AmmoCount = 0;
            DoReload();
            return;
        }

        AmmoCount--;
        OnCastSpell();
        Sound.Play("ability_01", Caster.Transform.Position);
        this.CooldownTimeUntil = 0;
    }

    protected abstract void OnCastSpell();

    public void DoReload()
    {
        //TODO do reload upgrades I.E explode on reload etc

        IsReloading = true;
        Sound.Play("reload", Caster.Transform.Position);

        GameTask.DelaySeconds(ReloadTime).ContinueWith(_ =>
        {
            IsReloading = false;
            AmmoCount = MagazineCapacity;
            this.CooldownTimeUntil = 0;
        });
    }

    public DamageData GetDamageData()
    {
        float damageAmount = Data.BaseDamage;
        float critRng = Random.Shared.Float(0, 1f);
        bool isCrit = critRng < Data.BaseCritChance;
        if (isCrit) damageAmount = Data.BaseDamage * Data.BaseCritDamage;


        //TODO calculate damage with modifiers
        DamageData dmg = new DamageData(damageAmount, isCrit);
        return dmg;
    }
}