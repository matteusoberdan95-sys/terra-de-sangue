using Godot;

[GlobalClass]
public partial class WaveSpawner : Node
{
    private sealed record SpawnEntry(string Kind, Vector2 Position);

    private static readonly SpawnEntry[][] Waves =
    [
        [new("mercenary", new Vector2(260, 148)), new("mercenary", new Vector2(340, 176)), new("mercenary", new Vector2(430, 156))],
        [new("mercenary", new Vector2(220, 140)), new("mercenary", new Vector2(400, 180)), new("brute", new Vector2(310, 168))],
        [new("brute", new Vector2(280, 150)), new("brute", new Vector2(420, 170)), new("mercenary", new Vector2(350, 138))]
    ];

    private const float InterWaveDelaySeconds = 2.4f;

    private PrototypeArena? _arena;
    private Label? _waveLabel;
    private int _nextWaveIndex;
    private int _activeWaveNumber;
    private bool _waveActive;
    private bool _betweenWaves;
    private float _interWaveTimer;

    public override void _Ready()
    {
        _arena = GetParent() as PrototypeArena;
        BuildWaveLabel();
        SpawnWave(0);
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;

        if (!_waveActive)
        {
            return;
        }

        if (CountLivingEnemies() > 0)
        {
            return;
        }

        if (!_betweenWaves)
        {
            _betweenWaves = true;
            _interWaveTimer = InterWaveDelaySeconds;
        }

        _interWaveTimer -= dt;
        if (_nextWaveIndex < Waves.Length)
        {
            UpdateWaveLabel($"Onda {_activeWaveNumber} limpa — proxima em {Mathf.CeilToInt(_interWaveTimer)}s");
        }
        else
        {
            UpdateWaveLabel("Todas as ondas derrotadas");
            _waveActive = false;
            return;
        }

        if (_interWaveTimer > 0f)
        {
            return;
        }

        _betweenWaves = false;
        SpawnWave(_nextWaveIndex);
    }

    private void SpawnWave(int waveIndex)
    {
        if (waveIndex >= Waves.Length || _arena is null)
        {
            _waveActive = false;
            UpdateWaveLabel("Todas as ondas derrotadas");
            return;
        }

        foreach (var entry in Waves[waveIndex])
        {
            _arena.SpawnEnemy(entry.Kind, entry.Position);
        }

        _activeWaveNumber = waveIndex + 1;
        _nextWaveIndex = waveIndex + 1;
        _waveActive = true;
        _betweenWaves = false;
        UpdateWaveLabel($"Onda {_activeWaveNumber}/{Waves.Length}");
    }

    private int CountLivingEnemies()
    {
        var count = 0;
        foreach (var node in GetTree().GetNodesInGroup("enemy"))
        {
            if (node is EnemyBase enemy && enemy.IsAlive)
            {
                count++;
            }
        }

        return count;
    }

    private void BuildWaveLabel()
    {
        _waveLabel = new Label
        {
            Name = "WaveLabel",
            Position = new Vector2(-360, -248)
        };
        _waveLabel.AddThemeColorOverride("font_color", new Color("#e0b75d"));
        AddChild(_waveLabel);
    }

    private void UpdateWaveLabel(string text)
    {
        if (_waveLabel is not null)
        {
            _waveLabel.Text = text;
        }
    }
}
