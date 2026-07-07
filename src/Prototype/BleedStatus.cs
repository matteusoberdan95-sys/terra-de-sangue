using Godot;

public enum BleedLevel
{
    None,
    Light,
    Heavy,
    Hemorrhage
}

public static class BleedDefs
{
    public static float Dps(BleedLevel level)
    {
        return level switch
        {
            BleedLevel.Light => 0.5f,
            BleedLevel.Heavy => 1f,
            BleedLevel.Hemorrhage => 1.5f,
            _ => 0f
        };
    }

    public static float Duration(BleedLevel level)
    {
        return level switch
        {
            BleedLevel.Light => 4f,
            BleedLevel.Heavy => 5f,
            BleedLevel.Hemorrhage => 6f,
            _ => 0f
        };
    }

    public static float SpeedMultiplier(BleedLevel level)
    {
        return level == BleedLevel.Hemorrhage ? 0.9f : 1f;
    }
}

public sealed class BleedStatus
{
    public BleedLevel Level { get; private set; } = BleedLevel.None;

    public float TimeRemaining { get; private set; }

    public bool IsActive => Level != BleedLevel.None && TimeRemaining > 0f;

    public void Apply(BleedLevel level)
    {
        if (level == BleedLevel.None)
        {
            return;
        }

        Level = StackLevel(Level, level);
        TimeRemaining = Mathf.Max(TimeRemaining, BleedDefs.Duration(Level));
    }

    public int Tick(float dt)
    {
        if (!IsActive)
        {
            return 0;
        }

        TimeRemaining -= dt;
        _damageAccumulator += BleedDefs.Dps(Level) * dt;
        var damage = 0;
        while (_damageAccumulator >= 1f)
        {
            damage++;
            _damageAccumulator -= 1f;
        }

        if (TimeRemaining <= 0f)
        {
            Level = BleedLevel.None;
            TimeRemaining = 0f;
        }

        return damage;
    }

    public void Clear()
    {
        Level = BleedLevel.None;
        TimeRemaining = 0f;
        _damageAccumulator = 0f;
    }

    private static BleedLevel StackLevel(BleedLevel current, BleedLevel incoming)
    {
        if (incoming == BleedLevel.Hemorrhage)
        {
            return BleedLevel.Hemorrhage;
        }

        if (current == BleedLevel.Heavy && incoming == BleedLevel.Light)
        {
            return BleedLevel.Hemorrhage;
        }

        if (current == BleedLevel.Light && incoming == BleedLevel.Heavy)
        {
            return BleedLevel.Heavy;
        }

        return incoming > current ? incoming : current;
    }

    private float _damageAccumulator;
}
