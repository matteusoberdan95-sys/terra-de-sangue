using Godot;

[GlobalClass]
public partial class CombatAudio : Node
{
    private AudioStreamPlayer? _swingPlayer;
    private AudioStreamPlayer? _hitPlayer;
    private AudioStreamPlayer? _deathPlayer;
    private AudioStreamPlayer? _hurtPlayer;
    private AudioStreamPlayer? _executePlayer;
    private AudioStreamPlayer? _telegraphPlayer;
    private AudioStreamPlayer? _enemySwingPlayer;
    private AudioStreamPlayer? _uiPlayer;

    public override void _Ready()
    {
        AddToGroup("combat_audio");
        _swingPlayer = CreatePlayer("SwingSfx", -6f);
        _hitPlayer = CreatePlayer("HitSfx", -4f);
        _deathPlayer = CreatePlayer("DeathSfx", -3f);
        _hurtPlayer = CreatePlayer("HurtSfx", -2f);
        _executePlayer = CreatePlayer("ExecuteSfx", -2f);
        _telegraphPlayer = CreatePlayer("TelegraphSfx", -8f);
        _enemySwingPlayer = CreatePlayer("EnemySwingSfx", -5f);
        _uiPlayer = CreatePlayer("UiSfx", -6f);

        AddChild(_swingPlayer);
        AddChild(_hitPlayer);
        AddChild(_deathPlayer);
        AddChild(_hurtPlayer);
        AddChild(_executePlayer);
        AddChild(_telegraphPlayer);
        AddChild(_enemySwingPlayer);
        AddChild(_uiPlayer);
    }

    public static CombatAudio? Get(Node from)
    {
        return from.GetTree().GetFirstNodeInGroup("combat_audio") as CombatAudio;
    }

    public void PlayPlayerSwing(PlayerAttackKind attackKind)
    {
        var stream = attackKind switch
        {
            PlayerAttackKind.Heavy => PlaceholderSfx.CreateHeavySwing(),
            PlayerAttackKind.ComboFinisher => PlaceholderSfx.CreateComboSwing(),
            _ => PlaceholderSfx.CreateLightSwing()
        };
        Play(_swingPlayer, stream, 0.94f + (float)GD.RandRange(0, 0.08));
    }

    public void PlayEnemyHit(PlayerAttackKind attackKind, bool isFatal)
    {
        var stream = attackKind switch
        {
            PlayerAttackKind.Heavy => PlaceholderSfx.CreateHitHeavy(),
            PlayerAttackKind.ComboFinisher => PlaceholderSfx.CreateHitCombo(),
            _ => PlaceholderSfx.CreateHitLight()
        };
        Play(_hitPlayer, stream, 0.92f + (float)GD.RandRange(0, 0.1));

        if (isFatal)
        {
            Play(_deathPlayer, PlaceholderSfx.CreateDeathCrunch(), 0.9f + (float)GD.RandRange(0, 0.06));
        }
    }

    public void PlayExecution(ExecutionStyle style)
    {
        var stream = style switch
        {
            ExecutionStyle.GutRip => PlaceholderSfx.CreateGutRip(),
            ExecutionStyle.SkullCrush => PlaceholderSfx.CreateSkullCrush(),
            _ => PlaceholderSfx.CreateExecutionCrunch()
        };
        Play(_executePlayer, stream, 1f);
        Play(_deathPlayer, PlaceholderSfx.CreateDismemberRip(), 0.95f);
    }

    public void PlayPlayerHurt(int damage)
    {
        var stream = damage >= 2 ? PlaceholderSfx.CreatePlayerHurtHeavy() : PlaceholderSfx.CreatePlayerHurtLight();
        Play(_hurtPlayer, stream, 0.96f + (float)GD.RandRange(0, 0.05));
    }

    public void PlayEnemyTelegraph(bool heavy = false)
    {
        Play(_telegraphPlayer, PlaceholderSfx.CreateEnemyTelegraph(), heavy ? 0.88f : 1f);
    }

    public void PlayEnemySwing(int damage)
    {
        var stream = damage >= 2 ? PlaceholderSfx.CreateEnemySwingHeavy() : PlaceholderSfx.CreateEnemySwing();
        Play(_enemySwingPlayer, stream, 0.94f + (float)GD.RandRange(0, 0.08));
    }

    public void PlayMemoryCollect()
    {
        Play(_uiPlayer, PlaceholderSfx.CreateMemoryCollect(), 1f);
    }

    public void PlayMiniBossIntro()
    {
        Play(_uiPlayer, PlaceholderSfx.CreateMiniBossIntro(), 1f);
    }

    public void PlayEncounterPulse()
    {
        Play(_uiPlayer, PlaceholderSfx.CreateEncounterPulse(), 0.92f);
    }

    private static AudioStreamPlayer CreatePlayer(string name, float volumeDb)
    {
        return new AudioStreamPlayer
        {
            Name = name,
            VolumeDb = volumeDb
        };
    }

    private static void Play(AudioStreamPlayer? player, AudioStream? stream = null, float pitch = 1f)
    {
        if (player is null)
        {
            return;
        }

        if (stream is not null)
        {
            player.Stream = stream;
        }

        if (player.Stream is null)
        {
            return;
        }

        player.PitchScale = pitch;
        player.Stop();
        player.Play();
    }
}
