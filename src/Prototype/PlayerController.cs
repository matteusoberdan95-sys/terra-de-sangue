using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerController : CharacterBody2D
{
    private const float Speed = 120f;
    private const float AttackCooldownSeconds = 0.34f;
    private const float AttackStartupSeconds = 0.06f;
    private const float AttackActiveSeconds = 0.09f;
    private const float AttackRecoverySeconds = 0.16f;
    private static readonly Rect2 DefaultMovementBounds = new(new Vector2(-380, 124), new Vector2(760, 92));

    private enum PlayerState
    {
        Idle,
        Walk,
        LightAttack
    }

    private Polygon2D? _body;
    private Polygon2D? _paint;
    private Polygon2D? _attackSlash;
    private Area2D? _attackHitbox;
    private CollisionShape2D? _attackHitboxShape;
    private readonly HashSet<ulong> _hitEnemiesThisAttack = new();
    private PlayerState _state = PlayerState.Idle;
    private Vector2 _facing = Vector2.Right;
    private Rect2 _movementBounds = DefaultMovementBounds;
    private float _attackCooldown;
    private float _slashTimer;
    private float _attackTimer;

    public Rect2 MovementBounds
    {
        get => _movementBounds;
        set => _movementBounds = value;
    }

    public override void _Ready()
    {
        AddToGroup("player");
        EnsureInputMap();
        BuildVisuals();
        BuildCollision();
        BuildAttackHitbox();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _attackCooldown = Mathf.Max(0f, _attackCooldown - dt);
        _slashTimer = Mathf.Max(0f, _slashTimer - dt);

        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (_state != PlayerState.LightAttack && input.LengthSquared() > 0.01f)
        {
            _facing = input.X < -0.1f ? Vector2.Left : input.X > 0.1f ? Vector2.Right : _facing;
        }

        if (_state == PlayerState.LightAttack)
        {
            UpdateAttack(dt);
            Velocity = Vector2.Zero;
        }
        else
        {
            Velocity = input.Normalized() * Speed;
            _state = input.LengthSquared() > 0.01f ? PlayerState.Walk : PlayerState.Idle;
        }

        MoveAndSlide();
        ClampToMovementBounds();
        ZIndex = Mathf.RoundToInt(GlobalPosition.Y);

        if (Input.IsActionJustPressed("attack_light") && _attackCooldown <= 0f)
        {
            StartLightAttack();
        }

        if (_attackSlash is not null)
        {
            _attackSlash.Visible = _slashTimer > 0f;
            _attackSlash.Scale = new Vector2(_facing.X, 1f);
        }

        UpdateVisualState();
    }

    private void StartLightAttack()
    {
        _state = PlayerState.LightAttack;
        _attackCooldown = AttackCooldownSeconds;
        _attackTimer = AttackStartupSeconds + AttackActiveSeconds + AttackRecoverySeconds;
        _slashTimer = AttackActiveSeconds + AttackRecoverySeconds;
        _hitEnemiesThisAttack.Clear();
        SetAttackHitboxEnabled(false);
    }

    private void UpdateAttack(float dt)
    {
        _attackTimer = Mathf.Max(0f, _attackTimer - dt);

        var activeStartsAt = AttackRecoverySeconds;
        var activeEndsAt = AttackRecoverySeconds + AttackActiveSeconds;
        var isActive = _attackTimer > activeStartsAt && _attackTimer <= activeEndsAt;

        SetAttackHitboxEnabled(isActive);
        if (isActive)
        {
            ResolveActiveHitbox();
        }

        if (_attackTimer <= 0f)
        {
            SetAttackHitboxEnabled(false);
            _state = PlayerState.Idle;
        }
    }

    private void ResolveActiveHitbox()
    {
        if (_attackHitbox is null)
        {
            return;
        }

        _attackHitbox.Position = new Vector2(_facing.X * 34f, -8f);
        _attackHitbox.ForceUpdateTransform();

        foreach (var area in _attackHitbox.GetOverlappingAreas())
        {
            if (area.GetParent() is not EnemyDummy enemy || _hitEnemiesThisAttack.Contains(enemy.GetInstanceId()))
            {
                continue;
            }

            _hitEnemiesThisAttack.Add(enemy.GetInstanceId());
            enemy.TakeHit(new Vector2(_facing.X * 96f, -10f));
            GetParent<PrototypeArena>()?.ApplyCombatImpact(4.5f, 0.04f);
        }
    }

    private void BuildVisuals()
    {
        _body = GetNodeOrNull<Polygon2D>("WarriorPlaceholder");
        _paint = GetNodeOrNull<Polygon2D>("Paint");
        _attackSlash = GetNodeOrNull<Polygon2D>("AttackSlash");

        if (_body is not null && _paint is not null && _attackSlash is not null)
        {
            return;
        }

        _body = new Polygon2D
        {
            Name = "WarriorPlaceholder",
            Color = new Color("#c46f35"),
            Polygon = new[]
            {
                new Vector2(-10, 12),
                new Vector2(-7, -18),
                new Vector2(0, -30),
                new Vector2(10, -18),
                new Vector2(12, 12)
            }
        };
        AddChild(_body);

        _paint = new Polygon2D
        {
            Name = "Paint",
            Color = new Color("#e0b75d"),
            Polygon = new[]
            {
                new Vector2(-7, -10),
                new Vector2(8, -14),
                new Vector2(7, -9),
                new Vector2(-8, -5)
            }
        };
        AddChild(_paint);

        _attackSlash = new Polygon2D
        {
            Name = "AttackSlash",
            Color = new Color("#b51f1f"),
            Visible = false,
            Polygon = new[]
            {
                new Vector2(10, -16),
                new Vector2(58, -5),
                new Vector2(54, 8),
                new Vector2(12, 3)
            }
        };
        AddChild(_attackSlash);
    }

    private void BuildCollision()
    {
        if (GetNodeOrNull<CollisionShape2D>("Collision") is not null)
        {
            return;
        }

        var shape = new CollisionShape2D
        {
            Name = "Collision",
            Shape = new RectangleShape2D
            {
                Size = new Vector2(18, 28)
            },
            Position = new Vector2(0, -4)
        };

        AddChild(shape);
    }

    private void BuildAttackHitbox()
    {
        _attackHitbox = GetNodeOrNull<Area2D>("LightAttackHitbox");
        _attackHitboxShape = _attackHitbox?.GetNodeOrNull<CollisionShape2D>("LightAttackShape");
        if (_attackHitbox is not null && _attackHitboxShape is not null)
        {
            SetAttackHitboxEnabled(false);
            return;
        }

        _attackHitboxShape = new CollisionShape2D
        {
            Name = "LightAttackShape",
            Disabled = true,
            Shape = new RectangleShape2D
            {
                Size = new Vector2(44, 24)
            }
        };

        _attackHitbox = new Area2D
        {
            Name = "LightAttackHitbox",
            Monitoring = true,
            Monitorable = false,
            CollisionLayer = 0,
            CollisionMask = EnemyDummy.HurtboxCollisionLayer,
            Position = new Vector2(34, -8)
        };
        _attackHitbox.AddChild(_attackHitboxShape);
        AddChild(_attackHitbox);
    }

    private void SetAttackHitboxEnabled(bool enabled)
    {
        if (_attackHitboxShape is not null)
        {
            _attackHitboxShape.Disabled = !enabled;
        }
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
                PlayerState.LightAttack => new Color("#d4843e"),
                PlayerState.Walk => new Color("#c46f35"),
                _ => new Color("#a85f34")
            };
        }

        if (_paint is not null)
        {
            _paint.Color = _state == PlayerState.LightAttack ? new Color("#f0d06a") : new Color("#e0b75d");
        }
    }

    private static void EnsureInputMap()
    {
        AddKeyAction("move_left", Key.A);
        AddKeyAction("move_right", Key.D);
        AddKeyAction("move_up", Key.W);
        AddKeyAction("move_down", Key.S);
        AddKeyAction("attack_light", Key.J);
    }

    private static void AddKeyAction(string action, Key key)
    {
        if (!InputMap.HasAction(action))
        {
            InputMap.AddAction(action);
        }

        foreach (var existingEvent in InputMap.ActionGetEvents(action))
        {
            if (existingEvent is InputEventKey existingKey && existingKey.Keycode == key)
            {
                return;
            }
        }

        InputMap.ActionAddEvent(action, new InputEventKey { Keycode = key });
    }
}
