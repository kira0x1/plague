namespace Kira;

[Category("Kira")]
public sealed class StageManager : Component
{
    // Time in seconds ( only updated every second so its ui friendly )
    public int StageTime { get; set; }
    private PlayerStats Player { get; set; }

    private TimeSince updateUiTime;

    protected override void OnStart()
    {
        base.OnStart();
        Player = Scene.Components.GetAll<PlayerStats>().FirstOrDefault();
        updateUiTime = 0;
        StageTime = 0;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (updateUiTime > 1 && !Player.IsDead)
        {
            StageTime++;
            updateUiTime = 0;
        }
    }
}