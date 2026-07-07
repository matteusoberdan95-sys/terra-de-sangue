using Godot;

[GlobalClass]
public partial class AldeiaEmCinzasArena : PrototypeArena
{
    private readonly System.Collections.Generic.List<Polygon2D> _emberLights = new();
    private float _ambientTime;

    public override void _Ready()
    {
        base._Ready();
        BuildForegroundProps();
        CacheEmberLights();
        GetNodeOrNull<PhaseDirector>("PhaseDirector")?.QueueFree();
        AddChild(new PhaseDirector { Name = "PhaseDirector" });
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _ambientTime += (float)delta;
        PulseEmberLights();
    }

    private void BuildForegroundProps()
    {
        AddProp("BrokenFence", new Vector2(-180, 150), new Color("#1a1410"), new[]
        {
            new Vector2(-24, 0), new Vector2(-20, -28), new Vector2(-14, -26), new Vector2(-16, 0),
            new Vector2(8, 0), new Vector2(12, -18), new Vector2(18, -16), new Vector2(14, 0)
        }, -2);

        AddProp("AshMound", new Vector2(60, 188), new Color("#3a2a22"), new[]
        {
            new Vector2(-28, 4), new Vector2(-8, -8), new Vector2(20, -4), new Vector2(32, 8), new Vector2(-16, 10)
        }, -1);

        AddProp("FallenPost", new Vector2(430, 162), new Color("#2b1b16"), new[]
        {
            new Vector2(-40, 6), new Vector2(30, -4), new Vector2(34, 2), new Vector2(-36, 12)
        }, 1);

        AddProp("CorpseSilhouette", new Vector2(-40, 168), new Color("#15110f", 0.7f), new[]
        {
            new Vector2(-18, 4), new Vector2(-12, -6), new Vector2(8, -4), new Vector2(22, 6), new Vector2(-6, 8)
        }, 0);

        AddProp("BrokenPot", new Vector2(200, 192), new Color("#4a3428"), new[]
        {
            new Vector2(-10, 0), new Vector2(-4, -8), new Vector2(8, -6), new Vector2(12, 2), new Vector2(-6, 4)
        }, 1);
    }

    private void AddProp(string name, Vector2 position, Color color, Vector2[] polygon, int zOffset)
    {
        if (HasNode(name))
        {
            return;
        }

        AddChild(new Polygon2D
        {
            Name = name,
            Color = color,
            Position = position,
            Polygon = polygon,
            ZIndex = Mathf.RoundToInt(position.Y) + zOffset
        });
    }

    private void CacheEmberLights()
    {
        _emberLights.Clear();
        foreach (var name in new[] { "HutEmberLightLeft", "HutEmberLightRight", "BloodMoonCore" })
        {
            if (GetNodeOrNull<Polygon2D>(name) is { } ember)
            {
                _emberLights.Add(ember);
            }
        }
    }

    private void PulseEmberLights()
    {
        for (var i = 0; i < _emberLights.Count; i++)
        {
            var pulse = 0.72f + Mathf.Sin(_ambientTime * 2.4f + i * 1.3f) * 0.18f;
            _emberLights[i].Modulate = new Color(pulse, pulse * 0.92f, pulse * 0.78f, 1f);
        }
    }
}
