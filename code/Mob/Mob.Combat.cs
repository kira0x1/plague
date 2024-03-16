namespace Kira;

public partial class Mob
{
    private TimeUntil AttackUntil { get; set; }
    private TimeSince SwingTime { get; set; }

    private float Damage { get; set; } = 20;
    private float SwingSpeed { get; set; } = 0.02f;
    private float AttackSpeed { get; set; } = 0.3f;
    private float MelleeRange { get; set; } = 60f;

    private void UpdateCombat()
    {
        if (!AttackUntil || TargetDistance > MelleeRange)
        {
            SwingTime = 0;
            return;
        }

        if (SwingTime >= SwingSpeed)
        {
            DoAttack();
            SwingTime = 0;
            AttackUntil = AttackSpeed;
        }
    }

    private void DoAttack()
    {
        Player.TakeDamage(Damage);
    }
}