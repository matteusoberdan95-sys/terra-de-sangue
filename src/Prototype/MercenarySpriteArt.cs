using Godot;

public static class MercenarySpriteArt
{
    private const int Width = 44;
    private const int Height = 52;

    public static SpriteFrames BuildSpriteFrames()
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
        frames.SetAnimationSpeed("attack", 11f);
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
        var body = hitFlash ? new Color("#f0d7aa") : PixelSpritePalette.Iron;
        var legShift = walkPhase switch
        {
            1 => -1,
            3 => 1,
            _ => 0
        };

        PixelCanvas.FillRect(image, 16 + legShift, 42, 6, 8, PixelSpritePalette.Leather);
        PixelCanvas.FillRect(image, 24 - legShift, 42, 6, 8, PixelSpritePalette.Leather);
        PixelCanvas.FillRect(image, 14, 22, 20, 22, body);
        PixelCanvas.FillRect(image, 18, 8, 12, 12, new Color("#6a5048"));
        PixelCanvas.FillRect(image, 16, 4, 16, 5, new Color("#3a3438"));
        PixelCanvas.FillRect(image, 12, 18, 6, 14, PixelSpritePalette.Cape);
        PixelCanvas.FillRect(image, 16, 28, 16, 4, PixelSpritePalette.Leather);

        var musketX = pose switch
        {
            2 => 30,
            1 => 24,
            _ => 20
        };
        PixelCanvas.FillRect(image, musketX, 16, 16, 3, PixelSpritePalette.Iron);
        PixelCanvas.FillRect(image, musketX - 4, 18, 6, 4, PixelSpritePalette.Wood);

        if (pose == 2)
        {
            PixelCanvas.FillRect(image, 28, 20, 10, 2, new Color("#8f1f17", 0.8f));
        }

        return PixelCanvas.ToTexture(image);
    }
}
