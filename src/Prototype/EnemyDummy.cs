using Godot;

[GlobalClass]
public partial class EnemyDummy : CharacterBody2D
{
    public const uint HurtboxCollisionLayer = 1u << 2;

    private const float Friction = 9f;
    private const float ApproachSpeed = 42f;
    private const float HitStunSeconds = 0.18f;
    private static readonly Rect2 DefaultMovementBounds = new(new Vector2(-380, 124), new Vector2(760, 92));

    private enum EnemyState
    {
        Idle,
        Approach,
        HitStun,
        Dead
    }

    private Polygon2D? _body;
    private Polygon2D? _mark;
    private EnemyState _state = EnemyState.Idle;
    private Rect2 _movementBounds = DefaultMovementBounds;
    private int _health = 3;
    private float _hitFlash;
    private float _hitStunTimer;

    public Rect2 MovementBounds
    {
        get => _movementBounds;
        set => _movementBounds = value;
    }

    public override void _Ready()
    {
        AddToGroup("enemy");
        BuildVisuals();
        BuildCollision();
        BuildHurtbox();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        UpdateState(dt);
        MoveAndSlide();
        ClampToMovementBounds();
        ZIndex = Mathf.RoundToInt(GlobalPosition.Y);

        _hitFlash = Mathf.Max(0f, _hitFlash - dt);
        UpdateVisualState();
    }

    public void TakeHit(Vector2 impulse)
    {
        if (_state == EnemyState.Dead)
        {
            return;
        }

        _health -= 1;
        _hitFlash = 0.1f;
        _hitStunTimer = HitStunSeconds;
        _state = _health <= 0 ? EnemyState.Dead : EnemyState.HitStun;
        Velocity = impulse;

        if (_health <= 0)
        {
            ZIndex += 1;
            GetTree().CreateTimer(0.22).Timeout += QueueFree;
        }
    }

    private void UpdateState(float dt)
    {
        if (_state == EnemyState.Dead)
        {
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            return;
        }

        if (_hitStunTimer > 0f)
        {
            _hitStunTimer = Mathf.Max(0f, _hitStunTimer - dt);
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            return;
        }

        var player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (player is null)
        {
            _state = EnemyState.Idle;
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            return;
        }

        var toPlayer = player.GlobalPosition - GlobalPosition;
        var shouldApproach = Mathf.Abs(toPlayer.X) > 34f || Mathf.Abs(toPlayer.Y) > 10f;
        if (!shouldApproach)
        {
            _state = EnemyState.Idle;
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            return;
        }

        _state = EnemyState.Approach;
        var desired = new Vector2(
            Mathf.Sign(toPlayer.X) * (Mathf.Abs(toPlayer.X) > 34f ? 1f : 0f),
            Mathf.Sign(toPlayer.Y) * (Mathf.Abs(toPlayer.Y) > 10f ? 1f : 0f));
        Velocity = desired.Normalized() * ApproachSpeed;
    }

    private void BuildVisuals()
    {
        _body = GetNodeOrNull<Polygon2D>("InvaderPlaceholder");
        _mark = GetNodeOrNull<Polygon2D>("RedMark");

        if (_body is not null && _mark is not null)
        {
            return;
        }

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

        _mark = new Polygon2D
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
        };
        AddChild(_mark);
    }

    private void BuildCollision()
    {
        if (GetNodeOrNull<CollisionShape2D>("Collision") is not null)
        {
            return;
        }

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

    private void BuildHurtbox()
    {
        if (GetNodeOrNull<Area2D>("Hurtbox") is not null)
        {
            return;
        }

        var hurtbox = new Area2D
        {
            Name = "Hurtbox",
            CollisionLayer = HurtboxCollisionLayer,
            CollisionMask = 0,
            Monitorable = true,
            Monitoring = false
        };

        hurtbox.AddChild(new CollisionShape2D
        {
            Name = "HurtboxShape",
            Shape = new RectangleShape2D
            {
                Size = new Vector2(24, 34)
            },
            Position = new Vector2(0, -6)
        });

        AddChild(hurtbox);
    }

    private void ClampToMovementBounds()
    {
        GlobalPosition = new Vector2(
            Mathf.Clamp(GlobalPosition.X, _movementBounds.Position.X, _movementBounds.End.X),
            Mathf.Clamp(GlobalPosition.Y, _movementBounds.Position.Y, _movementBounds.End.Y));
    }

    private void UpdateVisualState()
    {
        if (_body is not null)
        {
            _body.Color = _state switch
            {
                EnemyState.Dead => new Color("#332424"),
                _ when _hitFlash > 0f => new Color("#f0d7aa"),
                EnemyState.Approach => new Color("#707a80"),
                _ => new Color("#5f6970")
            };
        }

        if (_mark is not null)
        {
            _mark.Visible = _state != EnemyState.Dead;
        }
    }
}
