using Godot;

[GlobalClass]
public partial class MemoryPickup : Area2D
{
    private const uint PlayerBodyLayer = 1u;

    private Polygon2D? _glow;
    private Polygon2D? _maskShardA;
    private Polygon2D? _maskShardB;
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
        if (_collected)
        {
            return;
        }

        _pulseTime += (float)delta;
        var pulse = 0.55f + Mathf.Sin(_pulseTime * 4f) * 0.2f;

        if (_glow is not null)
        {
            _glow.Modulate = new Color(1f, 1f, 1f, pulse);
        }

        if (_maskShardA is not null)
        {
            _maskShardA.Rotation = Mathf.Sin(_pulseTime * 2f) * 0.08f;
        }

        if (_maskShardB is not null)
        {
            _maskShardB.Rotation = -0.2f + Mathf.Sin(_pulseTime * 2.4f) * 0.1f;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_collected || body is not PlayerController)
        {
            return;
        }

        _collected = true;
        Monitoring = false;
        CombatAudio.Get(this)?.PlayMemoryCollect();
        Visible = false;

        var phaseDirector = GetTree().GetFirstNodeInGroup("phase_director") as PhaseDirector;
        phaseDirector?.OnMemoryCollected(MemoryId, MemoryTitle, MemoryText);

        foreach (var node in GetTree().GetNodesInGroup("mata_director"))
        {
            if (node is MataFechadaDirector mataDirector)
            {
                mataDirector.OnMemoryCollected(MemoryId, MemoryTitle, MemoryText);
            }
        }

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
                new Vector2(-20, 0),
                new Vector2(0, -22),
                new Vector2(20, 0),
                new Vector2(0, 22)
            }
        };
        AddChild(_glow);

        _maskShardA = new Polygon2D
        {
            Name = "MaskShardA",
            Color = new Color("#878431"),
            Polygon = new[]
            {
                new Vector2(-12, 2),
                new Vector2(-4, -14),
                new Vector2(6, -8),
                new Vector2(2, 8)
            }
        };
        AddChild(_maskShardA);

        _maskShardB = new Polygon2D
        {
            Name = "MaskShardB",
            Color = new Color("#6a5a28"),
            Polygon = new[]
            {
                new Vector2(4, -6),
                new Vector2(14, -10),
                new Vector2(10, 6),
                new Vector2(0, 10)
            }
        };
        AddChild(_maskShardB);

        AddChild(new Polygon2D
        {
            Name = "PaintTrace",
            Color = new Color("#b51f1f", 0.55f),
            Polygon = new[]
            {
                new Vector2(-6, 0),
                new Vector2(0, -4),
                new Vector2(6, 0),
                new Vector2(0, 4)
            }
        });
    }
}
