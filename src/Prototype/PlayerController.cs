using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerController : CharacterBody2D
{
    public const uint HurtboxCollisionLayer = 1u << 3;

    private const float Speed = 120f;
    private const int MaxHealth = 8;
    private const float HitStunSeconds = 0.22f;
    private const float RespawnDelaySeconds = 1.2f;
    private const float AttackCooldownSeconds = 0.32f;
    private const float AttackStartupSeconds = 0.05f;
    private const float AttackActiveSeconds = 0.1f;
    private const float AttackRecoverySeconds = 0.14f;
    private const float HeavyAttackCooldownSeconds = 0.58f;
    private const float HeavyAttackStartupSeconds = 0.12f;
    private const float HeavyAttackActiveSeconds = 0.13f;
    private const float HeavyAttackRecoverySeconds = 0.26f;
    private const float ComboWindowSeconds = 0.28f;
    private const float ExecuteRange = 42f;
    private static readonly Rect2 DefaultMovementBounds = new(new Vector2(-380, 124), new Vector2(760, 92));

    private enum PlayerState
    {
        Idle,
        Walk,
        LightAttack,
        HeavyAttack,
        HitStun,
        Dead
    }

    private Polygon2D? _body;
    private Polygon2D? _paint;
    private Polygon2D? _attackSlash;
    private Area2D? _attackHitbox;
    private Area2D? _hurtbox;
    private CollisionShape2D? _attackHitboxShape;
    private Node2D? _visualRig;
    private VisualRigAnimator? _visualAnimator;
    private SpriteCharacterAnimator? _pixelSprite;
    private readonly HashSet<ulong> _hitEnemiesThisAttack = new();
    private PlayerState _state = PlayerState.Idle;
    private Vector2 _facing = Vector2.Right;
    private Vector2 _spawnPosition;
    private Rect2 _movementBounds = DefaultMovementBounds;
    private int _health = MaxHealth;
    private float _attackCooldown;
    private float _slashTimer;
    private float _attackTimer;
    private float _hitStunTimer;
    private float _hitFlash;
    private float _invulnerabilityTimer;
    private float _comboWindow;
    private int _comboStep;
    private bool _isHeavyAttack;

    public Rect2 MovementBounds
    {
        get => _movementBounds;
        set => _movementBounds = value;
    }

    public bool IsAlive => _state != PlayerState.Dead;

    public int CurrentHealth => _health;

    public int MaxHealthValue => MaxHealth;

    public override void _Ready()
    {
        AddToGroup("player");
        _spawnPosition = GlobalPosition;
        EnsureInputMap();
        SilhouetteArt.EnsurePlayerVisualRig(this);
        _visualRig = GetNode<Node2D>("VisualRig");
        _visualAnimator = GetNodeOrNull<VisualRigAnimator>("VisualAnimator");
        if (_visualAnimator is null)
        {
            _visualAnimator = new VisualRigAnimator { Name = "VisualAnimator" };
            AddChild(_visualAnimator);
            _visualAnimator.Bind(_visualRig);
        }

        BuildVisuals();
        AttachAranduSprite();
        BuildCollision();
        BuildAttackHitbox();
        BuildHurtbox();
    }

    public void TakeHit(Vector2 impulse, int damage)
    {
        if (_state == PlayerState.Dead || _invulnerabilityTimer > 0f)
        {
            return;
        }

        _health -= damage;
        _hitFlash = 0.12f;
        _hitStunTimer = HitStunSeconds + (damage - 1) * 0.05f;
        Velocity = impulse;
        ImpactFeedback.Get(this)?.OnPlayerHit(GlobalPosition + new Vector2(0, -4), damage);

        if (_health <= 0)
        {
            _state = PlayerState.Dead;
            SetAttackHitboxEnabled(false);
            GetTree().CreateTimer(RespawnDelaySeconds).Timeout += Respawn;
            return;
        }

        _state = PlayerState.HitStun;
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _attackCooldown = Mathf.Max(0f, _attackCooldown - dt);
        _slashTimer = Mathf.Max(0f, _slashTimer - dt);
        _hitFlash = Mathf.Max(0f, _hitFlash - dt);
        _invulnerabilityTimer = Mathf.Max(0f, _invulnerabilityTimer - dt);
        _comboWindow = Mathf.Max(0f, _comboWindow - dt);

        if (_state == PlayerState.Dead)
        {
            Velocity = Velocity.Lerp(Vector2.Zero, 9f * dt);
            MoveAndSlide();
            UpdateVisualState();
            return;
        }

        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (_state is not PlayerState.LightAttack and not PlayerState.HeavyAttack and not PlayerState.HitStun && input.LengthSquared() > 0.01f)
        {
            _facing = input.X < -0.1f ? Vector2.Left : input.X > 0.1f ? Vector2.Right : _facing;
        }

        if (_state == PlayerState.LightAttack || _state == PlayerState.HeavyAttack)
        {
            UpdateAttack(dt);
            Velocity = Vector2.Zero;
        }
        else if (_state == PlayerState.HitStun)
        {
            _hitStunTimer = Mathf.Max(0f, _hitStunTimer - dt);
            Velocity = Velocity.Lerp(Vector2.Zero, 9f * dt);
            if (_hitStunTimer <= 0f)
            {
                _state = PlayerState.Idle;
            }
        }
        else
        {
            Velocity = input.Normalized() * Speed;
            _state = input.LengthSquared() > 0.01f ? PlayerState.Walk : PlayerState.Idle;
        }

        MoveAndSlide();
        ClampToMovementBounds();
        ZIndex = Mathf.RoundToInt(GlobalPosition.Y);

        _visualAnimator?.SetMoving(_state == PlayerState.Walk && Velocity.LengthSquared() > 1f);
        _visualAnimator?.SetFacing(_facing.X);

        if (Input.IsActionJustPressed("attack_light") && _attackCooldown <= 0f && _state is PlayerState.Idle or PlayerState.Walk)
        {
            StartLightAttack(_comboWindow > 0f ? 2 : 1);
        }

        if (Input.IsActionJustPressed("attack_heavy") && _attackCooldown <= 0f && _state is PlayerState.Idle or PlayerState.Walk)
        {
            StartHeavyAttack();
        }

        if (Input.IsActionJustPressed("execute") && _state is PlayerState.Idle or PlayerState.Walk)
        {
            TryExecute();
        }

        if (_attackSlash is not null)
        {
            _attackSlash.Visible = _slashTimer > 0f;
        }

        UpdateVisualState();
    }

    private void Respawn()
    {
        _health = MaxHealth;
        _state = PlayerState.Idle;
        _hitStunTimer = 0f;
        _invulnerabilityTimer = 0.8f;
        GlobalPosition = _spawnPosition;
        Velocity = Vector2.Zero;
    }

    private void StartLightAttack(int comboStep)
    {
        _isHeavyAttack = false;
        _comboStep = comboStep;
        _state = PlayerState.LightAttack;
        _attackCooldown = AttackCooldownSeconds;
        _attackTimer = AttackStartupSeconds + AttackActiveSeconds + AttackRecoverySeconds;
        _slashTimer = AttackActiveSeconds + AttackRecoverySeconds;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(false, comboStep == 2);
        SetAttackHitboxEnabled(false);
    }

    private void StartHeavyAttack()
    {
        _isHeavyAttack = true;
        _comboStep = 0;
        _state = PlayerState.HeavyAttack;
        _attackCooldown = HeavyAttackCooldownSeconds;
        _attackTimer = HeavyAttackStartupSeconds + HeavyAttackActiveSeconds + HeavyAttackRecoverySeconds;
        _slashTimer = HeavyAttackActiveSeconds + HeavyAttackRecoverySeconds;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(true, false);
        SetAttackHitboxEnabled(false);
    }

    private void TryExecute()
    {
        foreach (var node in GetTree().GetNodesInGroup("enemy"))
        {
            if (node is not EnemyBase enemy || !enemy.CanBeExecuted)
            {
                continue;
            }

            if (GlobalPosition.DistanceTo(enemy.GlobalPosition) > ExecuteRange)
            {
                continue;
            }

            enemy.Execute(new Vector2(_facing.X * 120f, -14f), PickExecutionStyle(enemy));
            CombatFeel.ApplyExecuteImpact(GetParent<PrototypeArena>());
            return;
        }
    }

    private void UpdateAttack(float dt)
    {
        _attackTimer = Mathf.Max(0f, _attackTimer - dt);

        var recovery = _isHeavyAttack ? HeavyAttackRecoverySeconds : AttackRecoverySeconds;
        var active = _isHeavyAttack ? HeavyAttackActiveSeconds : AttackActiveSeconds;
        var activeStartsAt = recovery;
        var activeEndsAt = recovery + active;
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
            if (!_isHeavyAttack && _comboStep == 1)
            {
                _comboWindow = ComboWindowSeconds;
            }
            else
            {
                _comboStep = 0;
                _comboWindow = 0f;
            }
        }
    }

    private void ResolveActiveHitbox()
    {
        if (_attackHitbox is null || _attackHitboxShape?.Shape is not RectangleShape2D rectangle)
        {
            return;
        }

        rectangle.Size = _isHeavyAttack ? new Vector2(52, 28) : new Vector2(44, 24);
        _attackHitbox.Position = new Vector2(_facing.X * (_isHeavyAttack ? 40f : 34f), -8f);
        _attackHitbox.ForceUpdateTransform();

        foreach (var area in _attackHitbox.GetOverlappingAreas())
        {
            if (area.GetParent() is not EnemyBase enemy || _hitEnemiesThisAttack.Contains(enemy.GetInstanceId()))
            {
                continue;
            }

            _hitEnemiesThisAttack.Add(enemy.GetInstanceId());
            var impulse = new Vector2(_facing.X * (_isHeavyAttack ? 128f : 96f), _isHeavyAttack ? -16f : -10f);
            var attackKind = _isHeavyAttack
                ? PlayerAttackKind.Heavy
                : _comboStep == 2 ? PlayerAttackKind.ComboFinisher : PlayerAttackKind.Light;
            enemy.TakeHit(impulse, attackKind);
            if (_comboStep == 2)
            {
                enemy.TakeHit(impulse * 0.5f, PlayerAttackKind.ComboFinisher);
            }

            CombatFeel.ApplyPlayerAttackImpact(GetParent<PrototypeArena>(), attackKind);
        }
    }

    private void PlaySwingSfx(bool heavy, bool comboFinisher)
    {
        var kind = heavy
            ? PlayerAttackKind.Heavy
            : comboFinisher ? PlayerAttackKind.ComboFinisher : PlayerAttackKind.Light;
        CombatAudio.Get(this)?.PlayPlayerSwing(kind);
    }

    private void BuildVisuals()
    {
        _body = _visualRig?.GetNodeOrNull<Polygon2D>("WarriorPlaceholder");
        _paint = _visualRig?.GetNodeOrNull<Polygon2D>("Paint");
        _attackSlash = _visualRig?.GetNodeOrNull<Polygon2D>("AttackSlash");

        if (_body is not null && _paint is not null && _attackSlash is not null)
        {
            return;
        }

        _visualRig ??= new Node2D { Name = "VisualRig" };
        if (_visualRig.GetParent() is null)
        {
            AddChild(_visualRig);
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
        _visualRig.AddChild(_body);

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
        _visualRig.AddChild(_paint);

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
        _visualRig.AddChild(_attackSlash);
    }

    private void AttachAranduSprite()
    {
        if (_visualRig is null)
        {
            return;
        }

        _pixelSprite = new SpriteCharacterAnimator { Name = "AranduSprite" };
        _visualRig.AddChild(_pixelSprite);
        _pixelSprite.Configure(AranduSpriteArt.BuildSpriteFrames(), new Vector2(0, -6));

        foreach (var child in _visualRig.GetChildren())
        {
            if (child is Polygon2D polygon && child.Name.ToString() != "AttackSlash")
            {
                polygon.Visible = false;
            }
        }
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
            CollisionMask = EnemyBase.HurtboxCollisionLayer,
            Position = new Vector2(34, -8)
        };
        _attackHitbox.AddChild(_attackHitboxShape);
        AddChild(_attackHitbox);
    }

    private void BuildHurtbox()
    {
        _hurtbox = GetNodeOrNull<Area2D>("Hurtbox");
        if (_hurtbox is not null)
        {
            return;
        }

        _hurtbox = new Area2D
        {
            Name = "Hurtbox",
            CollisionLayer = HurtboxCollisionLayer,
            CollisionMask = 0,
            Monitorable = true,
            Monitoring = false
        };

        _hurtbox.AddChild(new CollisionShape2D
        {
            Name = "HurtboxShape",
            Shape = new RectangleShape2D
            {
                Size = new Vector2(22, 32)
            },
            Position = new Vector2(0, -6)
        });

        AddChild(_hurtbox);
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
                PlayerState.Dead => new Color("#4a2f24"),
                _ when _hitFlash > 0f => new Color("#f0d7aa"),
                PlayerState.LightAttack => new Color("#d4843e"),
                PlayerState.HeavyAttack => new Color("#b51f1f"),
                PlayerState.Walk => new Color("#c46f35"),
                _ => new Color("#a85f34")
            };

            _body.Modulate = _invulnerabilityTimer > 0f && Mathf.FloorToInt(_invulnerabilityTimer * 12f) % 2 == 0
                ? new Color(1f, 1f, 1f, 0.45f)
                : Colors.White;
        }

        if (_paint is not null)
        {
            _paint.Color = _state is PlayerState.LightAttack or PlayerState.HeavyAttack ? new Color("#f0d06a") : new Color("#e0b75d");
            _paint.Visible = _state != PlayerState.Dead;
        }

        _pixelSprite?.SetFacing(_facing.X);
        _pixelSprite?.UpdatePresentation(
            _state == PlayerState.Walk && Velocity.LengthSquared() > 1f,
            _state is PlayerState.LightAttack or PlayerState.HeavyAttack,
            _state == PlayerState.HeavyAttack,
            _hitFlash > 0f,
            _state == PlayerState.Dead);
    }

    private static ExecutionStyle PickExecutionStyle(EnemyBase enemy)
    {
        var bucket = Mathf.Abs((int)(enemy.GetInstanceId() % 3));
        return bucket switch
        {
            1 => ExecutionStyle.GutRip,
            2 => ExecutionStyle.SkullCrush,
            _ => ExecutionStyle.Decapitate
        };
    }

    private static void EnsureInputMap()
    {
        AddKeyAction("move_left", Key.A);
        AddKeyAction("move_right", Key.D);
        AddKeyAction("move_up", Key.W);
        AddKeyAction("move_down", Key.S);
        AddKeyAction("attack_light", Key.J);
        AddKeyAction("attack_heavy", Key.K);
        AddKeyAction("execute", Key.E);
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
