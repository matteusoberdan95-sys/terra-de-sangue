using Godot;

[GlobalClass]
public partial class MemoryPickup : Area2D
{
    private const uint PlayerBodyLayer = 1u;

    private Polygon2D? _glow;
    private bool _collected;
    private float _pulseTime;

    public string MemoryId { get; set; } = "mascara_quebrada";
    public string MemoryTitle { get; set; } = "Memoria: Mascara quebrada";
    public string MemoryText { get; set; } = "Fragmento da aldeia. Ainda guarda o rosto de alguem que nao voltou.";

    public override void _Ready()
    {
        AddToGroup("memory_pickup");
        CollisionLayer = 0;
        CollisionMask = PlayerBodyLayer;
        Monitoring = true;
        BodyEntered += OnBodyEntered;
        BuildVisuals();
    }

    public override void _Process(double delta)
    {
        if (_collected || _glow is null)
        {
            return;
        }

        _pulseTime += (float)delta;
        _glow.Modulate = new Color(1f, 1f, 1f, 0.55f + Mathf.Sin(_pulseTime * 4f) * 0.2f);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_collected || body is not PlayerController)
        {
            return;
        }

        _collected = true;
        Monitoring = false;
        Visible = false;

        var director = GetTree().GetFirstNodeInGroup("phase_director") as PhaseDirector;
        director?.OnMemoryCollected(MemoryId, MemoryTitle, MemoryText);
        QueueFree();
    }

    private void BuildVisuals()
    {
        AddChild(new CollisionShape2D
        {
            Name = "PickupShape",
            Shape = new CircleShape2D { Radius = 18f }
        });

        _glow = new Polygon2D
        {
            Name = "Glow",
            Color = new Color("#e0b75d", 0.35f),
            Polygon = new[]
            {
                new Vector2(-16, 0),
                new Vector2(0, -18),
                new Vector2(16, 0),
                new Vector2(0, 18)
            }
        };
        AddChild(_glow);

        AddChild(new Polygon2D
        {
            Name = "Shard",
            Color = new Color("#878431"),
            Polygon = new[]
            {
                new Vector2(-8, 4),
                new Vector2(-2, -10),
                new Vector2(10, -2),
                new Vector2(4, 10)
            }
        });
    }
}
