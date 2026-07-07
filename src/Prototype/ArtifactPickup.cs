using Godot;

[GlobalClass]
public partial class ArtifactPickup : Area2D
{
    private const uint PlayerBodyLayer = 1u;

    private bool _collected;
    private float _pulseTime;

    public ArtifactKind Kind { get; set; } = ArtifactKind.IronKnife;

    public int Uses { get; set; } = 3;

    public override void _Ready()
    {
        AddToGroup("artifact_pickup");
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
        Modulate = new Color(1f, 0.92f, 0.86f, 0.8f + Mathf.Sin(_pulseTime * 4f) * 0.15f);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_collected || body is not PlayerController player)
        {
            return;
        }

        _collected = true;
        Monitoring = false;
        player.GrantArtifact(Kind, Uses);
        CombatAudio.Get(this)?.PlayArtifactPickup();
        QueueFree();
    }

    private void BuildVisuals()
    {
        AddChild(new CollisionShape2D
        {
            Shape = new CircleShape2D { Radius = 16f }
        });

        if (Kind == ArtifactKind.BrokenClub)
        {
            AddChild(new Polygon2D
            {
                Color = new Color("#5a4030"),
                Polygon = new[]
                {
                    new Vector2(-4, -12),
                    new Vector2(8, -8),
                    new Vector2(12, 10),
                    new Vector2(-6, 12)
                }
            });

            AddChild(new Polygon2D
            {
                Position = new Vector2(6, -10),
                Color = new Color("#7a7a7a", 0.85f),
                Polygon = new[]
                {
                    new Vector2(0, -6),
                    new Vector2(10, 0),
                    new Vector2(0, 6),
                    new Vector2(-4, 0)
                }
            });
            return;
        }

        AddChild(new Polygon2D
        {
            Color = new Color("#6a6a72"),
            Polygon = new[]
            {
                new Vector2(-2, -10),
                new Vector2(10, 0),
                new Vector2(-2, 10),
                new Vector2(-8, 0)
            }
        });

        AddChild(new Polygon2D
        {
            Color = new Color("#8f1f1f", 0.7f),
            Polygon = new[]
            {
                new Vector2(2, -4),
                new Vector2(8, 0),
                new Vector2(2, 4),
                new Vector2(0, 0)
            }
        });
    }
}
