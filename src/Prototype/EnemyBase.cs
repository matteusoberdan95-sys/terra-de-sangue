using Godot;

[GlobalClass]
public abstract partial class EnemyBase : CharacterBody2D
{
    public const uint HurtboxCollisionLayer = 1u << 2;
    public const uint AttackHitboxCollisionLayer = 1u << 4;

    private const float Friction = 9f;
    private const float DepthTolerance = 12f;
    private const float AttackStartupSeconds = 0.22f;
    private const float AttackActiveSeconds = 0.1f;
    private const float AttackRecoverySeconds = 0.28f;
    private static readonly Rect2 DefaultMovementBounds = new(new Vector2(-380, 124), new Vector2(760, 92));

    private static EnemyBase? _activeAttacker;

    protected enum EnemyState
    {
        Idle,
        Approach,
        Attack,
        HitStun,
        Dead
    }

    private Polygon2D? _body;
    private Polygon2D? _mark;
    private Polygon2D? _attackSwing;
    private Area2D? _attackHitbox;
    private CollisionShape2D? _attackHitboxShape;
    private EnemyState _state = EnemyState.Idle;
    private Rect2 _movementBounds = DefaultMovementBounds;
    private int _health;
    private float _hitFlash;
    private float _hitStunTimer;
    private float _attackCooldown;
    private float _attackTimer;
    private Vector2 _facing = Vector2.Left;
    private bool _hitPlayerThisAttack;

    public Rect2 MovementBounds
    {
        get => _movementBounds;
        set => _movementBounds = value;
    }

    public bool IsAlive => _state != EnemyState.Dead;

    protected abstract int MaxHealth { get; }
    protected abstract float ApproachSpeed { get; }
    protected abstract float AttackRange { get; }
    protected abstract float AttackCooldownSeconds { get; }
    protected abstract int AttackDamage { get; }
    protected abstract Color BodyColor { get; }
    protected abstract Color ApproachColor { get; }
    protected abstract Color AttackColor { get; }

    public override void _Ready()
    {
        AddToGroup("enemy");
        _health = MaxHealth;
        BuildVisuals();
        BuildCollision();
        BuildHurtbox();
        BuildAttackHitbox();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _attackCooldown = Mathf.Max(0f, _attackCooldown - dt);
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

        if (_state == EnemyState.Attack)
        {
            ReleaseAttackSlot();
            SetAttackHitboxEnabled(false);
        }

        _health -= 1;
        _hitFlash = 0.1f;
        _hitStunTimer = 0.18f;
        _state = _health <= 0 ? EnemyState.Dead : EnemyState.HitStun;
        Velocity = impulse;

        if (_health <= 0)
        {
            ReleaseAttackSlot();
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

        if (_state == EnemyState.Attack)
        {
            UpdateAttack(dt);
            Velocity = Vector2.Zero;
            return;
        }

        if (_hitStunTimer > 0f)
        {
            _hitStunTimer = Mathf.Max(0f, _hitStunTimer - dt);
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            if (_hitStunTimer <= 0f && _state == EnemyState.HitStun)
            {
                _state = EnemyState.Idle;
            }

            return;
        }

        var player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (player is null || !player.IsAlive)
        {
            _state = EnemyState.Idle;
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            return;
        }

        var toPlayer = player.GlobalPosition - GlobalPosition;
        _facing = toPlayer.X < -2f ? Vector2.Left : toPlayer.X > 2f ? Vector2.Right : _facing;

        var inAttackRange = Mathf.Abs(toPlayer.X) <= AttackRange && Mathf.Abs(toPlayer.Y) <= DepthTolerance;
        if (inAttackRange && _attackCooldown <= 0f && TryClaimAttackSlot())
        {
            StartAttack();
            return;
        }

        var shouldApproach = Mathf.Abs(toPlayer.X) > AttackRange * 0.85f || Mathf.Abs(toPlayer.Y) > DepthTolerance * 0.8f;
        if (!shouldApproach)
        {
            _state = EnemyState.Idle;
            Velocity = Velocity.Lerp(Vector2.Zero, Friction * dt);
            return;
        }

        _state = EnemyState.Approach;
        var desired = new Vector2(
            Mathf.Sign(toPlayer.X) * (Mathf.Abs(toPlayer.X) > AttackRange * 0.85f ? 1f : 0f),
            Mathf.Sign(toPlayer.Y) * (Mathf.Abs(toPlayer.Y) > DepthTolerance * 0.8f ? 1f : 0f));
        Velocity = desired.Normalized() * ApproachSpeed;
    }

    private void StartAttack()
    {
        _state = EnemyState.Attack;
        _attackCooldown = AttackCooldownSeconds;
        _attackTimer = AttackStartupSeconds + AttackActiveSeconds + AttackRecoverySeconds;
        _hitPlayerThisAttack = false;
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
            ResolveAttackHitbox();
        }

        if (_attackTimer <= 0f)
        {
            SetAttackHitboxEnabled(false);
            ReleaseAttackSlot();
            _state = EnemyState.Idle;
        }
    }

