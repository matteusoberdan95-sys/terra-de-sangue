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
    private const float ComboWindowSeconds = 0.30f;
    private const float ExecuteRange = 42f;
    private const float MaxStamina = 100f;
    private const float DashCost = 22f;
    private const float DashDurationSeconds = 0.32f;
    private const float DashIFramesSeconds = 0.20f;
    private const float DashSpeed = 168f;
    private const float StaminaRegenOutOfCombat = 18f;
    private const float StaminaRegenInCombat = 10f;
    private const float StaminaRegenDelaySeconds = 0.35f;
    private const float RunSpeed = 198f;
    private const float RunDrainPerSecond = 15f;
    private const float RunMinStamina = 8f;
    private const float DoubleTapWindowSeconds = 0.22f;
    private const float HeavyAttackStaminaCost = 8f;
    private const float JumpCost = 14f;
    private const float JumpDurationSeconds = 0.42f;
    private const float JumpDepthArc = 20f;
    private const float JumpVisualLift = 10f;
    private const float FrontJumpHorizontalArc = 72f;
    private const float FrontJumpLandingRecoverySeconds = 0.12f;
    private const float AirSlamStaminaCost = 6f;
    private const float AirHammerStaminaCost = 12f;
    private const float AirSlamLandingRecoverySeconds = 0.14f;
    private const float AirHammerLandingRecoverySeconds = 0.22f;
    private const float AirSlamRadius = 28f;
    private const float AirDiveSlamRadius = 34f;
    private const int MaxArrows = 5;
    private const float BowStaminaCost = 8f;
    private const float BowRecoverySeconds = 0.28f;
    private static readonly Rect2 DefaultMovementBounds = new(new Vector2(-380, 124), new Vector2(760, 92));

    private enum AirAttackPending
    {
        None,
        Slam,
        Hammer
    }

    private enum PlayerState
    {
        Idle,
        Walk,
        LightAttack,
        HeavyAttack,
        Dash,
        Run,
        Jump,
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
    private float _facingX = 1f;
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
    private bool _isRunAttack;
    private bool _isComboHeavyFinisher;
    private bool _isArtifactAttack;
    private float _stamina = MaxStamina;
    private float _staminaRegenDelay;
    private float _staminaBlockedFlash;
    private float _gameTime;
    private float _lastLeftTapTime = -999f;
    private float _lastRightTapTime = -999f;
    private float _runDirectionX = 1f;
    private float _dashTimer;
    private float _dashInvulnTimer;
    private float _jumpTimer;
    private float _jumpStartDepthY;
    private float _jumpStartX;
    private bool _isFrontJump;
    private float _jumpLandingRecovery;
    private int _nextComboHit = 1;
    private bool _airLightUsed;
    private bool _airHeavyUsed;
    private float _airApexAttackTimer;
    private AirAttackPending _airAttackPending = AirAttackPending.None;
    private int _arrowCount;
    private bool _isAimingBow;
    private bool _bowHeld;
    private float _aimDepthOffset;
    private float _bowRecoveryTimer;
    private Polygon2D? _aimIndicator;
    private int _artifactCharges;
    private bool _artifactEquipped;
    private ArtifactKind _artifactKind = ArtifactKind.None;

    private WeaponProfile WeaponProfile => WeaponProgression.CurrentProfile;

    private float EffectiveComboWindow => ComboWindowSeconds + WeaponProfile.ComboWindowBonus;

    public Rect2 MovementBounds
    {
        get => _movementBounds;
        set => _movementBounds = value;
    }

    public bool IsAlive => _state != PlayerState.Dead;

    public int CurrentHealth => _health;

    public int MaxHealthValue => MaxHealth;

    public float CurrentStamina => _stamina;

    public float MaxStaminaValue => MaxStamina;

    public bool IsDashing => _state == PlayerState.Dash;

    public bool IsRunning => _state == PlayerState.Run;

    public bool IsJumping => _state == PlayerState.Jump;

    public bool StaminaExhaustedFlash => _staminaBlockedFlash > 0f;

    public int ArrowCount => _arrowCount;

    public int MaxArrowCount => MaxArrows;

    public string WeaponDisplayName => WeaponProfile.DisplayName;

    public int ArtifactCharges => _artifactCharges;

    public bool ArtifactEquipped => _artifactEquipped;

    public string ArtifactDisplayName => _artifactKind switch
    {
        ArtifactKind.IronKnife => "Faca de Ferro",
        ArtifactKind.BrokenClub => "Clava Quebrada",
        _ => string.Empty
    };

    public bool IsAimingBow => _isAimingBow;

    public void AddArrows(int amount)
    {
        _arrowCount = Mathf.Min(MaxArrows, _arrowCount + Mathf.Max(0, amount));
    }

    public void GrantArtifact(ArtifactKind kind, int uses)
    {
        _artifactKind = uses > 0 ? kind : ArtifactKind.None;
        _artifactCharges = Mathf.Max(0, uses);
        _artifactEquipped = _artifactCharges > 0;
    }

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
        BuildAimIndicator();
    }

    public void TakeHit(Vector2 impulse, int damage)
    {
        if (_state == PlayerState.Dead || _invulnerabilityTimer > 0f || _dashInvulnTimer > 0f)
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
        _gameTime += dt;
        _attackCooldown = Mathf.Max(0f, _attackCooldown - dt);
        _slashTimer = Mathf.Max(0f, _slashTimer - dt);
        _hitFlash = Mathf.Max(0f, _hitFlash - dt);
        _invulnerabilityTimer = Mathf.Max(0f, _invulnerabilityTimer - dt);
        _comboWindow = Mathf.Max(0f, _comboWindow - dt);
        _dashInvulnTimer = Mathf.Max(0f, _dashInvulnTimer - dt);
        _staminaRegenDelay = Mathf.Max(0f, _staminaRegenDelay - dt);
        _staminaBlockedFlash = Mathf.Max(0f, _staminaBlockedFlash - dt);
        _bowRecoveryTimer = Mathf.Max(0f, _bowRecoveryTimer - dt);
        UpdateStaminaRegen(dt);

        if (_state == PlayerState.Dead)
        {
            Velocity = Velocity.Lerp(Vector2.Zero, 9f * dt);
            MoveAndSlide();
            UpdateVisualState();
            return;
        }

        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        if (_state == PlayerState.Dash)
        {
            UpdateDash(dt);
        }
        else if (_state == PlayerState.Run)
        {
            UpdateRun(dt, input);
        }
        else if (_state == PlayerState.Jump)
        {
            UpdateJump(dt, input);
        }
        else if (_jumpLandingRecovery > 0f)
        {
            _jumpLandingRecovery = Mathf.Max(0f, _jumpLandingRecovery - dt);
            Velocity = Vector2.Zero;
            _state = PlayerState.Idle;
        }
        else if (_state == PlayerState.LightAttack || _state == PlayerState.HeavyAttack)
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
            var moveSpeed = _isAimingBow ? Speed * 0.35f : Speed;
            Velocity = input.Normalized() * moveSpeed;
            _state = input.LengthSquared() > 0.01f ? PlayerState.Walk : PlayerState.Idle;
        }

        UpdateBowAim(dt, input);

        UpdateFacing(input);

        MoveAndSlide();
        ClampToMovementBounds();
        ZIndex = Mathf.RoundToInt(GlobalPosition.Y);

        _visualAnimator?.SetMoving((_state == PlayerState.Walk || _state == PlayerState.Run) && Velocity.LengthSquared() > 1f);
        if (_pixelSprite is null)
        {
            _visualAnimator?.SetFacing(_facingX);
        }

        if (Input.IsActionJustPressed("move_left"))
        {
            TryDoubleTapRun(-1f);
            _lastLeftTapTime = _gameTime;
        }

        if (Input.IsActionJustPressed("move_right"))
        {
            TryDoubleTapRun(1f);
            _lastRightTapTime = _gameTime;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            TryStartJump();
        }

        if (Input.IsActionJustPressed("dash"))
        {
            TryStartDash();
        }

        if (Input.IsActionJustPressed("artifact_toggle"))
        {
            ToggleArtifact();
        }

        if (Input.IsActionJustPressed("attack_light") && _attackCooldown <= 0f)
        {
            if (_state == PlayerState.Jump)
            {
                TryAirLightAttack();
            }
            else if (_state == PlayerState.Run)
            {
                StartRunAttack();
            }
            else if (CanStartLightAttack())
            {
                if (_artifactEquipped && _artifactCharges > 0 && _artifactKind == ArtifactKind.IronKnife)
                {
                    StartArtifactAttack();
                }
                else
                {
                    StartLightAttack(ResolveComboStep());
                }
            }
        }

        if (Input.IsActionJustPressed("attack_heavy") && _attackCooldown <= 0f)
        {
            if (_artifactEquipped && _artifactCharges > 0 && _artifactKind == ArtifactKind.BrokenClub)
            {
                StartArtifactHeavyAttack();
            }
            else if (_state == PlayerState.Jump)
            {
                TryAirHeavyAttack();
            }
            else if (CanComboHeavyFinisher())
            {
                StartComboHeavyFinisher();
            }
            else if (CanStartHeavyAttack())
            {
                StartHeavyAttack();
            }
        }

        if (Input.IsActionJustPressed("execute") && _state is PlayerState.Idle or PlayerState.Walk)
        {
            TryExecute();
        }

        if (_attackSlash is not null)
        {
            _attackSlash.Visible = _slashTimer > 0f;
            _attackSlash.Scale = new Vector2(_facingX, 1f);
        }

        UpdateVisualState();
    }

    private void Respawn()
    {
        _health = MaxHealth;
        _stamina = MaxStamina;
        _state = PlayerState.Idle;
        _hitStunTimer = 0f;
        _dashTimer = 0f;
        _jumpTimer = 0f;
        _jumpLandingRecovery = 0f;
        _nextComboHit = 1;
        _airLightUsed = false;
        _airHeavyUsed = false;
        _airApexAttackTimer = 0f;
        _airAttackPending = AirAttackPending.None;
        _facingX = 1f;
        _invulnerabilityTimer = 0.8f;
        GlobalPosition = _spawnPosition;
        Velocity = Vector2.Zero;
    }

    private bool CanStartLightAttack()
    {
        return _bowRecoveryTimer <= 0f
            && !_isAimingBow
            && _jumpLandingRecovery <= 0f
            && (_state is PlayerState.Idle or PlayerState.Walk || _comboWindow > 0f);
    }

    private bool CanStartHeavyAttack()
    {
        return _bowRecoveryTimer <= 0f
            && !_isAimingBow
            && _jumpLandingRecovery <= 0f
            && _state is PlayerState.Idle or PlayerState.Walk;
    }

    private bool CanComboHeavyFinisher()
    {
        return _jumpLandingRecovery <= 0f && _comboWindow > 0f && _nextComboHit == 3 && _state is PlayerState.Idle or PlayerState.Walk;
    }

    private int ResolveComboStep()
    {
        if (_comboWindow > 0f)
        {
            return _nextComboHit;
        }

        return 1;
    }

    private void StartLightAttack(int comboStep)
    {
        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        _isHeavyAttack = false;
        _isRunAttack = false;
        _isArtifactAttack = false;

        if (_comboWindow > 0f)
        {
            _comboWindow = 0f;
        }

        _comboStep = comboStep;
        _state = PlayerState.LightAttack;
        _attackCooldown = AttackCooldownSeconds;
        var startup = _comboStep == 3 ? 0.04f : AttackStartupSeconds;
        _attackTimer = startup + AttackActiveSeconds + (_comboStep == 3 ? 0.12f : AttackRecoverySeconds);
        _slashTimer = AttackActiveSeconds + (_comboStep == 3 ? 0.12f : AttackRecoverySeconds);
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(false, _comboStep >= 2);
        SetAttackHitboxEnabled(false);
        SyncAttackHitboxPosition();
    }

    private void StartArtifactHeavyAttack()
    {
        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        _isHeavyAttack = true;
        _isRunAttack = false;
        _isArtifactAttack = true;
        _artifactEquipped = false;
        _comboStep = 0;
        _state = PlayerState.HeavyAttack;
        _attackCooldown = 0.62f;
        _attackTimer = 0.06f + HeavyAttackActiveSeconds + 0.24f;
        _slashTimer = HeavyAttackActiveSeconds + 0.24f;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(true, true);
        SetAttackHitboxEnabled(false);
        SyncAttackHitboxPosition();
    }

    private void StartArtifactAttack()
    {
        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        _isHeavyAttack = false;
        _isRunAttack = false;
        _isArtifactAttack = true;
        _artifactEquipped = false;
        _comboStep = 2;
        _state = PlayerState.LightAttack;
        _attackCooldown = 0.26f;
        _attackTimer = 0.03f + AttackActiveSeconds + 0.1f;
        _slashTimer = AttackActiveSeconds + 0.1f;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(false, true);
        SetAttackHitboxEnabled(false);
        SyncAttackHitboxPosition();
    }

    private void StartRunAttack()
    {
        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        _isHeavyAttack = false;
        _isRunAttack = true;
        _runDirectionX = _facingX;
        _comboStep = 2;
        _state = PlayerState.LightAttack;
        _attackCooldown = 0.28f;
        _attackTimer = 0.04f + AttackActiveSeconds + 0.1f;
        _slashTimer = AttackActiveSeconds + 0.1f;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(false, true);
        SetAttackHitboxEnabled(false);
        SyncAttackHitboxPosition();
    }

    private void StartComboHeavyFinisher()
    {
        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        if (_stamina < HeavyAttackStaminaCost)
        {
            _staminaBlockedFlash = 0.25f;
            return;
        }

        SpendStamina(HeavyAttackStaminaCost);
        _comboWindow = 0f;
        _nextComboHit = 1;
        _isHeavyAttack = true;
        _isComboHeavyFinisher = true;
        _isRunAttack = false;
        _comboStep = 3;
        _state = PlayerState.HeavyAttack;
        _attackCooldown = 0.48f;
        _attackTimer = 0.08f + HeavyAttackActiveSeconds + 0.2f;
        _slashTimer = HeavyAttackActiveSeconds + 0.2f;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(true, true);
        SetAttackHitboxEnabled(false);
        SyncAttackHitboxPosition();
    }

    private void StartHeavyAttack()
    {
        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        if (_stamina < HeavyAttackStaminaCost)
        {
            _staminaBlockedFlash = 0.25f;
            return;
        }

        SpendStamina(HeavyAttackStaminaCost);
        _isHeavyAttack = true;
        _isComboHeavyFinisher = false;
        _isRunAttack = false;
        _comboStep = 0;
        _state = PlayerState.HeavyAttack;
        _attackCooldown = HeavyAttackCooldownSeconds;
        _attackTimer = HeavyAttackStartupSeconds + HeavyAttackActiveSeconds + HeavyAttackRecoverySeconds;
        _slashTimer = HeavyAttackActiveSeconds + HeavyAttackRecoverySeconds;
        _hitEnemiesThisAttack.Clear();
        _visualAnimator?.PulseAttack();
        PlaySwingSfx(true, false);
        SetAttackHitboxEnabled(false);
        SyncAttackHitboxPosition();
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

            enemy.Execute(new Vector2(_facingX * 120f, -14f), PickExecutionStyle(enemy));
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

        if (!_isHeavyAttack && _state == PlayerState.LightAttack && _attackTimer > 0f && _attackTimer <= recovery && Input.IsActionJustPressed("dash"))
        {
            SetAttackHitboxEnabled(false);
            TryStartDash();
            return;
        }

        SetAttackHitboxEnabled(isActive);
        if (isActive)
        {
            ResolveActiveHitbox();
        }

        if (_attackTimer <= 0f)
        {
            SetAttackHitboxEnabled(false);
            var wasRunAttack = _isRunAttack;
            _state = PlayerState.Idle;
            _isRunAttack = false;
            _isComboHeavyFinisher = false;
            _isArtifactAttack = false;
            if (wasRunAttack)
            {
                _comboStep = 0;
                _comboWindow = 0f;
                _nextComboHit = 1;
            }
            else if (!_isHeavyAttack && _comboStep is > 0 and < 3)
            {
                _comboWindow = EffectiveComboWindow;
                _nextComboHit = _comboStep + 1;
            }
            else
            {
                _comboStep = 0;
                _comboWindow = 0f;
                _nextComboHit = 1;
            }
        }
    }

    private void ResolveActiveHitbox()
    {
        if (_attackHitbox is null || _attackHitboxShape?.Shape is not RectangleShape2D rectangle)
        {
            return;
        }

        rectangle.Size = _isHeavyAttack
            ? new Vector2(_isComboHeavyFinisher ? 56f : 52f, 28f)
            : _comboStep == 3 ? new Vector2(50f, 26f) : new Vector2(44, 24);
        SyncAttackHitboxPosition();
        _attackHitbox.ForceUpdateTransform();

        foreach (var area in _attackHitbox.GetOverlappingAreas())
        {
            if (area.GetParent() is not EnemyBase enemy || _hitEnemiesThisAttack.Contains(enemy.GetInstanceId()))
            {
                continue;
            }

            _hitEnemiesThisAttack.Add(enemy.GetInstanceId());
            var baseImpulseX = _isHeavyAttack ? 128f : _comboStep == 3 ? 112f : 96f;
            var impulse = new Vector2(_facingX * baseImpulseX, _isHeavyAttack ? -16f : _comboStep == 3 ? -14f : -10f);
            var isComboFinisher = _comboStep >= 2 || _isRunAttack || _isComboHeavyFinisher;
            var attackKind = _isHeavyAttack
                ? PlayerAttackKind.Heavy
                : isComboFinisher ? PlayerAttackKind.ComboFinisher : PlayerAttackKind.Light;
            var impulseScale = _isComboHeavyFinisher ? 1.35f
                : _comboStep == 3 ? 1.28f
                : _isRunAttack ? 1.3f
                : 1f;
            var hitDamage = ResolveHitDamage(attackKind);
            enemy.TakeHit(impulse * impulseScale, attackKind, hitDamage);
            if (_isArtifactAttack)
            {
                enemy.ApplyBleed(_artifactKind == ArtifactKind.BrokenClub ? BleedLevel.Hemorrhage : BleedLevel.Heavy);
            }
            else if (_isHeavyAttack && WeaponProfile.HeavyAppliesBleed)
            {
                enemy.ApplyBleed(BleedLevel.Heavy);
            }

            if (_comboStep == 2 || _comboStep == 3)
            {
                enemy.TakeHit(impulse * 0.5f, PlayerAttackKind.ComboFinisher, hitDamage > 1 ? 1 : hitDamage);
            }

            if (_isComboHeavyFinisher)
            {
                enemy.TakeHit(impulse * 0.35f, PlayerAttackKind.Heavy);
            }

            CombatFeel.ApplyPlayerAttackImpact(GetParent<PrototypeArena>(), attackKind);
        }

        if (_isArtifactAttack && _hitEnemiesThisAttack.Count > 0)
        {
            _artifactCharges = Mathf.Max(0, _artifactCharges - 1);
            if (_artifactCharges <= 0)
            {
                _artifactKind = ArtifactKind.None;
                CombatAudio.Get(this)?.PlayArtifactBreak();
            }
        }
    }

    private void UpdateBowAim(float dt, Vector2 input)
    {
        if (Input.IsActionJustPressed("aim_bow") && CanUseBow())
        {
            _bowHeld = true;
        }

        if (_bowHeld && Input.IsActionPressed("aim_bow") && CanUseBow())
        {
            _isAimingBow = true;
            if (input.Y < -0.1f)
            {
                _aimDepthOffset = Mathf.Clamp(_aimDepthOffset - dt * 1.4f, -0.65f, 0.65f);
            }
            else if (input.Y > 0.1f)
            {
                _aimDepthOffset = Mathf.Clamp(_aimDepthOffset + dt * 1.4f, -0.65f, 0.65f);
            }

            UpdateAimIndicator();
        }
        else
        {
            _isAimingBow = false;
            if (_aimIndicator is not null)
            {
                _aimIndicator.Visible = false;
            }
        }

        if (_bowHeld && Input.IsActionJustReleased("aim_bow"))
        {
            _bowHeld = false;
            _isAimingBow = false;
            if (_aimIndicator is not null)
            {
                _aimIndicator.Visible = false;
            }

            TryShootArrow();
        }
    }

    private bool CanUseBow()
    {
        return _bowRecoveryTimer <= 0f
            && _jumpLandingRecovery <= 0f
            && _state is PlayerState.Idle or PlayerState.Walk;
    }

    private void TryShootArrow()
    {
        if (_arrowCount <= 0 || _stamina < BowStaminaCost)
        {
            _staminaBlockedFlash = 0.25f;
            return;
        }

        SpendStamina(BowStaminaCost);
        _arrowCount--;
        _bowRecoveryTimer = BowRecoverySeconds;
        var direction = new Vector2(_facingX, _aimDepthOffset);
        if (direction.LengthSquared() < 0.01f)
        {
            direction = new Vector2(_facingX, 0f);
        }

        direction = direction.Normalized();
        var projectile = new ArrowProjectile { Name = "ArrowProjectile" };
        GetParent()?.AddChild(projectile);
        projectile.Launch(GlobalPosition + new Vector2(_facingX * 10f, -10f), direction);
        CombatAudio.Get(this)?.PlayBowRelease();
        _aimDepthOffset = 0f;
    }

    private int ResolveHitDamage(PlayerAttackKind attackKind)
    {
        if (_isHeavyAttack || attackKind is PlayerAttackKind.Heavy or PlayerAttackKind.ComboFinisher)
        {
            return 1;
        }

        return 1 + WeaponProfile.LightDamageBonus;
    }

    private void ToggleArtifact()
    {
        if (_artifactCharges <= 0)
        {
            _artifactEquipped = false;
            _artifactKind = ArtifactKind.None;
            return;
        }

        _artifactEquipped = !_artifactEquipped;
    }

    private void SyncAttackHitboxPosition()
    {
        if (_attackHitbox is null)
        {
            return;
        }

        _attackHitbox.Position = new Vector2(_facingX * (_isHeavyAttack ? 40f : _comboStep == 3 ? 38f : 34f), -8f);
    }

    private void BuildAimIndicator()
    {
        if (_visualRig is null)
        {
            return;
        }

        _aimIndicator = new Polygon2D
        {
            Name = "AimIndicator",
            Visible = false,
            Color = new Color("#e0b75d", 0.75f),
            Polygon = new[]
            {
                new Vector2(8, 0),
                new Vector2(48, -3),
                new Vector2(48, 3)
            }
        };
        _visualRig.AddChild(_aimIndicator);
    }

    private void UpdateAimIndicator()
    {
        if (_aimIndicator is null)
        {
            return;
        }

        _aimIndicator.Visible = true;
        _aimIndicator.Scale = new Vector2(_facingX, 1f);
        _aimIndicator.Position = new Vector2(0f, _aimDepthOffset * -18f);
    }

    private void PlaySwingSfx(bool heavy, bool comboFinisher)
    {
        var kind = heavy
            ? PlayerAttackKind.Heavy
            : comboFinisher ? PlayerAttackKind.ComboFinisher : PlayerAttackKind.Light;
        CombatAudio.Get(this)?.PlayPlayerSwing(kind);
    }

    private bool TryStartDash()
    {
        if (_state is PlayerState.Dead or PlayerState.HitStun or PlayerState.HeavyAttack or PlayerState.Dash or PlayerState.Jump)
        {
            return false;
        }

        if (_stamina < DashCost)
        {
            _staminaBlockedFlash = 0.25f;
            return false;
        }

        SpendStamina(DashCost);
        _state = PlayerState.Dash;
        _dashTimer = DashDurationSeconds;
        _dashInvulnTimer = DashIFramesSeconds;
        _attackTimer = 0f;
        SetAttackHitboxEnabled(false);

        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        var dashDirection = input.LengthSquared() > 0.01f ? input.Normalized() : new Vector2(_facingX, 0f);
        if (Mathf.Abs(dashDirection.X) > 0.1f)
        {
            SetFacingX(dashDirection.X);
        }

        Velocity = dashDirection * DashSpeed;
        CombatAudio.Get(this)?.PlayDodge();
        return true;
    }

    private void UpdateDash(float dt)
    {
        _dashTimer = Mathf.Max(0f, _dashTimer - dt);
        Velocity = Velocity.Lerp(Vector2.Zero, 4f * dt);

        if (_dashTimer > 0f)
        {
            return;
        }

        Velocity = Vector2.Zero;
        _state = PlayerState.Idle;
    }

    private bool TryStartJump()
    {
        if (_state is PlayerState.Dead or PlayerState.HitStun or PlayerState.HeavyAttack or PlayerState.Dash or PlayerState.Jump
            or PlayerState.LightAttack)
        {
            return false;
        }

        if (_stamina < JumpCost)
        {
            _staminaBlockedFlash = 0.25f;
            return false;
        }

        SpendStamina(JumpCost);
        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        _isFrontJump = Input.IsActionPressed("move_up") && Mathf.Abs(input.X) > 0.05f;
        if (_isFrontJump)
        {
            SetFacingX(input.X);
        }

        _state = PlayerState.Jump;
        _jumpTimer = JumpDurationSeconds;
        _jumpStartDepthY = GlobalPosition.Y;
        _jumpStartX = GlobalPosition.X;
        _airLightUsed = false;
        _airHeavyUsed = false;
        _airApexAttackTimer = 0f;
        _airAttackPending = AirAttackPending.None;
        CombatAudio.Get(this)?.PlayJump();
        return true;
    }

    private float JumpProgress => 1f - (_jumpTimer / JumpDurationSeconds);

    private bool TryAirLightAttack()
    {
        if (_state != PlayerState.Jump || _airHeavyUsed)
        {
            return false;
        }

        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        var progress = JumpProgress;

        if (progress < 0.5f && !_airLightUsed)
        {
            _airLightUsed = true;
            _airApexAttackTimer = 0.1f;
            _hitEnemiesThisAttack.Clear();
            PlaySwingSfx(false, false);
            return true;
        }

        if (progress is >= 0.5f and < 0.95f && _airAttackPending == AirAttackPending.None && !_airLightUsed)
        {
            if (_stamina < AirSlamStaminaCost)
            {
                _staminaBlockedFlash = 0.25f;
                return false;
            }

            SpendStamina(AirSlamStaminaCost);
            _airHeavyUsed = true;
            _airAttackPending = AirAttackPending.Slam;
            _slashTimer = 0.2f;
            PlaySwingSfx(false, true);
            return true;
        }

        return false;
    }

    private bool TryAirHeavyAttack()
    {
        if (_state != PlayerState.Jump || _airHeavyUsed || _airLightUsed)
        {
            return false;
        }

        var progress = JumpProgress;
        if (progress is < 0.45f or >= 0.9f)
        {
            return false;
        }

        if (_stamina < AirHammerStaminaCost)
        {
            _staminaBlockedFlash = 0.25f;
            return false;
        }

        SnapFacingForAttack(Input.GetVector("move_left", "move_right", "move_up", "move_down"));
        SpendStamina(AirHammerStaminaCost);
        _airHeavyUsed = true;
        _airAttackPending = AirAttackPending.Hammer;
        _slashTimer = 0.24f;
        PlaySwingSfx(true, false);
        return true;
    }

    private void UpdateJump(float dt, Vector2 input)
    {
        _jumpTimer = Mathf.Max(0f, _jumpTimer - dt);
        var progress = 1f - (_jumpTimer / JumpDurationSeconds);
        var arc = Mathf.Sin(progress * Mathf.Pi);

        var horizontalSpeed = _isFrontJump ? Speed * 0.72f : Speed * 0.55f;
        Velocity = new Vector2(input.X * horizontalSpeed, 0f);
        var horizontalOffset = _isFrontJump ? _facingX * FrontJumpHorizontalArc * progress : 0f;
        GlobalPosition = new Vector2(
            Mathf.Clamp(_jumpStartX + horizontalOffset, _movementBounds.Position.X, _movementBounds.End.X),
            Mathf.Clamp(_jumpStartDepthY - arc * JumpDepthArc, _movementBounds.Position.Y, _movementBounds.End.Y));

        if (_visualRig is not null)
        {
            _visualRig.Position = new Vector2(0f, -arc * JumpVisualLift);
        }

        UpdateAirApexAttack(dt);

        if (_jumpTimer > 0f)
        {
            return;
        }

        LandFromJump();
    }

    private void UpdateAirApexAttack(float dt)
    {
        if (_airApexAttackTimer <= 0f)
        {
            return;
        }

        _airApexAttackTimer = Mathf.Max(0f, _airApexAttackTimer - dt);
        if (_attackHitbox is null || _attackHitboxShape?.Shape is not RectangleShape2D rectangle)
        {
            return;
        }

        rectangle.Size = new Vector2(40f, 20f);
        _attackHitbox.Position = new Vector2(_facingX * 28f, -22f);
        _attackHitbox.ForceUpdateTransform();
        SetAttackHitboxEnabled(true);
        ResolveAirApexHits();

        if (_airApexAttackTimer <= 0f)
        {
            SetAttackHitboxEnabled(false);
        }
    }

    private void ResolveAirApexHits()
    {
        if (_attackHitbox is null)
        {
            return;
        }

        foreach (var area in _attackHitbox.GetOverlappingAreas())
        {
            if (area.GetParent() is not EnemyBase enemy || _hitEnemiesThisAttack.Contains(enemy.GetInstanceId()))
            {
                continue;
            }

            _hitEnemiesThisAttack.Add(enemy.GetInstanceId());
            var impulse = new Vector2(_facingX * 72f, -6f);
            enemy.TakeHit(impulse, PlayerAttackKind.Light);
            CombatFeel.ApplyPlayerAttackImpact(GetParent<PrototypeArena>(), PlayerAttackKind.Light);
        }
    }

    private void LandFromJump()
    {
        var wasFrontJump = _isFrontJump;
        var pending = _airAttackPending;

        GlobalPosition = new Vector2(GlobalPosition.X, _jumpStartDepthY);
        if (_visualRig is not null)
        {
            _visualRig.Position = Vector2.Zero;
        }

        Velocity = Vector2.Zero;
        if (pending != AirAttackPending.None)
        {
            ResolveAirLandingAttack(pending, wasFrontJump);
            _attackCooldown = 0.22f;
        }

        _jumpLandingRecovery = pending switch
        {
            AirAttackPending.Hammer => AirHammerLandingRecoverySeconds,
            AirAttackPending.Slam => AirSlamLandingRecoverySeconds,
            _ => wasFrontJump ? FrontJumpLandingRecoverySeconds : 0f
        };

        _airAttackPending = AirAttackPending.None;
        _isFrontJump = false;
        _state = PlayerState.Idle;
    }

    private void ResolveAirLandingAttack(AirAttackPending kind, bool diving)
    {
        var radius = kind == AirAttackPending.Slam && diving ? AirDiveSlamRadius : AirSlamRadius;
        var attackKind = kind == AirAttackPending.Hammer ? PlayerAttackKind.Heavy : PlayerAttackKind.ComboFinisher;
        var impulseX = kind == AirAttackPending.Hammer ? 140f : diving ? 118f : 104f;
        var impulseY = kind == AirAttackPending.Hammer ? -20f : -16f;
        var impulseScale = kind == AirAttackPending.Hammer ? 1.35f : diving ? 1.28f : 1.15f;
        var impulse = new Vector2(_facingX * impulseX, impulseY) * impulseScale;
        var hitAny = false;

        foreach (var node in GetTree().GetNodesInGroup("enemy"))
        {
            if (node is not EnemyBase enemy || !enemy.IsAlive)
            {
                continue;
            }

            if (GlobalPosition.DistanceTo(enemy.GlobalPosition) > radius + 12f)
            {
                continue;
            }

            enemy.TakeHit(impulse, attackKind);
            if (kind == AirAttackPending.Hammer)
            {
                enemy.TakeHit(impulse * 0.35f, PlayerAttackKind.Heavy);
            }
            else if (diving)
            {
                enemy.TakeHit(impulse * 0.45f, PlayerAttackKind.ComboFinisher);
            }

            hitAny = true;
        }

        if (kind == AirAttackPending.Hammer)
        {
            CombatFeel.ApplyPlayerAttackImpact(GetParent<PrototypeArena>(), PlayerAttackKind.Heavy);
        }
        else
        {
            CombatFeel.ApplyAirSlamImpact(GetParent<PrototypeArena>());
        }

        if (hitAny)
        {
            CombatAudio.Get(this)?.PlayAirSlam(kind == AirAttackPending.Hammer);
        }

        _slashTimer = 0.12f;
        _visualAnimator?.PulseAttack();
    }

    private void TryDoubleTapRun(float directionX)
    {
        var lastTap = directionX < 0f ? _lastLeftTapTime : _lastRightTapTime;
        if (_gameTime - lastTap > DoubleTapWindowSeconds)
        {
            return;
        }

        TryStartRun(directionX);
    }

    private bool TryStartRun(float directionX)
    {
        if (_state is not (PlayerState.Idle or PlayerState.Walk))
        {
            return false;
        }

        if (_stamina < RunMinStamina)
        {
            _staminaBlockedFlash = 0.25f;
            return false;
        }

        _runDirectionX = directionX;
        _state = PlayerState.Run;
        SetFacingX(directionX);
        CombatAudio.Get(this)?.PlayRunStart();
        return true;
    }

    private void UpdateRun(float dt, Vector2 input)
    {
        if (input.X * _runDirectionX < -0.3f)
        {
            Velocity = input.LengthSquared() > 0.01f ? input.Normalized() * Speed : Vector2.Zero;
            _state = input.LengthSquared() > 0.01f ? PlayerState.Walk : PlayerState.Idle;
            return;
        }

        if (_stamina <= 0f)
        {
            Velocity = Vector2.Zero;
            _state = PlayerState.Idle;
            _staminaBlockedFlash = 0.25f;
            return;
        }

        SpendStamina(RunDrainPerSecond * dt);
        Velocity = new Vector2(_runDirectionX * RunSpeed, input.Y * Speed);
    }

    private void SpendStamina(float amount)
    {
        _stamina = Mathf.Max(0f, _stamina - amount);
        _staminaRegenDelay = StaminaRegenDelaySeconds;
    }

    private void SetFacingX(float directionX)
    {
        _facingX = directionX < 0f ? -1f : 1f;
    }

    private void UpdateFacing(Vector2 input)
    {
        if (_state is PlayerState.LightAttack or PlayerState.HeavyAttack or PlayerState.HitStun or PlayerState.Jump)
        {
            return;
        }

        if (input.X < -0.05f)
        {
            SetFacingX(-1f);
            return;
        }

        if (input.X > 0.05f)
        {
            SetFacingX(1f);
            return;
        }

        if (_state == PlayerState.Run)
        {
            SetFacingX(_runDirectionX);
            return;
        }

        if (Mathf.Abs(Velocity.X) > 12f)
        {
            SetFacingX(Velocity.X);
        }
    }

    private void SnapFacingForAttack(Vector2 input)
    {
        if (input.X < -0.05f)
        {
            SetFacingX(-1f);
            return;
        }

        if (input.X > 0.05f)
        {
            SetFacingX(1f);
            return;
        }

        if (Mathf.Abs(Velocity.X) > 1f)
        {
            SetFacingX(Velocity.X);
        }
    }

    private void UpdateStaminaRegen(float dt)
    {
        if (_staminaRegenDelay > 0f || _stamina >= MaxStamina)
        {
            return;
        }

        var inCombat = _state is PlayerState.LightAttack or PlayerState.HeavyAttack or PlayerState.HitStun or PlayerState.Dash or PlayerState.Run or PlayerState.Jump;
        var rate = inCombat ? StaminaRegenInCombat : StaminaRegenOutOfCombat;
        _stamina = Mathf.Min(MaxStamina, _stamina + rate * dt);
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
        _visualRig.Scale = Vector2.One;

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
                PlayerState.Run => new Color("#d07840"),
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

        _pixelSprite?.SetFacing(_facingX);
        var dashFlash = _state == PlayerState.Dash && _dashInvulnTimer > 0f;
        var jumpFlash = _state == PlayerState.Jump;
        var slamReady = _state == PlayerState.Jump && _airAttackPending == AirAttackPending.Slam;
        _pixelSprite?.SpeedScale = _state == PlayerState.Run ? 1.55f : 1f;
        _pixelSprite?.Modulate = dashFlash ? new Color(0.85f, 0.92f, 1f, 0.82f)
            : slamReady ? new Color(1f, 0.88f, 0.78f)
            : jumpFlash ? new Color(0.96f, 0.98f, 1f)
            : Colors.White;
        _pixelSprite?.UpdatePresentation(
            (_state == PlayerState.Walk || _state == PlayerState.Run) && Velocity.LengthSquared() > 1f,
            _state is PlayerState.LightAttack or PlayerState.HeavyAttack,
            _state == PlayerState.HeavyAttack,
            _hitFlash > 0f || dashFlash || jumpFlash,
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
        SetKeyAction("jump", Key.Space);
        SetKeyAction("dash", Key.Shift);
        AddKeyAction("aim_bow", Key.R);
        AddKeyAction("artifact_toggle", Key.U);
    }

    private static void SetKeyAction(string action, Key key)
    {
        if (!InputMap.HasAction(action))
        {
            InputMap.AddAction(action);
        }

        foreach (var existingEvent in InputMap.ActionGetEvents(action))
        {
            InputMap.ActionEraseEvent(action, existingEvent);
        }

        InputMap.ActionAddEvent(action, new InputEventKey { Keycode = key });
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
