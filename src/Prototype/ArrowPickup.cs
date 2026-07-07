using Godot;

[GlobalClass]
public partial class ArrowPickup : Area2D
{
    private const uint PlayerBodyLayer = 1u;

    private bool _collected;
    private float _pulseTime;

    public int ArrowAmount { get; set; } = 1;

    public override void _Ready()
    {
        AddToGroup("arrow_pickup");
        CollisionLayer = 0;
        CollisionMask = PlayerBodyLayer;
        Monitoring = true;
        BodyEntered += OnBodyEntered;
        BuildVisuals();
    }

    public override void _Process(double delta)
    {
        if (_collected)
        {
            return;
        }

        _pulseTime += (float)delta;
        Modulate = new Color(1f, 1f, 1f, 0.75f + Mathf.Sin(_pulseTime * 5f) * 0.2f);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_collected || body is not PlayerController player)
        {
            return;
        }

        _collected = true;
        Monitoring = false;
        player.AddArrows(ArrowAmount);
        CombatAudio.Get(this)?.PlayArrowPickup();
        QueueFree();
    }

    private void BuildVisuals()
    {
        AddChild(new CollisionShape2D
        {
            Shape = new CircleShape2D { Radius = 16f }
        });

        var bundle = ArrowAmount >= 3;
        AddChild(new Polygon2D
        {
            Color = new Color("#5a3d22"),
            Polygon = bundle
                ? new[] { new Vector2(-10, -8), new Vector2(10, -8), new Vector2(12, 8), new Vector2(-12, 8) }
                : new[] { new Vector2(-8, -1), new Vector2(8, -1), new Vector2(8, 1), new Vector2(-8, 1) }
        });

        AddChild(new Polygon2D
        {
            Position = new Vector2(bundle ? 10 : 6, 0),
            Color = new Color("#9a9a9a"),
            Polygon = new[] { new Vector2(0, -2), new Vector2(5, 0), new Vector2(0, 2) }
        });
    }
}
