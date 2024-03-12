namespace Kira;

[Category("Kira"), Icon("favorite")]
public class PlayerVitals : Component
{
    [Property]
    public float Health { get; set; } = 100;
    [Property]
    public float MaxHealth { get; set; } = 100;
}