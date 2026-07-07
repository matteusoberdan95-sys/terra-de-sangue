using Godot;

[GlobalClass]
public partial class ArrowProjectile : Area2D
{
    private const float Speed = 420f;
    private const float MaxRange = 220f;
    private const float DepthTolerance = 14f;

    private Vector2 _direction = Vector2.Right;
    private float _travelled;
    private bool _resolved;

    public override void _Ready()
    {
        CollisionLayer = 0;
        CollisionMask = EnemyBase.HurtboxCollisionLayer;
        Monitoring = true;
        Monitorable = false;
        AddChild(new CollisionShape2D
        {
            Shape = new RectangleShape2D { Size = new Vector2(10f, 4f) }
        });

        var shaft = new Polygon2D
        {
            Color = new Color("#6a4a2a"),
            Polygon = new[] { new Vector2(-8, -1), new Vector2(8, -1), new Vector2(8, 1), new Vector2(-8, 1) }
        };
        AddChild(shaft);

        var tip = new Polygon2D
        {
            Position = new Vector2(8, 0),
            Color = new Color("#8a8a8a"),
            Polygon = new[] { new Vector2(0, -2), new Vector2(6, 0), new Vector2(0, 2) }
        };
        AddChild(tip);

        AreaEntered += OnAreaEntered;
    }

    public void Launch(Vector2 origin, Vector2 direction)
    {
        GlobalPosition = origin;
        _direction = direction.LengthSquared() > 0.01f ? direction.Normalized() : Vector2.Right;
        Rotation = _direction.Angle();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_resolved)
        {
            return;
        }

        var step = _direction * Speed * (float)delta;
        GlobalPosition += step;
        _travelled += step.Length();

        if (_travelled >= MaxRange)
        {
            ResolveMiss();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (_resolved || area.GetParent() is not EnemyBase enemy || !enemy.IsAlive)
        {
            return;
        }

        if (Mathf.Abs(enemy.GlobalPosition.Y - GlobalPosition.Y) > DepthTolerance)
        {
            return;
        }

        _resolved = true;
        Monitoring = false;
        var impulse = _direction * 72f + new Vector2(0, -6f);
        enemy.TakeHit(impulse, PlayerAttackKind.Light);
        enemy.ApplyBleed(BleedLevel.Light);
        CombatAudio.Get(this)?.PlayArrowHit();
        QueueFree();
    }

    private void ResolveMiss()
    {
        _resolved = true;
        QueueFree();
    }
}
