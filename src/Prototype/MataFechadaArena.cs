using Godot;

[GlobalClass]
public partial class MataFechadaArena : PrototypeArena
{
    protected override bool ShouldSpawnPhaseDirector => false;

    public override void _Ready()
    {
        base._Ready();
        ApplyForestPalette();
        BuildVegetation();
        AddChild(new MataFechadaDirector { Name = "MataFechadaDirector" });
    }

    protected override void ConfigurePlayerLoadout(PlayerController player)
    {
        player.AddArrows(2);
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

    private void BuildVegetation()
    {
        AddVegetationProp("FernLeft", new Vector2(-260, 148), new Color("#243528"), new[]
        {
            new Vector2(-18, 8),
            new Vector2(-4, -16),
            new Vector2(10, -8),
            new Vector2(6, 12),
            new Vector2(-14, 10)
        });

        AddVegetationProp("RootMass", new Vector2(40, 156), new Color("#1a241c"), new[]
        {
            new Vector2(-22, 6),
            new Vector2(-8, -10),
            new Vector2(16, -4),
            new Vector2(20, 10),
            new Vector2(-10, 12)
        });

        AddVegetationProp("FernRight", new Vector2(300, 150), new Color("#2a3f2d"), new[]
        {
            new Vector2(-12, 10),
            new Vector2(0, -14),
            new Vector2(16, -6),
            new Vector2(12, 12),
            new Vector2(-8, 8)
        });
    }

    private void AddVegetationProp(string name, Vector2 position, Color color, Vector2[] polygon)
    {
        if (HasNode(name))
        {
            return;
        }

        var prop = new Polygon2D
        {
            Name = name,
            Color = color,
            Position = position,
            Polygon = polygon,
            ZIndex = Mathf.RoundToInt(position.Y) - 3
        };
        prop.AddToGroup("vegetation");
        AddChild(prop);
    }
}
