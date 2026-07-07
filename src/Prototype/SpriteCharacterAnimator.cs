using Godot;

[GlobalClass]
public partial class SpriteCharacterAnimator : AnimatedSprite2D
{
    private string _currentAnimation = "idle";

    public void Configure(SpriteFrames frames, Vector2 offset, float scale = 1.2f)
    {
        SpriteFrames = frames;
        Centered = true;
        Position = offset;
        Scale = new Vector2(scale, scale);
        ZIndex = 5;
        Play("idle");
    }

    public void SetFacing(float directionX)
    {
        FlipH = directionX < 0f;
    }

    public void UpdatePresentation(bool moving, bool attacking, bool heavyAttack, bool runLightAttack, bool runHeavyAttack, bool hitFlash, bool dead)
    {
        if (SpriteFrames is null)
        {
            return;
        }

        var next = dead ? "idle"
            : hitFlash ? "hit"
            : runHeavyAttack ? "run_attack_heavy"
            : runLightAttack ? "run_attack_light"
            : attacking ? heavyAttack ? "attack_heavy" : "attack_light"
            : moving ? "walk"
            : "idle";

        if (next != _currentAnimation || !IsPlaying())
        {
            _currentAnimation = next;
            Play(next);
        }

        Modulate = dead ? new Color("#4a2f24") : Colors.White;
    }

    public void UpdateEnemyPresentation(bool moving, bool attacking, bool hitFlash, bool dead)
    {
        if (SpriteFrames is null)
        {
            return;
        }

        var next = dead ? "idle"
            : hitFlash ? "hit"
            : attacking ? "attack"
            : moving ? "walk"
            : "idle";

        if (next != _currentAnimation || !IsPlaying())
        {
            _currentAnimation = next;
            Play(next);
        }

        Modulate = dead ? new Color("#332424") : Colors.White;
    }
}
