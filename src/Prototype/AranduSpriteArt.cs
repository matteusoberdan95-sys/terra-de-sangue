using Godot;

public static class AranduSpriteArt
{
    private const int Width = 48;
    private const int Height = 56;

    public static SpriteFrames BuildSpriteFrames()
    {
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

        frames.AddAnimation("attack_heavy");
        frames.SetAnimationSpeed("attack_heavy", 10f);
        frames.AddFrame("attack_heavy", BuildFrame(0, 3, false));
        frames.AddFrame("attack_heavy", BuildFrame(0, 4, false));

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
