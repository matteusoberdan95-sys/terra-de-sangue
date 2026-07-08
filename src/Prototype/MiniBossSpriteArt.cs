using Godot;

public static class MiniBossSpriteArt
{
    private const string IdleSheetPath = "res://assets/art/sprites/enemies/sergeant_idle_sheet.png";
    private const string WalkSheetPath = "res://assets/art/sprites/enemies/sergeant_walk_sheet.png";
    private const string AttackSheetPath = "res://assets/art/sprites/enemies/sergeant_attack_sheet.png";
    private const string HitSheetPath = "res://assets/art/sprites/enemies/sergeant_hit_sheet.png";
    private const string DeathSheetPath = "res://assets/art/sprites/enemies/sergeant_death_sheet.png";
    private const int Width = 50;
    private const int Height = 60;

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
        frames.SetAnimationSpeed("walk", 8f);
        for (var i = 0; i < 4; i++)
        {
            frames.AddFrame("walk", BuildFrame(i, 0, false));
        }

        frames.AddAnimation("attack");
        frames.SetAnimationSpeed("attack", 10f);
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
        var body = hitFlash ? new Color("#f0d7aa") : PixelSpritePalette.SergeantBody;
        var legShift = walkPhase switch
        {
            1 => -1,
            3 => 1,
            _ => 0
        };

        PixelCanvas.FillRect(image, 16 + legShift, 48, 6, 8, PixelSpritePalette.SergeantCoat);
        PixelCanvas.FillRect(image, 26 - legShift, 48, 6, 8, PixelSpritePalette.SergeantCoat);
        PixelCanvas.FillRect(image, 10, 18, 8, 18, PixelSpritePalette.SergeantCoat);
        PixelCanvas.FillRect(image, 14, 20, 20, 24, body);
        PixelCanvas.FillRect(image, 10, 24, 6, 8, PixelSpritePalette.SergeantMetal);
        PixelCanvas.FillRect(image, 16, 30, 18, 4, PixelSpritePalette.Cloth);
        PixelCanvas.FillRect(image, 18, 8, 12, 12, new Color("#5a4a48"));
        PixelCanvas.FillRect(image, 14, 2, 20, 5, new Color("#1a1418"));
        PixelCanvas.FillRect(image, 28, 0, 4, 10, PixelSpritePalette.Cloth);
        PixelCanvas.FillRect(image, 28, 16, 10, 10, body);

        var saberX = pose switch
        {
            2 => 32,
            1 => 26,
            _ => 22
        };
        var saberY = pose switch
        {
            2 => 10,
            1 => 14,
            _ => 18
        };
        PixelCanvas.FillRect(image, saberX, saberY, 14, 3, PixelSpritePalette.SergeantMetal);
        PixelCanvas.FillRect(image, saberX + 10, saberY - 2, 4, 8, PixelSpritePalette.Iron);

        if (pose == 2)
        {
            PixelCanvas.FillRect(image, 30, 18, 12, 2, new Color("#e0b75d", 0.9f));
        }

        return PixelCanvas.ToTexture(image);
    }
}
