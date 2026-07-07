using Godot;
using System;

public static class AudioLibrary
{
    public static AudioStream Resolve(string resourcePath, Func<AudioStream> fallback)
    {
        if (ResourceLoader.Exists(resourcePath))
        {
            return ResourceLoader.Load<AudioStream>(resourcePath);
        }

        return fallback();
    }
}
