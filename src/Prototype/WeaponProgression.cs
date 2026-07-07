using Godot;

public static class WeaponProgression
{
    public const string TierOneMemoryId = "mascara_quebrada";

    private static int _tacapeTier;

    public static int TacapeTier => _tacapeTier;

    public static WeaponProfile CurrentProfile => WeaponProfile.ForTier(_tacapeTier);

    public static void UnlockTier(int tier)
    {
        _tacapeTier = Mathf.Max(_tacapeTier, tier);
    }

    public static void OnMemoryCollected(string memoryId)
    {
        if (memoryId == TierOneMemoryId)
        {
            UnlockTier(1);
        }
    }

    public static void Reset()
    {
        _tacapeTier = 0;
    }
}
