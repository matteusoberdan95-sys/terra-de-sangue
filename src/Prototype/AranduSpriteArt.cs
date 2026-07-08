using Godot;

public static class AranduSpriteArt
{
    private const string IdleSheetPath = "res://assets/art/sprites/player/arandu_idle_sheet.png";
    private const string WalkSheetPath = "res://assets/art/sprites/player/arandu_walk_sheet.png";
    private const string AttackLightSheetPath = "res://assets/art/sprites/player/arandu_attack_light_sheet.png";
    private const string AttackHeavySheetPath = "res://assets/art/sprites/player/arandu_attack_heavy_sheet.png";
    private const string RunSheetPath = "res://assets/art/sprites/player/arandu_run_sheet.png";
    private const string HitSheetPath = "res://assets/art/sprites/player/arandu_hit_sheet.png";
    private const string DeathSheetPath = "res://assets/art/sprites/player/arandu_death_sheet.png";
    private const string BowSheetPath = "res://assets/art/sprites/player/arandu_bow_sheet.png";
    private const int ExternalSheetFrameCount = 8;
    private const int ExternalSheetFrameSize = 256;
    private const int Width = 48;
    private const int Height = 56;

    public static SpriteFrames BuildSpriteFrames()
    {
        if (HasExternalWalkSheet())
        {
            return BuildSpriteFramesFromWalkSheet();
        }

        var frames = new SpriteFrames();
        frames.AddAnimation("idle");
        frames.SetAnimationSpeed("idle", 1f);
        frames.AddFrame("idle", BuildFrame(0, 0, false));

        frames.AddAnimation("walk");
        frames.SetAnimationSpeed("walk", 9f);
        for (var i = 0; i < 4; i++)
        {
            frames.AddFrame("walk", BuildFrame(i, 0, false));
        }

        frames.AddAnimation("attack_light");
        frames.SetAnimationSpeed("attack_light", 12f);
        frames.AddFrame("attack_light", BuildFrame(0, 1, false));
        frames.AddFrame("attack_light", BuildFrame(0, 2, false));

        frames.AddAnimation("attack_light_1");
        frames.SetAnimationSpeed("attack_light_1", 14f);
        frames.AddFrame("attack_light_1", BuildFrame(0, 1, false));
        frames.AddFrame("attack_light_1", BuildFrame(1, 1, false));

        frames.AddAnimation("attack_light_2");
        frames.SetAnimationSpeed("attack_light_2", 15f);
        frames.AddFrame("attack_light_2", BuildFrame(0, 2, false));
        frames.AddFrame("attack_light_2", BuildFrame(1, 2, false));

        frames.AddAnimation("attack_light_3");
        frames.SetAnimationSpeed("attack_light_3", 13f);
        frames.AddFrame("attack_light_3", BuildFrame(0, 5, false));
        frames.AddFrame("attack_light_3", BuildFrame(1, 5, false));
        frames.AddFrame("attack_light_3", BuildFrame(2, 5, false));

        frames.AddAnimation("attack_heavy");
        frames.SetAnimationSpeed("attack_heavy", 10f);
        frames.AddFrame("attack_heavy", BuildFrame(0, 3, false));
        frames.AddFrame("attack_heavy", BuildFrame(0, 4, false));

        frames.AddAnimation("attack_heavy_1");
        frames.SetAnimationSpeed("attack_heavy_1", 11f);
        frames.AddFrame("attack_heavy_1", BuildFrame(0, 3, false));
        frames.AddFrame("attack_heavy_1", BuildFrame(1, 3, false));
        frames.AddFrame("attack_heavy_1", BuildFrame(2, 3, false));

        frames.AddAnimation("attack_heavy_2");
        frames.SetAnimationSpeed("attack_heavy_2", 10f);
        frames.AddFrame("attack_heavy_2", BuildFrame(0, 4, false));
        frames.AddFrame("attack_heavy_2", BuildFrame(1, 4, false));
        frames.AddFrame("attack_heavy_2", BuildFrame(2, 4, false));
        frames.AddFrame("attack_heavy_2", BuildFrame(3, 4, false));

        frames.AddAnimation("attack_heavy_3");
        frames.SetAnimationSpeed("attack_heavy_3", 9f);
        frames.AddFrame("attack_heavy_3", BuildFrame(0, 6, false));
        frames.AddFrame("attack_heavy_3", BuildFrame(1, 6, false));
        frames.AddFrame("attack_heavy_3", BuildFrame(2, 6, false));
        frames.AddFrame("attack_heavy_3", BuildFrame(3, 6, false));

        frames.AddAnimation("run_attack_light");
        frames.SetAnimationSpeed("run_attack_light", 14f);
        frames.AddFrame("run_attack_light", BuildFrame(0, 5, false));
        frames.AddFrame("run_attack_light", BuildFrame(0, 5, false));

        frames.AddAnimation("run_attack_heavy");
        frames.SetAnimationSpeed("run_attack_heavy", 11f);
        frames.AddFrame("run_attack_heavy", BuildFrame(0, 6, false));
        frames.AddFrame("run_attack_heavy", BuildFrame(0, 6, false));

        frames.AddAnimation("hit");
        frames.SetAnimationSpeed("hit", 1f);
        frames.AddFrame("hit", BuildFrame(0, 0, true));

        return frames;
    }

