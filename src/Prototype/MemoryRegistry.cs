using Godot;
using System.Collections.Generic;

public static class MemoryRegistry
{
    private static readonly List<string> Collected = new();

    public static IReadOnlyList<string> All => Collected;

    public static void Collect(string id, string title, string text)
    {
        if (Collected.Contains(id))
        {
            return;
        }

        Collected.Add(id);
        GD.Print($"[Memoria] {title}: {text}");
    }

    public static void Reset()
    {
        Collected.Clear();
    }
}
