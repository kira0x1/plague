namespace Kira;

public class DamageData
{
    public float Damage { get; set; }
    public bool IsCrit { get; set; }

    public DamageData(float damage, bool IsCrit = false)
    {
        this.Damage = damage;
        this.IsCrit = IsCrit;
    }
}