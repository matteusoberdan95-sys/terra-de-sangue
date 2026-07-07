using Godot;

[GlobalClass]
public partial class ImpactFeedback : Node
{
    private AudioStreamPlayer? _hitPlayer;
    private AudioStreamPlayer? _deathPlayer;
    private AudioStreamPlayer? _hurtPlayer;
    private Node2D? _effectsRoot;

    public override void _Ready()
    {
        AddToGroup("impact_feedback");
        _effectsRoot = new Node2D { Name = "EffectsRoot" };
        AddChild(_effectsRoot);

        _hitPlayer = CreatePlayer("HitSfx", PlaceholderSfx.CreateHitThud());
        _deathPlayer = CreatePlayer("DeathSfx", PlaceholderSfx.CreateDeathCrunch());
        _hurtPlayer = CreatePlayer("HurtSfx", PlaceholderSfx.CreatePlayerHurt());
        AddChild(_hitPlayer);
        AddChild(_deathPlayer);
        AddChild(_hurtPlayer);
    }

    public static ImpactFeedback? Get(Node from)
    {
        return from.GetTree().GetFirstNodeInGroup("impact_feedback") as ImpactFeedback;
    }

    public void OnEnemyHit(Vector2 worldPosition, Vector2 impulse, bool isFatal)
    {
        SpawnSplatter(worldPosition, impulse);
        SpawnDecal(worldPosition + new Vector2(0, 10));
        Play(_hitPlayer);

        if (isFatal)
        {
            SpawnSplatter(worldPosition, impulse * 1.4f);
            SpawnDecal(worldPosition + new Vector2((float)GD.RandRange(-8, 8), 12));
            Play(_deathPlayer);
        }
    }

    public void OnPlayerHit(Vector2 worldPosition)
    {
        SpawnSplatter(worldPosition, new Vector2((float)GD.RandRange(-1, 1), -0.4f));
        Play(_hurtPlayer);
    }

    private void SpawnSplatter(Vector2 worldPosition, Vector2 impulse)
    {
        if (_effectsRoot is null)
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
        if (_effectsRoot is null)
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

    private static AudioStreamPlayer CreatePlayer(string name, AudioStream stream)
    {
        return new AudioStreamPlayer
        {
            Name = name,
            Stream = stream,
            VolumeDb = -4f
        };
    }

    private void Play(AudioStreamPlayer? player)
    {
        if (player is null)
        {
            return;
        }

        player.Stop();
        player.Play();
    }
}
