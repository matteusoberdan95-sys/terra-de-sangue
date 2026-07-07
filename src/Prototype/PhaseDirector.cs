using Godot;

[GlobalClass]
public partial class PhaseDirector : Node
{
    private sealed record SpawnEntry(string Kind, Vector2 Position);

    private static readonly SpawnEntry[][] Encounters =
    [
        [new("mercenary", new Vector2(250, 148)), new("mercenary", new Vector2(360, 168))],
        [new("mercenary", new Vector2(220, 140)), new("mercenary", new Vector2(390, 176)), new("brute", new Vector2(300, 160))],
        [new("brute", new Vector2(270, 150)), new("brute", new Vector2(410, 170)), new("mercenary", new Vector2(340, 138))]
    ];

    private const float IntroSeconds = 3.6f;
    private const float InterEncounterDelaySeconds = 2.2f;
    private const float OutroSeconds = 4f;
    private static readonly Vector2 MemorySpawnPosition = new(480, 164);
    private static readonly Vector2 MiniBossSpawnPosition = new(320, 160);

    private enum PhaseState
    {
        Intro,
        Encounter,
        InterEncounterDelay,
        MiniBoss,
        AwaitingMemory,
        Outro,
        Complete
    }

    private PrototypeArena? _arena;
    private Label? _phaseTitleLabel;
    private Label? _statusLabel;
    private Label? _bannerLabel;
    private PhaseState _state = PhaseState.Intro;
    private int _nextEncounterIndex;
    private int _activeEncounterNumber;
    private bool _encounterActive;
    private bool _betweenEncounters;
    private float _timer;

    public override void _Ready()
    {
        AddToGroup("phase_director");
        _arena = GetParent() as PrototypeArena;
        BuildUi();
        ShowBanner("Aldeia em Cinzas", "A terra ainda fuma. Arandu atravessa as cinzas.");
        UpdateStatus("Fase 1 — inicio");
        _timer = IntroSeconds;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _timer -= dt;

        switch (_state)
        {
            case PhaseState.Intro:
                if (_timer <= 0f)
                {
                    HideBanner();
                    StartEncounter(0);
                }

                break;

            case PhaseState.Encounter:
            case PhaseState.MiniBoss:
                UpdateCombatState(dt);
                break;

            case PhaseState.InterEncounterDelay:
                UpdateStatus($"Encontro {_activeEncounterNumber} limpo — proximo em {Mathf.CeilToInt(_timer)}s");
                if (_timer <= 0f)
                {
                    _betweenEncounters = false;
                    StartEncounter(_nextEncounterIndex);
                }

                break;

            case PhaseState.AwaitingMemory:
                UpdateStatus("Uma memoria pulsa entre as ruinas.");
                break;

            case PhaseState.Outro:
                if (_timer <= 0f)
                {
                    _state = PhaseState.Complete;
                    ShowBanner("Fase concluida", "Aldeia em Cinzas foi atravessada. O Capitao do Ferro espera adiante.");
                    UpdateStatus("Fase 1 — concluida");
                    GetTree().CallGroup("game_flow", nameof(GameRoot.OnAldeiaPhaseComplete));
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
        ShowBanner(title, memoryText);
        UpdateStatus($"Memoria coletada: {memoryId}");
        MemoryRegistry.Collect(memoryId, title, memoryText);
    }

    private void UpdateCombatState(float dt)
    {
        if (!_encounterActive)
        {
            return;
        }

        if (CountLivingEnemies() > 0)
        {
            return;
        }

        _encounterActive = false;

        if (_state == PhaseState.MiniBoss)
        {
            SpawnMemory();
            return;
        }

        if (_nextEncounterIndex < Encounters.Length)
        {
            _betweenEncounters = true;
            _timer = InterEncounterDelaySeconds;
            _state = PhaseState.InterEncounterDelay;
            return;
        }

        StartMiniBoss();
    }

    private void StartEncounter(int encounterIndex)
    {
        if (encounterIndex >= Encounters.Length || _arena is null)
        {
            StartMiniBoss();
            return;
        }

        foreach (var entry in Encounters[encounterIndex])
        {
            _arena.SpawnEnemy(entry.Kind, entry.Position);
        }

        _activeEncounterNumber = encounterIndex + 1;
        _nextEncounterIndex = encounterIndex + 1;
        _state = PhaseState.Encounter;
        _encounterActive = true;
        _betweenEncounters = false;
        UpdateStatus($"Encontro {_activeEncounterNumber}/{Encounters.Length}");
    }

    private void StartMiniBoss()
    {
        if (_arena is null)
        {
            return;
        }

        ShowBanner("Sargento do Ferro", "O ultimo guardiao da retirada ainda nao caiu.");
        _arena.SpawnEnemy("miniboss", MiniBossSpawnPosition);
        _state = PhaseState.MiniBoss;
        _encounterActive = true;
        UpdateStatus("Mini-chefe");
    }

    private void SpawnMemory()
    {
        HideBanner();
        _arena?.SpawnMemoryPickup(MemorySpawnPosition);
        _state = PhaseState.AwaitingMemory;
        UpdateStatus("Aproxime-se da memoria");
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

    private void BuildUi()
    {
        _phaseTitleLabel = new Label
        {
            Name = "PhaseTitleLabel",
            Position = new Vector2(-360, -272),
            Text = "Fase 1 — Aldeia em Cinzas"
        };
        _phaseTitleLabel.AddThemeColorOverride("font_color", new Color("#e0b75d"));
        AddChild(_phaseTitleLabel);

        _statusLabel = new Label
        {
            Name = "StatusLabel",
            Position = new Vector2(-360, -248)
        };
        _statusLabel.AddThemeColorOverride("font_color", new Color("#bb9a5d"));
        AddChild(_statusLabel);

        _bannerLabel = new Label
        {
            Name = "BannerLabel",
            Position = new Vector2(-120, -118),
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(240, 0)
        };
        _bannerLabel.AddThemeColorOverride("font_color", new Color("#f0d7aa"));
        AddChild(_bannerLabel);
    }

    private void ShowBanner(string title, string body)
    {
        if (_bannerLabel is null)
        {
            return;
        }

        _bannerLabel.Visible = true;
        _bannerLabel.Text = $"{title}\n{body}";
    }

    private void HideBanner()
    {
        if (_bannerLabel is not null)
        {
            _bannerLabel.Visible = false;
        }
    }

    private void UpdateStatus(string text)
    {
        if (_statusLabel is not null)
        {
            _statusLabel.Text = text;
        }
    }
}
