using Godot;

namespace TerraSangrada.Prototype;

public partial class EnemyDummy : CharacterBody2D
{
    private const float Friction = 9f;

    private Polygon2D? _body;
    private int _health = 3;
    private float _hitFlash;

    public override void _Ready()
    {
        AddToGroup("enemy");
        BuildVisuals();
        BuildCollision();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
        MoveAndSlide();
        ZIndex = Mathf.RoundToInt(GlobalPosition.Y);

        _hitFlash = Mathf.Max(0f, _hitFlash - dt);
        if (_body is not null)
        {
            _body.Color = _hitFlash > 0f ? new Color("#f0d7aa") : new Color("#5f6970");
        }
    }

    public void TakeHit(Vector2 impulse)
    {
        _health -= 1;
        _hitFlash = 0.08f;
        Velocity = impulse;

        if (_health <= 0)
        {
            QueueFree();
        }
    }

    private void BuildVisuals()
    {
        _body = new Polygon2D
        {
            Name = "InvaderPlaceholder",
            Color = new Color("#5f6970"),
            Polygon = new[]
            {
                new Vector2(-10, 12),
                new Vector2(-12, -12),
                new Vector2(-2, -24),
                new Vector2(11, -10),
                new Vector2(10, 12)
            }
        };
        AddChild(_body);

        AddChild(new Polygon2D
        {
            Name = "RedMark",
            Color = new Color("#8f1f17"),
            Polygon = new[]
            {
                new Vector2(-8, -8),
                new Vector2(10, -6),
                new Vector2(9, 1),
                new Vector2(-9, -1)
            }
        });
    }

    private void BuildCollision()
    {
        AddChild(new CollisionShape2D
        {
            Name = "Collision",
            Shape = new RectangleShape2D
            {
                Size = new Vector2(18, 28)
            },
            Position = new Vector2(0, -4)
        });
    }
}