    public static bool HasExternalWalkSheet()
    {
        return FileAccess.FileExists(WalkSheetPath);
    }

    public static bool HasExternalRunSheet()
    {
        return FileAccess.FileExists(RunSheetPath);
    }

    /// <summary>
    /// Run sheet gerado como faixa horizontal precisa ser in-place (personagem centrado em cada frame).
    /// Sheets que falham na validacao devem ficar fora de player/ ate regerar.
    /// </summary>
    public static bool UsesDedicatedRunSheet()
    {
        return HasExternalRunSheet();
    }

    public static bool HasExternalHitSheet()
    {
        return FileAccess.FileExists(HitSheetPath);
    }

    public static bool HasExternalDeathSheet()
    {
        return FileAccess.FileExists(DeathSheetPath);
    }

    public static bool HasExternalBowSheet()
    {
        return FileAccess.FileExists(BowSheetPath);
    }

    private static SpriteFrames BuildSpriteFramesFromWalkSheet()
    {
        var frames = new SpriteFrames();
        var walkSheet = LoadSheet(WalkSheetPath);
        var idleSheet = FileAccess.FileExists(IdleSheetPath) ? LoadSheet(IdleSheetPath) : walkSheet;
        var attackLightSheet = FileAccess.FileExists(AttackLightSheetPath) ? LoadSheet(AttackLightSheetPath) : walkSheet;
        var attackHeavySheet = FileAccess.FileExists(AttackHeavySheetPath) ? LoadSheet(AttackHeavySheetPath) : attackLightSheet;
        var runSheet = FileAccess.FileExists(RunSheetPath) ? LoadSheet(RunSheetPath) : walkSheet;
        var hitSheet = FileAccess.FileExists(HitSheetPath) ? LoadSheet(HitSheetPath) : walkSheet;
        var bowSheet = FileAccess.FileExists(BowSheetPath) ? LoadSheet(BowSheetPath) : walkSheet;

        ExternalSpriteSheetArt.AddLoopingFrames(frames, "idle", idleSheet, 6f);
        ExternalSpriteSheetArt.AddLoopingFrames(frames, "walk", walkSheet, 10f);

        if (FileAccess.FileExists(RunSheetPath))
        {
            ExternalSpriteSheetArt.AddLoopingFrames(frames, "run", runSheet, 14f);
        }
        else
        {
            // Fallback: walk no mesmo tamanho, mais rapido — evita sheet de corrida mal recortado.
            ExternalSpriteSheetArt.AddLoopingFrames(frames, "run", walkSheet, 14f);
        }

        ExternalSpriteSheetArt.AddFrameRange(frames, "attack_light_1", attackLightSheet, 0, 2, 16f);

        frames.AddAnimation("attack_light_2");
        frames.SetAnimationSpeed("attack_light_2", 17f);
        frames.AddFrame("attack_light_2", BuildSheetFrame(attackLightSheet, 2));
        frames.AddFrame("attack_light_2", BuildSheetFrame(attackLightSheet, 4));

        frames.AddAnimation("attack_light_3");
        frames.SetAnimationSpeed("attack_light_3", 14f);
        frames.AddFrame("attack_light_3", BuildSheetFrame(attackLightSheet, 5));
        frames.AddFrame("attack_light_3", BuildSheetFrame(attackLightSheet, 6));
        frames.AddFrame("attack_light_3", BuildSheetFrame(attackLightSheet, 4));
        ExternalSpriteSheetArt.AddLoopingFrames(frames, "attack_light", attackLightSheet, 14f);

        ExternalSpriteSheetArt.AddFrameRange(frames, "attack_heavy_1", attackHeavySheet, 0, 4, 11f);
        ExternalSpriteSheetArt.AddFrameRange(frames, "attack_heavy_2", attackHeavySheet, 4, 4, 10f);
        ExternalSpriteSheetArt.AddFrameRange(frames, "attack_heavy_3", attackHeavySheet, 4, 4, 9f);
        ExternalSpriteSheetArt.AddLoopingFrames(frames, "attack_heavy", attackHeavySheet, 11f);

        frames.AddAnimation("run_attack_light");
        frames.SetAnimationSpeed("run_attack_light", 14f);
        frames.AddFrame("run_attack_light", BuildSheetFrame(attackLightSheet, 3));
        frames.AddFrame("run_attack_light", BuildSheetFrame(attackLightSheet, 4));
        frames.AddFrame("run_attack_light", BuildSheetFrame(attackLightSheet, 5));

        frames.AddAnimation("run_attack_heavy");
        frames.SetAnimationSpeed("run_attack_heavy", 11f);
        frames.AddFrame("run_attack_heavy", BuildSheetFrame(attackHeavySheet, 2));
        frames.AddFrame("run_attack_heavy", BuildSheetFrame(attackHeavySheet, 4));
        frames.AddFrame("run_attack_heavy", BuildSheetFrame(attackHeavySheet, 5));

        if (FileAccess.FileExists(BowSheetPath))
        {
            ExternalSpriteSheetArt.AddFrameRange(frames, "bow_draw", bowSheet, 0, 2, 10f);
            ExternalSpriteSheetArt.AddFrameRange(frames, "bow_aim_level", bowSheet, 2, 1, 8f, loop: true);
            ExternalSpriteSheetArt.AddFrameRange(frames, "bow_aim_up", bowSheet, 3, 1, 8f, loop: true);
            ExternalSpriteSheetArt.AddFrameRange(frames, "bow_aim_down", bowSheet, 4, 1, 8f, loop: true);
            ExternalSpriteSheetArt.AddFrameRange(frames, "bow_release", bowSheet, 5, 1, 12f);
            ExternalSpriteSheetArt.AddFrameRange(frames, "bow_recovery", bowSheet, 6, 2, 10f, loop: false);
        }

        frames.AddAnimation("hit");
        frames.SetAnimationSpeed("hit", FileAccess.FileExists(HitSheetPath) ? 12f : 1f);
        if (FileAccess.FileExists(HitSheetPath))
        {
            for (var i = 0; i < ExternalSheetFrameCount; i++)
            {
                frames.AddFrame("hit", BuildSheetFrame(hitSheet, i));
            }
        }
        else
        {
            frames.AddFrame("hit", BuildSheetFrame(walkSheet, 2));
        }

        if (FileAccess.FileExists(DeathSheetPath))
        {
            ExternalSpriteSheetArt.AddLoopingFrames(frames, "death", LoadSheet(DeathSheetPath), 10f, loop: false);
        }

        return frames;
    }

