using Godot;

public static class BruteSpriteArt
{
    private const string IdleSheetPath = "res://assets/art/sprites/enemies/brute_idle_sheet.png";
    private const string WalkSheetPath = "res://assets/art/sprites/enemies/brute_walk_sheet.png";
    private const string AttackSheetPath = "res://assets/art/sprites/enemies/brute_attack_sheet.png";
    private const string HitSheetPath = "res://assets/art/sprites/enemies/brute_hit_sheet.png";
    private const string DeathSheetPath = "res://assets/art/sprites/enemies/brute_death_sheet.png";
    private const int Width = 52;
    private const int Height = 58;

    public static SpriteFrames BuildSpriteFrames()
    {
        if (HasExternalWalkSheet())
        {
            return ExternalSpriteSheetArt.BuildEnemyFrames(
                WalkSheetPath,
                IdleSheetPath,
                AttackSheetPath,
                HitSheetPath,
                DeathSheetPath);
        }

        return BuildProceduralSpriteFrames();
    }

    public static bool HasExternalWalkSheet()
    {
        return ExternalSpriteSheetArt.HasWalkSheet(WalkSheetPath);
    }

    private static SpriteFrames BuildProceduralSpriteFrames()
    {
        var frames = new SpriteFrames();
        frames.AddAnimation("idle");
        frames.SetAnimationSpeed("idle", 1f);
        frames.AddFrame("idle", BuildFrame(0, 0, false));

        frames.AddAnimation("walk");
        frames.SetAnimationSpeed("walk", 7f);
        for (var i = 0; i < 4; i++)
        {
            frames.AddFrame("walk", BuildFrame(i, 0, false));
        }

        frames.AddAnimation("attack");
        frames.SetAnimationSpeed("attack", 9f);
        frames.AddFrame("attack", BuildFrame(0, 1, false));
        frames.AddFrame("attack", BuildFrame(0, 2, false));

        frames.AddAnimation("hit");
        frames.SetAnimationSpeed("hit", 1f);
        frames.AddFrame("hit", BuildFrame(0, 0, true));

        return frames;
    }

    private static Texture2D BuildFrame(int walkPhase, int pose, bool hitFlash)
    {
        var image = PixelCanvas.Create(Width, Height);
        var body = hitFlash ? new Color("#f0d7aa") : PixelSpritePalette.BruteBody;
        var shadow = hitFlash ? new Color("#e8c89a") : PixelSpritePalette.BruteShadow;
        var legShift = walkPhase switch
        {
            1 => -2,
            3 => 2,
            _ => 0
        };

        PixelCanvas.FillRect(image, 14 + legShift, 46, 8, 10, shadow);
        PixelCanvas.FillRect(image, 28 - legShift, 46, 8, 10, shadow);
        PixelCanvas.FillRect(image, 12, 20, 26, 28, body);
        PixelCanvas.FillRect(image, 16, 6, 18, 16, shadow);
        PixelCanvas.FillRect(image, 18, 2, 14, 6, new Color("#2a2220"));
        PixelCanvas.FillRect(image, 20, 10, 4, 6, new Color("#560f0b", 0.85f));
        PixelCanvas.FillRect(image, 30, 18, 10, 12, shadow);
        PixelCanvas.FillRect(image, 34, 10, 8, 10, PixelSpritePalette.BruteChain);

        var clubX = pose switch
        {
            2 => 34,
            1 => 28,
            _ => 22
        };
        var clubY = pose switch
        {
            2 => 0,
            1 => 8,
            _ => 16
        };
        PixelCanvas.FillRect(image, clubX, clubY, 6, 22, PixelSpritePalette.Wood);
        PixelCanvas.FillRect(image, clubX + 2, clubY - 6, 10, 8, PixelSpritePalette.Wood);

        if (pose == 2)
        {
            PixelCanvas.FillRect(image, 24, 24, 18, 4, new Color("#8f1f17", 0.75f));
        }

        return PixelCanvas.ToTexture(image);
    }
}
