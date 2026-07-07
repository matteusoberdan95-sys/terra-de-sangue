using Godot;

[GlobalClass]
public partial class BossDirector : Node
{
    private const float IntroSeconds = 4f;
    private const float VictorySeconds = 6f;
    private static readonly Vector2 BossSpawn = new(320, 160);

    private PrototypeArena? _arena;
    private CanvasLayer? _uiLayer;
    private Label? _banner;
    private Label? _status;
    private bool _bossSpawned;
    private bool _victoryShown;
    private bool _transitionTriggered;
    private float _timer;

    public override void _Ready()
    {
        AddToGroup("boss_director");
        _arena = GetParent() as PrototypeArena;
        if (_arena is null)
        {
            GD.PrintErr("BossDirector: pai nao e PrototypeArena.");
        }

        BuildUi();
        ShowBanner("Capitao do Ferro", "Correntes, ferro e cinzas. O invasor nao recua.");
        UpdateStatus("Chefe 1 — aguarde o intro");
        _timer = IntroSeconds;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _timer -= dt;

        if (!_bossSpawned)
        {
            if (_timer <= 0f)
            {
                HideBanner();
                if (_arena is null)
                {
                    GD.PrintErr("BossDirector: arena nao encontrada para spawn do chefe.");
                    return;
                }

                CombatAudio.Get(this)?.PlayMiniBossIntro();
                _arena.SpawnEnemy("capitao", BossSpawn);
                _bossSpawned = true;
                UpdateStatus("Derrote o Capitao do Ferro");
            }

            return;
        }

        if (_victoryShown)
        {
            UpdateStatus(_transitionTriggered
                ? "Entrando na Mata Fechada..."
                : $"Mata Fechada em {Mathf.CeilToInt(Mathf.Max(0f, _timer))}s");

            if (!_transitionTriggered && _timer <= 0f)
            {
                TriggerTransition();
            }

            return;
        }

        if (CountLivingBosses() > 0)
        {
            return;
        }

        BeginVictory();
    }

    public void OnBossDefeated()
    {
        if (!_bossSpawned || _victoryShown)
        {
            return;
        }

        BeginVictory();
    }

    private void BeginVictory()
    {
        if (_victoryShown)
        {
            return;
        }

        _victoryShown = true;
        _timer = VictorySeconds;
        MemoryRegistry.Collect("corrente_do_capitao", "Memoria: Corrente do Capitao", "O ferro ainda lembra o peso da invasao.");
        CombatAudio.Get(this)?.PlayMemoryCollect();
        ShowBanner("Capitao derrotado", "A corrente quebrou. A Mata Fechada aguarda adiante.");
        UpdateStatus($"Mata Fechada em {Mathf.CeilToInt(_timer)}s");
        GD.Print("BossDirector: Capitao derrotado, iniciando contagem para Mata Fechada.");
    }

    private void TriggerTransition()
    {
        _transitionTriggered = true;
        HideBanner();
        UpdateStatus("Entrando na Mata Fechada...");
        GD.Print("BossDirector: solicitando transicao para Mata Fechada.");
        GameFlow.RequestBossVictory(this);
    }

    private int CountLivingBosses()
    {
        var count = 0;
        foreach (var node in GetTree().GetNodesInGroup("boss"))
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
        _uiLayer = new CanvasLayer { Name = "BossUi", Layer = 8 };

        _status = new Label
        {
            Name = "BossStatus",
            Position = new Vector2(8, 58)
        };
        _status.AddThemeColorOverride("font_color", new Color("#e0b75d"));
        _uiLayer.AddChild(_status);

        _banner = new Label
        {
            Name = "BossBanner",
            Position = new Vector2(48, 96),
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(384, 0)
        };
        _banner.AddThemeColorOverride("font_color", new Color("#f0d7aa"));
        _uiLayer.AddChild(_banner);

        AddChild(_uiLayer);
    }

    private void ShowBanner(string title, string body)
    {
        if (_banner is not null)
        {
            _banner.Visible = true;
            _banner.Text = $"{title}\n{body}";
        }
    }

    private void HideBanner()
    {
        if (_banner is not null)
        {
            _banner.Visible = false;
        }
    }

    private void UpdateStatus(string text)
    {
        if (_status is not null)
        {
            _status.Text = text;
        }
    }
}
