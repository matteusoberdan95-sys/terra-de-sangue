using Godot;

[GlobalClass]
public partial class CombatAudio : Node
{
    private const string SfxRoot = "res://assets/audio/sfx";

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
            PlayerAttackKind.Heavy => Load($"{SfxRoot}/combat/swing_heavy.wav", PlaceholderSfx.CreateHeavySwing),
            PlayerAttackKind.ComboFinisher => Load($"{SfxRoot}/combat/swing_combo.wav", PlaceholderSfx.CreateComboSwing),
            _ => Load($"{SfxRoot}/combat/swing_light.wav", PlaceholderSfx.CreateLightSwing)
        };
        Play(_swingPlayer, stream, 0.94f + (float)GD.RandRange(0, 0.08));
    }

    public void PlayEnemyHit(PlayerAttackKind attackKind, bool isFatal)
    {
        var stream = attackKind switch
        {
            PlayerAttackKind.Heavy => Load($"{SfxRoot}/combat/hit_heavy.wav", PlaceholderSfx.CreateHitHeavy),
            PlayerAttackKind.ComboFinisher => Load($"{SfxRoot}/combat/hit_combo.wav", PlaceholderSfx.CreateHitCombo),
            _ => Load($"{SfxRoot}/combat/hit_light.wav", PlaceholderSfx.CreateHitLight)
        };
        Play(_hitPlayer, stream, 0.92f + (float)GD.RandRange(0, 0.1));

        if (isFatal)
        {
            Play(_deathPlayer, Load($"{SfxRoot}/combat/death.wav", PlaceholderSfx.CreateDeathCrunch), 0.9f + (float)GD.RandRange(0, 0.06));
        }
    }

    public void PlayExecution(ExecutionStyle style)
    {
        var stream = style switch
        {
            ExecutionStyle.GutRip => Load($"{SfxRoot}/combat/execute_gut.wav", PlaceholderSfx.CreateGutRip),
            ExecutionStyle.SkullCrush => Load($"{SfxRoot}/combat/execute_skull.wav", PlaceholderSfx.CreateSkullCrush),
            _ => Load($"{SfxRoot}/combat/execute_decap.wav", PlaceholderSfx.CreateExecutionCrunch)
        };
        Play(_executePlayer, stream, 1f);
        Play(_deathPlayer, Load($"{SfxRoot}/combat/dismember.wav", PlaceholderSfx.CreateDismemberRip), 0.95f);
    }

    public void PlayPlayerHurt(int damage)
    {
        var stream = damage >= 2
            ? Load($"{SfxRoot}/combat/player_hurt_heavy.wav", PlaceholderSfx.CreatePlayerHurtHeavy)
            : Load($"{SfxRoot}/combat/player_hurt_light.wav", PlaceholderSfx.CreatePlayerHurtLight);
        Play(_hurtPlayer, stream, 0.96f + (float)GD.RandRange(0, 0.05));
    }

    public void PlayEnemyTelegraph(bool heavy = false)
    {
        Play(_telegraphPlayer, Load($"{SfxRoot}/combat/enemy_telegraph.wav", PlaceholderSfx.CreateEnemyTelegraph), heavy ? 0.88f : 1f);
    }

    public void PlayEnemySwing(int damage)
    {
        var stream = damage >= 2
            ? Load($"{SfxRoot}/combat/enemy_swing_heavy.wav", PlaceholderSfx.CreateEnemySwingHeavy)
            : Load($"{SfxRoot}/combat/enemy_swing.wav", PlaceholderSfx.CreateEnemySwing);
        Play(_enemySwingPlayer, stream, 0.94f + (float)GD.RandRange(0, 0.08));
    }

    public void PlayMemoryCollect()
    {
        Play(_uiPlayer, Load($"{SfxRoot}/ui/memory_collect.wav", PlaceholderSfx.CreateMemoryCollect), 1f);
    }

    public void PlayMiniBossIntro()
    {
        Play(_uiPlayer, Load($"{SfxRoot}/ui/miniboss_intro.wav", PlaceholderSfx.CreateMiniBossIntro), 1f);
    }

    public void PlayEncounterPulse()
    {
        Play(_uiPlayer, Load($"{SfxRoot}/ui/encounter_pulse.wav", PlaceholderSfx.CreateEncounterPulse), 0.92f);
    }

    public void PlayDodge()
    {
        Play(_swingPlayer, Load($"{SfxRoot}/combat/dodge.wav", PlaceholderSfx.CreateDodgeRoll), 1f);
    }

    public void PlayRunStart()
    {
        Play(_swingPlayer, Load($"{SfxRoot}/combat/run_burst.wav", PlaceholderSfx.CreateRunBurst), 1.04f);
    }

    public void PlayDashImpulse()
    {
        Play(_swingPlayer, Load($"{SfxRoot}/combat/dash_impulse.wav", PlaceholderSfx.CreateDashImpulse), 1.08f);
    }

    public void PlayJump()
    {
        Play(_swingPlayer, Load($"{SfxRoot}/combat/jump.wav", PlaceholderSfx.CreateJumpHop), 1f);
    }

    public void PlayAirSlam(bool heavy)
    {
        var stream = heavy
            ? Load($"{SfxRoot}/combat/air_hammer_land.wav", PlaceholderSfx.CreateAirSlamLand)
            : Load($"{SfxRoot}/combat/air_slam_land.wav", PlaceholderSfx.CreateAirSlamLand);
        Play(_hitPlayer, stream, heavy ? 0.96f : 1.04f);
    }

    private static AudioStream Load(string path, System.Func<AudioStream> fallback)
    {
        return AudioLibrary.Resolve(path, fallback);
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
