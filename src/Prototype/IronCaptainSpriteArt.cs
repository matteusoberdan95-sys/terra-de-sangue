using Godot;

public static class IronCaptainSpriteArt
{
    private const int Width = 56;
    private const int Height = 64;

    public static SpriteFrames BuildSpriteFrames()
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
        var body = hitFlash ? new Color("#f0d7aa") : PixelSpritePalette.SergeantBody;
        var legShift = walkPhase switch
        {
            1 => -1,
            3 => 1,
            _ => 0
        };

        PixelCanvas.FillRect(image, 18 + legShift, 52, 7, 8, PixelSpritePalette.SergeantCoat);
        PixelCanvas.FillRect(image, 30 - legShift, 52, 7, 8, PixelSpritePalette.SergeantCoat);
        PixelCanvas.FillRect(image, 8, 20, 10, 20, PixelSpritePalette.SergeantCoat);
        PixelCanvas.FillRect(image, 14, 22, 24, 26, body);
        PixelCanvas.FillRect(image, 8, 22, 8, 10, PixelSpritePalette.SergeantMetal);
        PixelCanvas.FillRect(image, 36, 20, 8, 12, PixelSpritePalette.SergeantMetal);
        PixelCanvas.FillRect(image, 14, 32, 22, 5, PixelSpritePalette.Cloth);
        PixelCanvas.FillRect(image, 18, 8, 14, 14, new Color("#4a4248"));
        PixelCanvas.FillRect(image, 12, 0, 24, 6, new Color("#1a1418"));
        PixelCanvas.FillRect(image, 30, -2, 5, 12, PixelSpritePalette.Cloth);
        PixelCanvas.FillRect(image, 6, 26, 14, 4, PixelSpritePalette.BruteChain);

        var weaponX = pose switch
        {
            2 => 36,
            1 => 30,
            _ => 24
        };
        var weaponY = pose switch
        {
            2 => 8,
            1 => 14,
            _ => 20
        };
        PixelCanvas.FillRect(image, weaponX, weaponY, 16, 4, PixelSpritePalette.BruteChain);
        PixelCanvas.FillRect(image, weaponX + 12, weaponY - 4, 8, 12, PixelSpritePalette.SergeantMetal);

        if (pose == 2)
        {
            PixelCanvas.FillRect(image, 32, 22, 14, 3, new Color("#b51f1f", 0.9f));
        }

        return PixelCanvas.ToTexture(image);
    }
}
