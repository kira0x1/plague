namespace Kira;

public class DamageNumberText
{
    public Transform transform;
    public TimeUntil shouldDestroy;
    private RealTimeSince numbersTextUpdateSince;

    private readonly DamageData damageData;
    private const float textUpdateFrequency = 0.05f;
    private const float StopTime = 2f;

    public DamageNumberText(Vector3 Position, DamageData DamageData)
    {
        this.damageData = DamageData;
        this.transform = new Transform(Position);
        DrawText();
        numbersTextUpdateSince = 0;
        shouldDestroy = StopTime;
    }

    private void DrawText()
    {
        Gizmo.Draw.Color = damageData.IsCrit ? Color.Yellow : Color.White;
        Gizmo.Draw.Text($"{damageData.Damage}", transform, size: damageData.IsCrit ? 22f : 16f);
    }

    public void Update()
    {
        DrawText();
        if (numbersTextUpdateSince > textUpdateFrequency)
        {
            numbersTextUpdateSince = 0;
        }
    }
}