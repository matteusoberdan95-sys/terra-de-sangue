using Godot;

[GlobalClass]
public partial class SpriteCharacterAnimator : AnimatedSprite2D
{
    private string _currentAnimation = "idle";
    private int _lastLightComboStep;
    private int _lastHeavyComboStep;
    private bool _invertFlip;

    public void Configure(SpriteFrames frames, Vector2 offset, float scale = 1.2f)
    {
        SpriteFrames = frames;
        Centered = true;
        Position = offset;
        Scale = new Vector2(scale, scale);
        ZIndex = 5;
        Play("idle");
    }

    public void SetInvertFlip(bool invert)
    {
        _invertFlip = invert;
    }

    public void SetFacing(float directionX)
    {
        var flip = directionX < 0f;
        FlipH = _invertFlip ? !flip : flip;
    }

    public void UpdatePresentation(
        bool moving,
        bool running,
        bool attacking,
        bool heavyAttack,
        bool runLightAttack,
        bool runHeavyAttack,
        int lightComboStep,
        int heavyComboStep,
        bool bowDrawing,
        bool aimingBow,
        float aimDepthOffset,
        bool bowShooting,
        bool bowRecovering,
        bool hitFlash,
        bool dead)
    {
        if (SpriteFrames is null)
        {
            return;
        }

        var next = dead
            ? HasAnimation("death") ? "death" : "idle"
            : hitFlash && HasAnimation("hit") ? "hit"
            : bowShooting && HasAnimation("bow_release") ? "bow_release"
            : bowRecovering && HasAnimation("bow_recovery") ? "bow_recovery"
            : bowDrawing && HasAnimation("bow_draw") ? "bow_draw"
            : aimingBow ? ResolveBowAimAnimation(aimDepthOffset)
            : runHeavyAttack ? "run_attack_heavy"
            : runLightAttack ? "run_attack_light"
            : attacking ? ResolveAttackAnimation(heavyAttack, lightComboStep, heavyComboStep)
            : running && HasAnimation("run") ? "run"
            : moving ? "walk"
            : "idle";

        var forceReplay = attacking
            && (lightComboStep != _lastLightComboStep || heavyComboStep != _lastHeavyComboStep);

        if (next != _currentAnimation || forceReplay)
        {
            _currentAnimation = next;
            Play(next);
            if (forceReplay)
            {
                Frame = 0;
            }
        }

        if (attacking)
        {
            _lastLightComboStep = lightComboStep;
            _lastHeavyComboStep = heavyComboStep;
        }
        else
        {
            _lastLightComboStep = 0;
            _lastHeavyComboStep = 0;
        }

        Modulate = dead && !HasAnimation("death") ? new Color("#4a2f24") : Colors.White;
    }

    public void UpdateEnemyPresentation(bool moving, bool attacking, bool hitFlash, bool dead)
    {
        if (SpriteFrames is null)
        {
            return;
        }

        var next = dead
            ? HasAnimation("death") ? "death" : "idle"
            : hitFlash && HasAnimation("hit") ? "hit"
            : attacking ? "attack"
            : moving ? "walk"
            : "idle";

        if (next != _currentAnimation)
        {
            _currentAnimation = next;
            Play(next);
        }

        Modulate = dead && !HasAnimation("death") ? new Color("#332424") : Colors.White;
    }

    private string ResolveBowAimAnimation(float aimDepthOffset)
    {
        if (aimDepthOffset < -0.15f && HasAnimation("bow_aim_up"))
        {
            return "bow_aim_up";
        }

        if (aimDepthOffset > 0.15f && HasAnimation("bow_aim_down"))
        {
            return "bow_aim_down";
        }

        return HasAnimation("bow_aim_level") ? "bow_aim_level" : "idle";
    }

    private string ResolveAttackAnimation(bool heavyAttack, int lightComboStep, int heavyComboStep)
    {
        if (heavyAttack)
        {
            if (heavyComboStep >= 3 && HasAnimation("attack_heavy_3"))
            {
                return "attack_heavy_3";
            }

            if (heavyComboStep >= 2 && HasAnimation("attack_heavy_2"))
            {
                return "attack_heavy_2";
            }

            if (HasAnimation("attack_heavy_1"))
            {
                return "attack_heavy_1";
            }

            return "attack_heavy";
        }

        return lightComboStep switch
        {
            3 when HasAnimation("attack_light_3") => "attack_light_3",
            2 when HasAnimation("attack_light_2") => "attack_light_2",
            _ when HasAnimation("attack_light_1") => "attack_light_1",
            _ => "attack_light"
        };
    }

    private bool HasAnimation(string name)
    {
        return SpriteFrames?.HasAnimation(name) ?? false;
    }
}
