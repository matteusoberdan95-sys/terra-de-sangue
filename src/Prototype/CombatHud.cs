using Godot;

[GlobalClass]
public partial class CombatHud : CanvasLayer
{
    private Label? _healthLabel;
    private Label? _staminaLabel;
    private Label? _bossLabel;
    private Label? _memoryLabel;
    private Label? _quiverLabel;
    private Label? _weaponLabel;
    private Label? _artifactLabel;
    private ProgressBar? _healthBar;
    private ProgressBar? _staminaBar;
    private ProgressBar? _bossBar;

    public override void _Ready()
    {
        Layer = 10;
        BuildUi();
    }

    public override void _Process(double delta)
    {
        _ = delta;
        Refresh();
    }

    private void Refresh()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (player is not null && _healthBar is not null && _healthLabel is not null)
        {
            _healthBar.MaxValue = player.MaxHealthValue;
            _healthBar.Value = player.CurrentHealth;
            _healthLabel.Text = $"Vida {player.CurrentHealth}/{player.MaxHealthValue}";
        }

        if (player is not null && _staminaBar is not null && _staminaLabel is not null)
        {
            _staminaBar.MaxValue = player.MaxStaminaValue;
            _staminaBar.Value = player.CurrentStamina;
            _staminaLabel.Text = $"Stamina {Mathf.CeilToInt(player.CurrentStamina)}";
            _staminaBar.Modulate = player.StaminaExhaustedFlash
                ? new Color(1f, 0.45f, 0.4f)
                : Colors.White;
        }

        EnemyBase? boss = null;
        foreach (var node in GetTree().GetNodesInGroup("boss"))
        {
            if (node is EnemyBase enemy && enemy.IsAlive)
            {
                boss = enemy;
                break;
            }
        }

        if (_bossBar is not null && _bossLabel is not null)
        {
            var visible = boss is not null;
            _bossBar.Visible = visible;
            _bossLabel.Visible = visible;
            if (boss is not null)
            {
                _bossBar.MaxValue = boss.MaxHealth;
                _bossBar.Value = boss.HealthRemaining;
                _bossLabel.Text = $"Chefe {boss.HealthRemaining}/{boss.MaxHealth}";
            }
        }

        if (_memoryLabel is not null)
        {
            _memoryLabel.Text = $"Memorias: {MemoryRegistry.All.Count}";
        }

        if (player is not null && _quiverLabel is not null)
        {
            var filled = new string('●', player.ArrowCount);
            var empty = new string('○', player.MaxArrowCount - player.ArrowCount);
            _quiverLabel.Text = $"Flechas {filled}{empty}";
            _quiverLabel.Modulate = player.IsAimingBow
                ? new Color("#f0d7aa")
                : Colors.White;
        }

        if (player is not null && _weaponLabel is not null)
        {
            _weaponLabel.Text = player.WeaponDisplayName;
        }

        if (player is not null && _artifactLabel is not null)
        {
            if (player.ArtifactCharges <= 0)
            {
                _artifactLabel.Visible = false;
            }
            else
            {
                _artifactLabel.Visible = true;
                var status = player.ArtifactEquipped ? "equipada" : "guardada";
                _artifactLabel.Text = $"Faca ({player.ArtifactCharges}) — {status}";
                _artifactLabel.Modulate = player.ArtifactEquipped
                    ? new Color("#e8c4a0")
                    : new Color("#9a8a72");
            }
        }
    }

    private void BuildUi()
    {
        _healthLabel = new Label { Position = new Vector2(12, 8) };
        _healthLabel.AddThemeColorOverride("font_color", new Color("#e0b75d"));
        AddChild(_healthLabel);

        _healthBar = new ProgressBar
        {
            Position = new Vector2(12, 28),
            CustomMinimumSize = new Vector2(120, 10),
            MaxValue = 8,
            Value = 8,
            ShowPercentage = false
        };
        AddChild(_healthBar);

        _staminaLabel = new Label { Position = new Vector2(12, 42) };
        _staminaLabel.AddThemeColorOverride("font_color", new Color("#c9a84a"));
        AddChild(_staminaLabel);

        _staminaBar = new ProgressBar
        {
            Position = new Vector2(12, 62),
            CustomMinimumSize = new Vector2(120, 8),
            MaxValue = 100,
            Value = 100,
            ShowPercentage = false
        };
        AddChild(_staminaBar);

        _bossLabel = new Label { Position = new Vector2(12, 76), Visible = false };
        _bossLabel.AddThemeColorOverride("font_color", new Color("#b51f1f"));
        AddChild(_bossLabel);

        _bossBar = new ProgressBar
        {
            Position = new Vector2(12, 96),
            CustomMinimumSize = new Vector2(160, 12),
            Visible = false,
            ShowPercentage = false
        };
        AddChild(_bossBar);

        _memoryLabel = new Label { Position = new Vector2(12, 116) };
        _memoryLabel.AddThemeColorOverride("font_color", new Color("#bb9a5d"));
        AddChild(_memoryLabel);

        _quiverLabel = new Label { Position = new Vector2(12, 136) };
        _quiverLabel.AddThemeColorOverride("font_color", new Color("#c9a84a"));
        AddChild(_quiverLabel);

        _weaponLabel = new Label { Position = new Vector2(12, 154) };
        _weaponLabel.AddThemeColorOverride("font_color", new Color("#8a7a5a"));
        AddChild(_weaponLabel);

        _artifactLabel = new Label { Position = new Vector2(12, 172), Visible = false };
        _artifactLabel.AddThemeColorOverride("font_color", new Color("#b87070"));
        AddChild(_artifactLabel);
    }
}
