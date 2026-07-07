using Godot;

[GlobalClass]
public partial class MataFechadaDirector : Node
{
    private sealed record SpawnEntry(string Kind, Vector2 Position);

    private static readonly SpawnEntry[][] Encounters =
    [
        [
            new("mercenary", new Vector2(220, 150)),
            new("mercenary", new Vector2(300, 168)),
            new("brute", new Vector2(360, 158))
        ],
        [
            new("brute", new Vector2(250, 145)),
            new("mercenary", new Vector2(320, 172)),
            new("mercenary", new Vector2(390, 158)),
            new("brute", new Vector2(340, 148))
        ],
        [
            new("mercenary", new Vector2(240, 152)),
            new("mercenary", new Vector2(280, 178)),
            new("brute", new Vector2(320, 148)),
            new("mercenary", new Vector2(380, 166)),
            new("brute", new Vector2(350, 162))
        ]
    ];

    private const float IntroSeconds = 5.5f;
    private const float DelaySeconds = 3.5f;
    private const float OutroSeconds = 5f;
    private static readonly Vector2 MemoryPosition = new(280, 162);

    private enum PhaseState
    {
        Intro,
        Encounter,
        Between,
        AwaitingMemory,
        Outro,
        Complete
    }

    private PrototypeArena? _arena;
    private CanvasLayer? _uiLayer;
    private Label? _banner;
    private Label? _status;
    private PhaseState _state = PhaseState.Intro;
    private int _nextEncounter;
    private int _activeEncounter;
    private float _timer;

    public override void _Ready()
    {
        AddToGroup("mata_director");
        _arena = GetParent() as PrototypeArena;
        BuildUi();
        ShowBanner("Mata Fechada", "A escuridao da floresta esconde os ultimos invasores.\nApressar aqui e morrer afogado em folha e ferro.");
        UpdateStatus("Fase 2 — inicio");
        _timer = IntroSeconds;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        if (_state == PhaseState.Complete)
        {
            return;
        }

        _timer -= dt;

        switch (_state)
        {
            case PhaseState.Intro:
                UpdateStatus($"Mata Fechada — inimigos em {Mathf.CeilToInt(Mathf.Max(0f, _timer))}s");
                if (_timer <= 0f)
                {
                    HideBanner();
                    StartEncounter(0);
                }

                break;

            case PhaseState.Encounter:
                if (CountEnemies() > 0)
                {
                    return;
                }

                if (_nextEncounter < Encounters.Length)
                {
                    _state = PhaseState.Between;
                    _timer = DelaySeconds;
                    ShowBetweenBanner(_activeEncounter);
                    UpdateStatus($"Encontro {_activeEncounter} limpo — respire, proximo em {Mathf.CeilToInt(_timer)}s");
                }
                else
                {
                    BeginMemoryPhase();
                }

                break;

            case PhaseState.Between:
                UpdateStatus($"Encontro {_activeEncounter} limpo — respire, proximo em {Mathf.CeilToInt(Mathf.Max(0f, _timer))}s");
                if (_timer <= 0f)
                {
                    StartEncounter(_nextEncounter);
                }

                break;

            case PhaseState.AwaitingMemory:
                UpdateStatus("Memoria da mata — aproxime-se e encoste");
                break;

            case PhaseState.Outro:
                UpdateStatus($"Fase 2 concluida em {Mathf.CeilToInt(Mathf.Max(0f, _timer))}s");
                if (_timer <= 0f)
                {
                    _state = PhaseState.Complete;
                    ShowBanner("Mata atravessada", "A floresta fecha o caminho, mas nao o luto.");
                    GameFlow.RequestMataComplete(this);
                }

                break;
        }
    }

    public void OnMemoryCollected(string memoryId, string title, string memoryText)
    {
        if (_state != PhaseState.AwaitingMemory)
        {
            return;
        }

        _state = PhaseState.Outro;
        _timer = OutroSeconds;
        MemoryRegistry.Collect(memoryId, title, memoryText);
        ShowBanner(title, memoryText);
        UpdateStatus($"Fase 2 concluida em {Mathf.CeilToInt(_timer)}s");
    }

    private void StartEncounter(int index)
    {
        if (_arena is null || index >= Encounters.Length)
        {
            BeginMemoryPhase();
            return;
        }

        HideBanner();

        foreach (var entry in Encounters[index])
        {
            _arena.SpawnEnemy(entry.Kind, entry.Position);
        }

        if (index == 0)
        {
            _arena.SpawnArrowPickup(new Vector2(200, 154), 3);
        }
        else if (index == 1)
        {
            CombatAudio.Get(this)?.PlayEncounterPulse();
            _arena.SpawnArrowPickup(new Vector2(180, 160), 2);
        }
        else if (index == 2)
        {
            CombatAudio.Get(this)?.PlayEncounterPulse();
            _arena.SpawnArrowPickup(new Vector2(220, 156), 2);
            _arena.SpawnArtifactPickup(new Vector2(260, 152), ArtifactKind.IronKnife);
        }

        _activeEncounter = index + 1;
        _nextEncounter = index + 1;
        _state = PhaseState.Encounter;
        UpdateStatus($"Encontro {_activeEncounter}/{Encounters.Length}");
    }

    private void BeginMemoryPhase()
    {
        HideBanner();
        _arena?.SpawnMemoryPickup(MemoryPosition, "semente_negra",
            "Memoria: Semente Negra",
            "Algo antigo germina na terra molhada.");
        _state = PhaseState.AwaitingMemory;
        UpdateStatus("Memoria da mata — aproxime-se e encoste");
    }

    private void ShowBetweenBanner(int clearedEncounter)
    {
        var (title, body) = clearedEncounter switch
        {
            1 => ("Trilha estreita", "Os invasores recuam, mas a mata nao abre passagem facil."),
            2 => ("Ultimo bosque", "O cheiro de ferro ainda paira entre as arvores."),
            _ => ("Mata Fechada", "A floresta observa.")
        };
        ShowBanner(title, body);
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
        _uiLayer = new CanvasLayer { Name = "MataUi", Layer = 8 };

        _status = new Label
        {
            Name = "MataStatus",
            Position = new Vector2(8, 58)
        };
        _status.AddThemeColorOverride("font_color", new Color("#7f9a72"));
        _uiLayer.AddChild(_status);

        _banner = new Label
        {
            Name = "MataBanner",
            Position = new Vector2(48, 88),
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(384, 0)
        };
        _banner.AddThemeColorOverride("font_color", new Color("#9ab58a"));
        _uiLayer.AddChild(_banner);

        AddChild(_uiLayer);
    }

    private void ShowBanner(string title, string body)
    {
        _banner!.Visible = true;
        _banner.Text = $"{title}\n{body}";
    }

    private void HideBanner() => _banner!.Visible = false;
    private void UpdateStatus(string text) => _status!.Text = text;
}
