namespace Kira;

[Category("Kira")]
public class AbilityDB : Component
{
    [Property]
    public List<AbilityInstance> Abilities { get; set; } = new List<AbilityInstance>();
}