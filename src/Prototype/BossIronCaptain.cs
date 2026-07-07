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

    protected override int MaxHealthValue => 20;
    protected override float ApproachSpeed => 28f;
    protected override float AttackRange => 44f;
    protected override float AttackCooldownSeconds => 1.2f;
    protected override int AttackDamage => 2;
    protected override Color BodyColor => new("#2e2a30");
    protected override Color ApproachColor => new("#3d3840");
    protected override Color AttackColor => new("#b51f1f");
    protected override EnemyVisualArchetype VisualArchetype => EnemyVisualArchetype.IronCaptain;

    protected override bool UsePixelSprite => true;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup("boss");
        Scale = new Vector2(1.38f, 1.38f);
    }

    protected override void OnDefeated()
    {
        foreach (var node in GetTree().GetNodesInGroup("boss_director"))
        {
            if (node is BossDirector director)
            {
                director.OnBossDefeated();
                return;
            }
        }
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
        AttackPattern.IronCrush => 0.48f,
        AttackPattern.ChargeLunge => 0.16f,
        _ => 0.26f
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
            Velocity = new Vector2(FacingDirection.X * 96f, 0f);
        }
    }
}
