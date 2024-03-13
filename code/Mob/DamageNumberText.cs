namespace Kira;

public class DamageNumberText
{
    public Transform transform;
    public TimeUntil shouldDestroy;
    private RealTimeSince numbersTextUpdateSince;

    private readonly float damage;
    private const float textUpdateFrequency = 0.05f;
    private const float StopTime = 2f;

    public DamageNumberText(Vector3 Position, float Damage)
    {
        this.damage = Damage;
        this.transform = new Transform(Position);
        Gizmo.Draw.Text($"{damage}", transform);
        numbersTextUpdateSince = 0;
        shouldDestroy = StopTime;
    }

    public void Update()
    {
        Gizmo.Draw.Text($"{damage}", transform);
        if (numbersTextUpdateSince > textUpdateFrequency)
        {
            numbersTextUpdateSince = 0;
        }
    }
}