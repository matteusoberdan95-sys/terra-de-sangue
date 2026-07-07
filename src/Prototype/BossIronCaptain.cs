using Godot;

[GlobalClass]
public partial class BossIronCaptain : EnemyBase
{
    private enum AttackPattern
    {
        ChainSwing,
        IronCrush,
        ChargeLunge
    }

    private AttackPattern _currentPattern = AttackPattern.ChainSwing;
    private AttackPattern _nextPattern = AttackPattern.IronCrush;

    protected override int MaxHealthValue => 18;
    protected override float ApproachSpeed => 30f;
    protected override float AttackRange => 44f;
    protected override float AttackCooldownSeconds => 1.15f;
    protected override int AttackDamage => 2;
    protected override Color BodyColor => new("#2e2a30");
    protected override Color ApproachColor => new("#3d3840");
    protected override Color AttackColor => new("#b51f1f");

    public override void _Ready()
    {
        base._Ready();
        AddToGroup("boss");
        Scale = new Vector2(1.42f, 1.42f);
        AddChild(new Polygon2D
        {
            Name = "IronChain",
            Color = new Color("#5f6970"),
            Polygon = new[]
            {
                new Vector2(-18, -6),
                new Vector2(18, -10),
                new Vector2(16, -2),
                new Vector2(-16, 2)
            }
        });
        AddChild(new Polygon2D
        {
            Name = "CaptainCape",
            Color = new Color("#560f0b", 0.8f),
            Polygon = new[]
            {
                new Vector2(-10, 4),
                new Vector2(10, 2),
                new Vector2(14, 18),
                new Vector2(-12, 16)
            }
        });
    }

    protected override void OnAttackPatternStarted()
    {
        _currentPattern = _nextPattern;
        _nextPattern = _currentPattern switch
        {
            AttackPattern.ChainSwing => AttackPattern.IronCrush,
            AttackPattern.IronCrush => AttackPattern.ChargeLunge,
            _ => AttackPattern.ChainSwing
        };
    }

    protected override float GetAttackStartup() => _currentPattern switch
    {
        AttackPattern.IronCrush => 0.42f,
        AttackPattern.ChargeLunge => 0.14f,
        _ => 0.24f
    };

    protected override float GetAttackActive() => _currentPattern switch
    {
        AttackPattern.IronCrush => 0.14f,
        AttackPattern.ChargeLunge => 0.16f,
        _ => 0.11f
    };

    protected override float GetAttackRecovery() => _currentPattern switch
    {
        AttackPattern.IronCrush => 0.36f,
        AttackPattern.ChargeLunge => 0.28f,
        _ => 0.26f
    };

    protected override int GetAttackDamageAmount() => _currentPattern switch
    {
        AttackPattern.IronCrush => 3,
        AttackPattern.ChargeLunge => 2,
        _ => 2
    };

    protected override Vector2 GetAttackHitboxSize() => _currentPattern switch
    {
        AttackPattern.IronCrush => new(52, 30),
        AttackPattern.ChargeLunge => new(44, 24),
        _ => new(40, 22)
    };

    protected override float GetAttackReach() => _currentPattern switch
    {
        AttackPattern.ChargeLunge => 38f,
        AttackPattern.IronCrush => 34f,
        _ => 32f
    };

    protected override void OnAttackActiveFrame(float dt)
    {
        if (_currentPattern == AttackPattern.ChargeLunge)
        {
            Velocity = new Vector2(FacingDirection.X * 110f, 0f);
        }
    }
}
