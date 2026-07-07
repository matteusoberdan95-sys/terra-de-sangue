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
    private Polygon2D? _head;
    private Polygon2D? _arm;
    private Polygon2D? _mark;
    private Polygon2D? _tornCloth;
    private Polygon2D? _bleedingWound;
    private Polygon2D? _deepGash;
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

    protected Vector2 FacingDirection => _facing;
    private bool _hitPlayerThisAttack;

    public Rect2 MovementBounds
    {
        get => _movementBounds;
        set => _movementBounds = value;
    }

    public bool IsAlive => _state != EnemyState.Dead;

    public int HealthRemaining => _health;

    public bool CanBeExecuted => IsAlive && _health == 1;

    public int MaxHealth => MaxHealthValue;

    protected abstract int MaxHealthValue { get; }
    protected abstract float ApproachSpeed { get; }
    protected abstract float AttackRange { get; }
    protected abstract float AttackCooldownSeconds { get; }
    protected abstract int AttackDamage { get; }
    protected abstract Color BodyColor { get; }
    protected abstract Color ApproachColor { get; }
    protected abstract Color AttackColor { get; }

    protected virtual float GetAttackStartup() => AttackStartupSeconds;
    protected virtual float GetAttackActive() => AttackActiveSeconds;
    protected virtual float GetAttackRecovery() => AttackRecoverySeconds;
    protected virtual int GetAttackDamageAmount() => AttackDamage;
    protected virtual Vector2 GetAttackHitboxSize() => new(36, 22);
    protected virtual float GetAttackReach() => 30f;
    protected virtual void OnAttackPatternStarted() { }
    protected virtual void OnAttackActiveFrame(float dt) { }

    public override void _Ready()
    {
        AddToGroup("enemy");
        _health = MaxHealthValue;
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

    public void TakeHit(Vector2 impulse, PlayerAttackKind attackKind = PlayerAttackKind.Light)
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

        ImpactFeedback.Get(this)?.OnEnemyHit(GlobalPosition + new Vector2(0, -4), impulse, _health <= 0, attackKind);
        UpdateWoundVisuals();

        if (_health <= 0)
        {
            ReleaseAttackSlot();
            ApplyDeathFeedback(attackKind);
            ZIndex += 1;
            GetTree().CreateTimer(0.38).Timeout += QueueFree;
        }
    }

    public void Execute(Vector2 impulse, ExecutionStyle style)
    {
        if (!CanBeExecuted)
        {
            return;
        }

        if (_state == EnemyState.Attack)
        {
            ReleaseAttackSlot();
            SetAttackHitboxEnabled(false);
        }

        _health = 0;
        _state = EnemyState.Dead;
        Velocity = impulse;
        ImpactFeedback.Get(this)?.OnExecution(GlobalPosition + new Vector2(0, -4), impulse, style);
        ReleaseAttackSlot();
        ApplyDeathFeedback(PlayerAttackKind.Execute, style);
        Modulate = new Color("#5a2020");
        ZIndex += 1;
        GetTree().CreateTimer(0.5).Timeout += QueueFree;
    }

    private void ApplyDeathFeedback(PlayerAttackKind attackKind, ExecutionStyle? executionStyle = null)
    {
        Modulate = new Color("#8a4a4a");
        Rotation = _facing.X < 0f ? 0.28f : -0.28f;
        Scale *= new Vector2(1.04f, 0.82f);

        if (executionStyle == ExecutionStyle.Decapitate || attackKind == PlayerAttackKind.ComboFinisher)
        {
            if (_head is not null)
            {
                _head.Visible = false;
            }
        }

        if (attackKind is PlayerAttackKind.Light or PlayerAttackKind.Heavy)
        {
            if (_arm is not null)
            {
                _arm.Visible = false;
            }
        }

        if (executionStyle == ExecutionStyle.GutRip)
        {
            if (_body is not null)
            {
                _body.Color = new Color("#4a1814");
            }
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
        OnAttackPatternStarted();
        _attackTimer = GetAttackStartup() + GetAttackActive() + GetAttackRecovery();
        _hitPlayerThisAttack = false;
        SetAttackHitboxEnabled(false);
    }

    private void UpdateAttack(float dt)
    {
        _attackTimer = Mathf.Max(0f, _attackTimer - dt);

        var recovery = GetAttackRecovery();
        var active = GetAttackActive();
        var activeStartsAt = recovery;
        var activeEndsAt = recovery + active;
        var isActive = _attackTimer > activeStartsAt && _attackTimer <= activeEndsAt;

        SetAttackHitboxEnabled(isActive);
        if (isActive)
        {
            OnAttackActiveFrame(dt);
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
        if (_attackHitbox is null || _attackHitboxShape?.Shape is not RectangleShape2D rectangle)
        {
            return;
        }

        var size = GetAttackHitboxSize();
        rectangle.Size = size;
        _attackHitbox.Position = new Vector2(_facing.X * GetAttackReach(), -6f);
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
            player.TakeHit(new Vector2(_facing.X * 72f, -6f), GetAttackDamageAmount());
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

        if (_tornCloth is null)
        {
            _tornCloth = new Polygon2D
            {
                Name = "TornCloth",
                Visible = false,
                Color = new Color("#560f0b", 0.72f),
                Polygon = new[]
                {
                    new Vector2(-9, 0),
                    new Vector2(7, -4),
                    new Vector2(11, 6),
                    new Vector2(-6, 9)
                }
            };
            AddChild(_tornCloth);
        }

        if (_bleedingWound is null)
        {
            _bleedingWound = new Polygon2D
            {
                Name = "BleedingWound",
                Visible = false,
                Color = new Color("#8f1f17", 0.88f),
                Polygon = new[]
                {
                    new Vector2(-5, -6),
                    new Vector2(6, -8),
                    new Vector2(4, 2),
                    new Vector2(-7, 0)
                }
            };
            AddChild(_bleedingWound);
        }

        if (_deepGash is null)
        {
            _deepGash = new Polygon2D
            {
                Name = "DeepGash",
                Visible = false,
                Color = new Color("#5a0f0c", 0.92f),
                Polygon = new[]
                {
                    new Vector2(-3, -10),
                    new Vector2(5, -12),
                    new Vector2(3, 4),
                    new Vector2(-5, 2)
                }
            };
            AddChild(_deepGash);
        }

        if (_head is null)
        {
            _head = new Polygon2D
            {
                Name = "Head",
                Color = new Color("#6a3a34"),
                Polygon = new[]
                {
                    new Vector2(-6, -18),
                    new Vector2(0, -26),
                    new Vector2(7, -18),
                    new Vector2(5, -12),
                    new Vector2(-5, -12)
                }
            };
            AddChild(_head);
        }

        if (_arm is null)
        {
            _arm = new Polygon2D
            {
                Name = "Arm",
                Color = BodyColor.Darkened(0.85f),
                Polygon = new[]
                {
                    new Vector2(10, -6),
                    new Vector2(16, -4),
                    new Vector2(14, 6),
                    new Vector2(8, 4)
                }
            };
            AddChild(_arm);
        }
    }

    private void UpdateWoundVisuals()
    {
        var ratio = _health / (float)MaxHealthValue;

        if (_tornCloth is not null)
        {
            _tornCloth.Visible = _state != EnemyState.Dead && ratio < 0.75f;
        }

        if (_bleedingWound is not null)
        {
            _bleedingWound.Visible = _state != EnemyState.Dead && ratio <= 0.5f;
        }

        if (_deepGash is not null)
        {
            _deepGash.Visible = _state != EnemyState.Dead && ratio <= 0.34f;
        }

        if (_arm is not null && _state != EnemyState.Dead && ratio <= 0.25f)
        {
            _arm.Rotation = 0.35f;
            _arm.Modulate = new Color("#9a6a64");
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
            var woundedBody = BodyColor.Lerp(new Color("#6a3a34"), 1f - _health / (float)MaxHealthValue);
            _body.Color = _state switch
            {
                EnemyState.Dead => new Color("#332424"),
                _ when _hitFlash > 0f => new Color("#f0d7aa"),
                EnemyState.Attack => AttackColor,
                EnemyState.Approach => ApproachColor,
                _ => woundedBody
            };
        }

        if (_mark is not null)
        {
            _mark.Visible = _state != EnemyState.Dead;
        }

        if (_head is not null)
        {
            _head.Visible = _state != EnemyState.Dead;
        }

        UpdateWoundVisuals();
    }
}