    private void ResolveAttackHitbox()
    {
        if (_attackHitbox is null)
        {
            return;
        }

        _attackHitbox.Position = new Vector2(_facing.X * 30f, -6f);
        _attackHitbox.ForceUpdateTransform();

        if (_hitPlayerThisAttack)
        {
            return;
        }

        foreach (var area in _attackHitbox.GetOverlappingAreas())
        {
            if (area.GetParent() is not PlayerController player || !player.IsAlive)
            {
                continue;
            }

            _hitPlayerThisAttack = true;
            player.TakeHit(new Vector2(_facing.X * 72f, -6f), AttackDamage);
            GetParent<PrototypeArena>()?.ApplyCombatImpact(3f, 0.03f);
            break;
        }
    }

    private bool TryClaimAttackSlot()
    {
        if (_activeAttacker is not null && GodotObject.IsInstanceValid(_activeAttacker) && _activeAttacker.IsAlive)
        {
            return false;
        }

        _activeAttacker = this;
        return true;
    }

    private void ReleaseAttackSlot()
    {
        if (_activeAttacker == this)
        {
            _activeAttacker = null;
        }
    }

    private void BuildVisuals()
    {
        _body = GetNodeOrNull<Polygon2D>("InvaderPlaceholder");
        _mark = GetNodeOrNull<Polygon2D>("RedMark");
        _attackSwing = GetNodeOrNull<Polygon2D>("AttackSwing");

        if (_body is null)
        {
            _body = new Polygon2D
            {
                Name = "InvaderPlaceholder",
                Polygon = new[]
                {
                    new Vector2(-12, 12),
                    new Vector2(-13, -8),
                    new Vector2(-4, -26),
                    new Vector2(9, -18),
                    new Vector2(14, -4),
                    new Vector2(11, 12),
                    new Vector2(0, 16)
                }
            };
            AddChild(_body);
        }

        if (_mark is null)
        {
            _mark = new Polygon2D
            {
                Name = "RedMark",
                Color = new Color("#560f0b"),
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

        if (_attackSwing is null)
        {
            _attackSwing = new Polygon2D
            {
                Name = "AttackSwing",
                Color = new Color("#8f1f17"),
                Visible = false,
                Polygon = new[]
                {
                    new Vector2(8, -12),
                    new Vector2(44, -8),
                    new Vector2(40, 6),
                    new Vector2(10, 2)
                }
            };
            AddChild(_attackSwing);
        }
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

    private void BuildAttackHitbox()
    {
        _attackHitbox = GetNodeOrNull<Area2D>("AttackHitbox");
        _attackHitboxShape = _attackHitbox?.GetNodeOrNull<CollisionShape2D>("AttackHitboxShape");
        if (_attackHitbox is not null && _attackHitboxShape is not null)
        {
            SetAttackHitboxEnabled(false);
            return;
        }

        _attackHitboxShape = new CollisionShape2D
        {
            Name = "AttackHitboxShape",
            Disabled = true,
            Shape = new RectangleShape2D
            {
                Size = new Vector2(36, 22)
            }
        };

        _attackHitbox = new Area2D
        {
            Name = "AttackHitbox",
            Monitoring = true,
            Monitorable = false,
            CollisionLayer = AttackHitboxCollisionLayer,
            CollisionMask = PlayerController.HurtboxCollisionLayer,
            Position = new Vector2(-30, -6)
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

        if (_attackSwing is not null)
        {
            _attackSwing.Visible = enabled;
            _attackSwing.Scale = new Vector2(_facing.X, 1f);
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
                EnemyState.Dead => new Color("#332424"),
                _ when _hitFlash > 0f => new Color("#f0d7aa"),
                EnemyState.Attack => AttackColor,
                EnemyState.Approach => ApproachColor,
                _ => BodyColor
            };
        }

        if (_mark is not null)
        {
            _mark.Visible = _state != EnemyState.Dead;
        }
    }
}
