using Godot;

[GlobalClass]
public partial class MataFechadaDirector : Node
{
    private sealed record SpawnEntry(string Kind, Vector2 Position);

    private static readonly SpawnEntry[][] Encounters =
    [
        [new("mercenary", new Vector2(240, 150)), new("brute", new Vector2(360, 170))],
        [new("brute", new Vector2(280, 145)), new("mercenary", new Vector2(400, 165)), new("mercenary", new Vector2(320, 180))]
    ];

    private const float IntroSeconds = 3.2f;
    private const float DelaySeconds = 2f;
    private const float OutroSeconds = 4f;
    private static readonly Vector2 MemoryPosition = new(470, 162);

    private PrototypeArena? _arena;
    private Label? _banner;
    private Label? _status;
    private int _nextEncounter;
    private int _activeEncounter;
    private bool _active;
    private bool _between;
    private float _timer;
    private bool _done;

    public override void _Ready()
    {
        _arena = GetParent() as PrototypeArena;
        BuildUi();
        ShowBanner("Mata Fechada", "A escuridao da floresta esconde os ultimos invasores.");
        UpdateStatus("Fase 2 — inicio");
        _timer = IntroSeconds;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        if (_done)
        {
            return;
        }

        _timer -= dt;

        if (_nextEncounter == 0 && _timer > 0f)
        {
            return;
        }

        if (_nextEncounter == 0 && _timer <= 0f)
        {
            HideBanner();
            StartEncounter(0);
            return;
        }

        if (!_active)
        {
            return;
        }

        if (CountEnemies() > 0)
        {
            return;
        }

        if (_between)
        {
            if (_timer > 0f)
            {
                UpdateStatus($"Encontro {_activeEncounter} limpo — proximo em {Mathf.CeilToInt(_timer)}s");
                return;
            }

            _between = false;
            if (_nextEncounter < Encounters.Length)
            {
                StartEncounter(_nextEncounter);
            }
            else
            {
                FinishPhase();
            }

            return;
        }

        _active = false;
        if (_nextEncounter < Encounters.Length)
        {
            _between = true;
            _timer = DelaySeconds;
        }
        else
        {
            FinishPhase();
        }
    }

    private void StartEncounter(int index)
    {
        if (_arena is null)
        {
            return;
        }

        foreach (var entry in Encounters[index])
        {
            _arena.SpawnEnemy(entry.Kind, entry.Position);
        }

        _activeEncounter = index + 1;
        _nextEncounter = index + 1;
        _active = true;
        UpdateStatus($"Encontro {_activeEncounter}/{Encounters.Length}");
    }

    private void FinishPhase()
    {
        _done = true;
        _arena?.SpawnMemoryPickup(MemoryPosition);
        MemoryRegistry.Collect("semente_negra", "Memoria: Semente Negra", "Algo antigo germina na terra molhada.");
        ShowBanner("Mata atravessada", "A floresta fecha o caminho, mas nao o luto.");
        UpdateStatus("Fase 2 — concluida");
        GetTree().CallGroup("game_flow", nameof(GameRoot.OnMataFechadaComplete));
    }

    private int CountEnemies()
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

    private void BuildUi()
    {
        _banner = new Label
        {
            Position = new Vector2(-120, -118),
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(240, 0)
        };
        _banner.AddThemeColorOverride("font_color", new Color("#9ab58a"));
        AddChild(_banner);

        _status = new Label { Position = new Vector2(-360, -248) };
        _status.AddThemeColorOverride("font_color", new Color("#7f9a72"));
        AddChild(_status);
    }

    private void ShowBanner(string title, string body)
    {
        _banner!.Visible = true;
        _banner.Text = $"{title}\n{body}";
    }

    private void HideBanner() => _banner!.Visible = false;
    private void UpdateStatus(string text) => _status!.Text = text;
}
