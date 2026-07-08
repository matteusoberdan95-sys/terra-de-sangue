using Godot;

public static class ExternalSpriteSheetArt
{
    public const int FrameCount = 8;
    public const int FrameSize = 256;
    public const float DefaultExternalScale = 0.30f;

    public static bool HasWalkSheet(string walkPath)
    {
        return FileAccess.FileExists(walkPath);
    }

    public static SpriteFrames BuildEnemyFrames(
        string walkPath,
        string? idlePath = null,
        string? attackPath = null,
        string? hitPath = null,
        string? deathPath = null)
    {
        var walkSheet = LoadSheet(walkPath);
        var idleSheet = ResolveSheet(idlePath, walkSheet);
        var attackSheet = ResolveSheet(attackPath, walkSheet);
        var hitSheet = ResolveSheet(hitPath, walkSheet);

        var frames = new SpriteFrames();

        AddLoopingFrames(frames, "idle", idleSheet, 6f);
        AddLoopingFrames(frames, "walk", walkSheet, 10f);
        AddLoopingFrames(frames, "attack", attackSheet, 14f);
        AddLoopingFrames(frames, "hit", hitSheet, 12f);

        if (!string.IsNullOrEmpty(deathPath) && FileAccess.FileExists(deathPath))
        {
            AddLoopingFrames(frames, "death", LoadSheet(deathPath), 10f, loop: false);
        }

        return frames;
    }

    public static void AddLoopingFrames(
        SpriteFrames frames,
        string animation,
        Texture2D sheet,
        float speed,
        bool loop = true)
    {
        frames.AddAnimation(animation);
        frames.SetAnimationSpeed(animation, speed);
#pragma warning disable CS0618
        frames.SetAnimationLoop(animation, loop);
#pragma warning restore CS0618

        for (var i = 0; i < FrameCount; i++)
        {
            frames.AddFrame(animation, BuildSheetFrame(sheet, i));
        }
    }

    public static void AddFrameRange(
        SpriteFrames frames,
        string animation,
        Texture2D sheet,
        int startFrame,
        int frameCount,
        float speed,
        bool loop = false)
    {
        frames.AddAnimation(animation);
        frames.SetAnimationSpeed(animation, speed);
#pragma warning disable CS0618
        frames.SetAnimationLoop(animation, loop);
#pragma warning restore CS0618

        for (var i = 0; i < frameCount; i++)
        {
            frames.AddFrame(animation, BuildSheetFrame(sheet, startFrame + i));
        }
    }

    public static Texture2D LoadSheet(string path)
    {
        var image = Image.LoadFromFile(path);
        return ImageTexture.CreateFromImage(image);
    }

    public static Texture2D ResolveSheet(string? path, Texture2D fallback)
    {
        return !string.IsNullOrEmpty(path) && FileAccess.FileExists(path)
            ? LoadSheet(path)
            : fallback;
    }

    public static Texture2D BuildSheetFrame(Texture2D sheet, int frameIndex)
    {
        return new AtlasTexture
        {
            Atlas = sheet,
            Region = new Rect2(frameIndex * FrameSize, 0, FrameSize, FrameSize)
        };
    }
}
