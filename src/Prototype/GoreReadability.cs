using Godot;

public static class GoreReadability
{
    public const int MaxActiveSplatters = 10;
    public const int MaxActiveTemporaryDecals = 14;
    public const int MaxActivePersistentDecals = 18;
    public const int MaxActiveLimbs = 8;
    public const float VegetationStainRadius = 48f;

    public static bool CanSpawnSplatter(Node sceneRoot, int pending = 1)
    {
        return CountNodes<BloodSplatter>(sceneRoot) + pending <= MaxActiveSplatters;
    }

    public static bool CanSpawnTemporaryDecal(Node sceneRoot, int pending = 1)
    {
        return CountNodes<BloodDecal>(sceneRoot) + pending <= MaxActiveTemporaryDecals;
    }

    public static bool CanSpawnPersistentDecal(Node sceneRoot, int pending = 1)
    {
        return CountNodes<PersistentBloodDecal>(sceneRoot) + pending <= MaxActivePersistentDecals;
    }

    public static bool CanSpawnLimb(Node sceneRoot, int pending = 1)
    {
        return CountNodes<SeveredLimb>(sceneRoot) + pending <= MaxActiveLimbs;
    }

    private static int CountNodes<T>(Node root) where T : Node
    {
        var count = 0;
        foreach (var node in root.GetTree().GetNodesInGroup("gore_effect"))
        {
            if (node is T)
            {
                count++;
            }
        }

        return count;
    }
}
