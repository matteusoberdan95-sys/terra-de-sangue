using Godot;

[GlobalClass]
public partial class MataFechadaArena : PrototypeArena
{
    public override void _Ready()
    {
        base._Ready();
        ApplyForestPalette();
        GetNodeOrNull<PhaseDirector>("PhaseDirector")?.QueueFree();
        AddChild(new MataFechadaDirector { Name = "MataFechadaDirector" });
    }

    private void ApplyForestPalette()
    {
        if (GetNodeOrNull<Polygon2D>("NightCanopy") is { } canopy)
        {
            canopy.Color = new Color("#0d1410");
        }

        if (GetNodeOrNull<Polygon2D>("WalkBand") is { } walk)
        {
            walk.Color = new Color("#1a2a1e");
        }

        AddChild(new Polygon2D
        {
            Name = "FogVeil",
            Color = new Color("#2a3d2c", 0.22f),
            Polygon = new[]
            {
                new Vector2(-500, -300),
                new Vector2(900, -300),
                new Vector2(900, 130),
                new Vector2(-500, 130)
            }
        });
    }
}
