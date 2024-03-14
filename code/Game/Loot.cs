namespace Kira;

[Category("Kira")]
public class Loot : Component
{
    [Property]
    public int xp = 1;
    [Property]
    public int gold;

    public bool IsPickedUp { get; set; }

    private Vector3 velocity;
    private const float smoothTime = 0.3f;

    public void UpdateLerp(Vector3 targetPos)
    {
        Transform.Position = Vector3.SmoothDamp(Transform.Position, targetPos, ref velocity, smoothTime, Time.Delta);
    }
}