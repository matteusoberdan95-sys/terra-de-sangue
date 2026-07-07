using Godot;

public enum BleedLevel
{
    None,
    Light,
    Heavy
}

public static class BleedDefs
{
    public static float Dps(BleedLevel level)
    {
        return level switch
        {
            BleedLevel.Light => 0.5f,
            BleedLevel.Heavy => 1f,
            _ => 0f
        };
    }

    public static float Duration(BleedLevel level)
    {
        return level switch
        {
            BleedLevel.Light => 4f,
            BleedLevel.Heavy => 5f,
            _ => 0f
        };
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

        if (level > Level)
        {
            Level = level;
        }

        TimeRemaining = Mathf.Max(TimeRemaining, BleedDefs.Duration(level));
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

    private float _damageAccumulator;
}