    private static Texture2D LoadSheet(string path)
    {
        var image = Image.LoadFromFile(path);
        return ImageTexture.CreateFromImage(image);
    }

    private static Texture2D BuildSheetFrame(Texture2D sheet, int frameIndex)
    {
        return new AtlasTexture
        {
            Atlas = sheet,
            Region = new Rect2(frameIndex * ExternalSheetFrameSize, 0, ExternalSheetFrameSize, ExternalSheetFrameSize)
        };
    }

    private static Texture2D BuildFrame(int walkPhase, int pose, bool hitFlash)
    {
        var image = PixelCanvas.Create(Width, Height);
        var skin = hitFlash ? new Color("#f0d7aa") : PixelSpritePalette.Skin;
        var skinShadow = hitFlash ? new Color("#e8c89a") : PixelSpritePalette.SkinShadow;

        var legShift = walkPhase switch
        {
            1 => -1,
            3 => 1,
            _ => 0
        };

        PixelCanvas.FillRect(image, 20 + legShift, 44, 6, 10, skinShadow);
        PixelCanvas.FillRect(image, 28 - legShift, 44, 6, 10, skinShadow);
        PixelCanvas.FillRect(image, 18, 24, 18, 22, skin);
        PixelCanvas.FillRect(image, 22, 10, 12, 14, skin);
        PixelCanvas.FillRect(image, 20, 4, 16, 8, PixelSpritePalette.Hair);
        PixelCanvas.FillRect(image, 24, 14, 10, 4, PixelSpritePalette.Paint);
        PixelCanvas.FillRect(image, 20, 30, 14, 6, PixelSpritePalette.Cloth);

        var tacapeX = pose switch
        {
            2 => 34,
            4 => 38,
            3 => 30,
            _ => 26
        };
        var tacapeY = pose switch
        {
            >= 3 => 8,
            2 => 14,
            1 => 18,
            _ => 22
        };
        PixelCanvas.FillRect(image, tacapeX, tacapeY, 14, 5, PixelSpritePalette.Wood);
        PixelCanvas.FillRect(image, tacapeX + 10, tacapeY - 2, 6, 4, PixelSpritePalette.Wood);

        if (pose is 1 or 2)
        {
            PixelCanvas.FillRect(image, 30, 20, 12, 3, new Color("#b51f1f", 0.85f));
        }

        if (pose >= 3 && pose < 5)
        {
            PixelCanvas.FillRect(image, 32, 12, 16, 6, PixelSpritePalette.Wood);
        }

        if (pose == 5)
        {
            PixelCanvas.FillRect(image, 14, 34, 10, 8, skinShadow);
            PixelCanvas.FillRect(image, 32, 22, 14, 5, skin);
            PixelCanvas.FillRect(image, 38, 20, 8, 8, skin);
            PixelCanvas.FillRect(image, 42, 22, 6, 3, new Color("#b51f1f", 0.85f));
        }

        if (pose == 6)
        {
            PixelCanvas.FillRect(image, 16, 38, 12, 8, skinShadow);
            PixelCanvas.FillRect(image, 30, 28, 12, 10, skin);
            PixelCanvas.FillRect(image, 34, 18, 10, 12, skinShadow);
            PixelCanvas.FillRect(image, 26, 14, 18, 6, PixelSpritePalette.Wood);
        }

        return PixelCanvas.ToTexture(image);
    }
}
