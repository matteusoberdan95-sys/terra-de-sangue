using Godot;

[GlobalClass]
public partial class ImpactFeedback : Node
{
    private Node2D? _effectsRoot;

    public override void _Ready()
    {
        AddToGroup("impact_feedback");
        _effectsRoot = new Node2D { Name = "EffectsRoot" };
        AddChild(_effectsRoot);
    }

    public static ImpactFeedback? Get(Node from)
    {
        return from.GetTree().GetFirstNodeInGroup("impact_feedback") as ImpactFeedback;
    }

    public void OnEnemyHit(Vector2 worldPosition, Vector2 impulse, bool isFatal, PlayerAttackKind attackKind)
    {
        SpawnSplatter(worldPosition, impulse);
        SpawnDecal(worldPosition + new Vector2(0, 10));

        if (attackKind == PlayerAttackKind.Heavy)
        {
            SpawnSplatter(worldPosition, impulse * 0.8f);
        }

        CombatAudio.Get(this)?.PlayEnemyHit(attackKind, isFatal);

        if (isFatal)
        {
            SpawnSplatter(worldPosition, impulse * 1.4f);
            SpawnDecal(worldPosition + new Vector2((float)GD.RandRange(-8, 8), 12));
            SpawnPersistentDecal(worldPosition + new Vector2(0, 12));
            ApplyFatalGore(worldPosition, impulse, attackKind);
        }
        else if (attackKind == PlayerAttackKind.Heavy)
        {
            SpawnPersistentDecal(worldPosition + new Vector2((float)GD.RandRange(-4, 4), 11));
        }
    }

    public void OnExecution(Vector2 worldPosition, Vector2 impulse, ExecutionStyle style)
    {
        SpawnSplatter(worldPosition, impulse * 1.8f);
        SpawnSplatter(worldPosition, impulse * 1.2f);
        SpawnPersistentDecal(worldPosition + new Vector2(-6, 14));
        SpawnPersistentDecal(worldPosition + new Vector2(10, 10));
        TrySpawnVegetationStain(worldPosition);

        switch (style)
        {
            case ExecutionStyle.GutRip:
                SpawnDismemberment(worldPosition + new Vector2(0, 4), impulse * 0.7f, LimbKind.TorsoChunk);
                SpawnPersistentDecal(worldPosition + new Vector2(0, 16));
                break;
            case ExecutionStyle.SkullCrush:
                SpawnSplatter(worldPosition + new Vector2(0, -12), impulse * 0.5f);
                SpawnPersistentDecal(worldPosition + new Vector2(0, 8));
                break;
            default:
                SpawnDismemberment(worldPosition + new Vector2(0, -14), impulse * 1.1f, LimbKind.Head);
                break;
        }

        CombatAudio.Get(this)?.PlayExecution(style);
    }

    public void OnPlayerHit(Vector2 worldPosition, int damage)
    {
        SpawnSplatter(worldPosition, new Vector2((float)GD.RandRange(-1, 1), -0.4f));
        CombatAudio.Get(this)?.PlayPlayerHurt(damage);
    }

    public void SpawnDismemberment(Vector2 worldPosition, Vector2 impulse, LimbKind kind)
    {
        if (_effectsRoot is null || !GoreReadability.CanSpawnLimb(this))
        {
            return;
        }

        var limb = new SeveredLimb
        {
            GlobalPosition = worldPosition,
            ZIndex = Mathf.RoundToInt(worldPosition.Y) + 1
        };
        limb.Configure(kind, impulse);
        _effectsRoot.AddChild(limb);
        TrySpawnVegetationStain(worldPosition);
    }

    private void ApplyFatalGore(Vector2 worldPosition, Vector2 impulse, PlayerAttackKind attackKind)
    {
        switch (attackKind)
        {
            case PlayerAttackKind.Heavy:
                SpawnDismemberment(worldPosition + new Vector2(8, -2), impulse * 0.9f, LimbKind.Arm);
                SpawnDismemberment(worldPosition + new Vector2(-6, 2), impulse * 0.75f, LimbKind.TorsoChunk);
                SpawnPersistentDecal(worldPosition + new Vector2(12, 14));
                break;
            case PlayerAttackKind.ComboFinisher:
                SpawnDismemberment(worldPosition + new Vector2(0, -12), impulse, LimbKind.Head);
                SpawnSplatter(worldPosition + new Vector2(0, -16), impulse * 0.6f);
                break;
            case PlayerAttackKind.Light:
                SpawnDismemberment(worldPosition + new Vector2(10, -4), impulse * 0.85f, LimbKind.Arm);
                break;
        }

        TrySpawnVegetationStain(worldPosition);
    }

    private void TrySpawnVegetationStain(Vector2 worldPosition)
    {
        if (_effectsRoot is null)
        {
            return;
        }

        Node2D? nearest = null;
        var nearestDistance = GoreReadability.VegetationStainRadius;

        foreach (var node in GetTree().GetNodesInGroup("vegetation"))
        {
            if (node is not Node2D vegetation)
            {
                continue;
            }

            var distance = vegetation.GlobalPosition.DistanceTo(worldPosition);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = vegetation;
            }
        }

        if (nearest is null)
        {
            return;
        }

        var stain = new VegetationBloodStain
        {
            GlobalPosition = nearest.ToGlobal(new Vector2((float)GD.RandRange(-8, 8), (float)GD.RandRange(-6, 6))),
            ZIndex = nearest.ZIndex + 1
        };
        _effectsRoot.AddChild(stain);
    }

    private void SpawnSplatter(Vector2 worldPosition, Vector2 impulse)
    {
        if (_effectsRoot is null || !GoreReadability.CanSpawnSplatter(this))
        {
            return;
        }

        var splatter = new BloodSplatter
        {
            GlobalPosition = worldPosition
        };
        splatter.Launch(impulse);
        _effectsRoot.AddChild(splatter);
    }

    private void SpawnDecal(Vector2 worldPosition)
    {
        if (_effectsRoot is null || !GoreReadability.CanSpawnTemporaryDecal(this))
        {
            return;
        }

        var decal = new BloodDecal
        {
            GlobalPosition = worldPosition,
            ZIndex = Mathf.RoundToInt(worldPosition.Y) - 1
        };
        _effectsRoot.AddChild(decal);
    }

    private void SpawnPersistentDecal(Vector2 worldPosition)
    {
        if (_effectsRoot is null || !GoreReadability.CanSpawnPersistentDecal(this))
        {
            return;
        }

        var decal = new PersistentBloodDecal
        {
            GlobalPosition = worldPosition,
            ZIndex = Mathf.RoundToInt(worldPosition.Y) - 2
        };
        _effectsRoot.AddChild(decal);
    }
}
