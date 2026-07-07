using Godot;

[GlobalClass]
public partial class BossDirector : Node
{
    private const float IntroSeconds = 4f;
    private const float VictorySeconds = 5f;
    private static readonly Vector2 BossSpawn = new(320, 160);

    private PrototypeArena? _arena;
    private Label? _banner;
    private Label? _status;
    private bool _bossSpawned;
    private bool _victoryShown;
    private float _timer;

    public override void _Ready()
    {
        _arena = GetParent() as PrototypeArena;
        BuildUi();
        ShowBanner("Capitao do Ferro", "Correntes, ferro e cinzas. O invasor nao recua.");
        UpdateStatus("Chefe 1");
        _timer = IntroSeconds;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _timer -= dt;

        if (!_bossSpawned && _timer <= 0f)
        {
            HideBanner();
            _arena?.SpawnEnemy("capitao", BossSpawn);
            _bossSpawned = true;
            UpdateStatus("Derrote o Capitao do Ferro");
            return;
        }

        if (!_bossSpawned || _victoryShown)
        {
            return;
        }

        if (CountLivingBosses() > 0)
        {
            return;
        }

        _victoryShown = true;
        _timer = VictorySeconds;
        MemoryRegistry.Collect("corrente_do_capitao", "Memoria: Corrente do Capitao", "O ferro ainda lembra o peso da invasao.");
        ShowBanner("Capitao derrotado", "A corrente quebrou. Uma memoria pesada fica com Arandu.");
        UpdateStatus("Recompensa narrativa coletada");
        GetTree().CallGroup("game_flow", nameof(GameRoot.OnBossVictory));
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
        _banner = new Label
        {
            Name = "BossBanner",
            Position = new Vector2(-130, -118),
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(260, 0)
        };
        _banner.AddThemeColorOverride("font_color", new Color("#f0d7aa"));
        AddChild(_banner);

        _status = new Label
        {
            Name = "BossStatus",
            Position = new Vector2(-360, -248)
        };
        _status.AddThemeColorOverride("font_color", new Color("#e0b75d"));
        AddChild(_status);
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
