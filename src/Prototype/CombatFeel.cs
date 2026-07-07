using Godot;

public static class CombatFeel
{
    public static void ApplyPlayerAttackImpact(PrototypeArena? arena, PlayerAttackKind attackKind)
    {
        if (arena is null)
        {
            return;
        }

        switch (attackKind)
        {
            case PlayerAttackKind.Heavy:
                arena.ApplyCombatImpact(7.5f, 0.07f, 0.055f);
                break;
            case PlayerAttackKind.ComboFinisher:
                arena.ApplyCombatImpact(5.8f, 0.055f, 0.065f);
                break;
            default:
                arena.ApplyCombatImpact(4.2f, 0.038f, 0.09f);
                break;
        }
    }

    public static void ApplyAirSlamImpact(PrototypeArena? arena)
    {
        arena?.ApplyCombatImpact(6.2f, 0.052f, 0.07f);
    }

    public static void ApplyExecuteImpact(PrototypeArena? arena)
    {
        arena?.ApplyCombatImpact(8.5f, 0.085f, 0.05f);
    }

    public static void ApplyEnemyHitOnPlayer(PrototypeArena? arena, int damage)
    {
        var shake = 2.8f + damage * 1.2f;
        var pause = 0.028f + damage * 0.008f;
        arena?.ApplyCombatImpact(shake, pause, 0.12f);
    }

    public static float GetEnemyHitStun(PlayerAttackKind attackKind)
    {
        return attackKind switch
        {
            PlayerAttackKind.Heavy => 0.26f,
            PlayerAttackKind.ComboFinisher => 0.22f,
            _ => 0.17f
        };
    }

    public static Vector2 ScaleKnockback(Vector2 impulse, PlayerAttackKind attackKind)
    {
        return attackKind switch
        {
            PlayerAttackKind.Heavy => impulse * 1.25f,
            PlayerAttackKind.ComboFinisher => impulse * 1.1f,
            _ => impulse
        };
    }
}
