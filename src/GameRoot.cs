using Godot;

[GlobalClass]
public partial class GameRoot : Node2D
{
    public static GameRoot? Instance { get; private set; }

    private Node? _currentLevel;
    private CanvasLayer? _transitionLayer;
    private Label? _transitionBanner;
    private CanvasLayer? _completeLayer;
    private string _pendingScenePath = string.Empty;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public override void _Ready()
    {
        AddToGroup("game_flow");
        Engine.MaxFps = 60;
        MemoryRegistry.Reset();
        WeaponProgression.Reset();
        AddChild(new CombatHud { Name = "CombatHud" });
        BuildTransitionBanner();
        LoadLevel("res://scenes/levels/AldeiaEmCinzas.tscn");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true, Echo: false } key
            && key.Keycode == Key.R
            && _completeLayer is not null)
        {
            HideSliceCompleteScreen();
            MemoryRegistry.Reset();
            WeaponProgression.Reset();
            LoadLevel("res://scenes/levels/AldeiaEmCinzas.tscn");
            GetViewport().SetInputAsHandled();
        }
    }

    public void OnAldeiaPhaseComplete()
    {
        RequestLevel("res://scenes/levels/IronCaptainArena.tscn");
    }

    public void OnBossVictory()
    {
        RequestLevel("res://scenes/levels/MataFechada.tscn");
    }

    public void OnMataFechadaComplete()
    {
        GD.Print("Terra Sangrada: progressao das fases 1-2 concluida no prototipo.");
        ShowSliceCompleteScreen();
    }

    public void RequestLevel(string scenePath)
    {
        if (string.IsNullOrEmpty(scenePath))
        {
            return;
        }

        HideSliceCompleteScreen();
        _pendingScenePath = scenePath;
        ShowTransitionBanner(scenePath);
        CallDeferred(nameof(DeferredLoadLevel));
    }

    public void DeferredLoadLevel()
    {
        if (string.IsNullOrEmpty(_pendingScenePath))
        {
            return;
        }

        var scenePath = _pendingScenePath;
        _pendingScenePath = string.Empty;
        LoadLevel(scenePath);
    }

    public void LoadLevel(string scenePath)
    {
        var packed = ResourceLoader.Load<PackedScene>(scenePath);
        if (packed is null)
        {
            GD.PrintErr($"Terra Sangrada: falha ao carregar cena {scenePath}");
            HideTransitionBanner();
            return;
        }

        if (_currentLevel is not null)
        {
            RemoveChild(_currentLevel);
            _currentLevel.Free();
            _currentLevel = null;
        }

        _currentLevel = packed.Instantiate();
        AddChild(_currentLevel);
        HideTransitionBanner();
        GD.Print($"Terra Sangrada: nivel carregado {scenePath}");
    }

    private void ShowSliceCompleteScreen()
    {
        HideSliceCompleteScreen();

        _completeLayer = new CanvasLayer { Name = "SliceCompleteLayer", Layer = 20 };

        var backdrop = new ColorRect
        {
            Color = new Color("#0d1410", 0.82f),
            Position = Vector2.Zero,
            Size = new Vector2(480, 270)
        };
        _completeLayer.AddChild(backdrop);

        var title = new Label
        {
            Text = "Vertical slice concluido",
            Position = new Vector2(96, 72),
            HorizontalAlignment = HorizontalAlignment.Center,
            CustomMinimumSize = new Vector2(288, 0)
        };
        title.AddThemeColorOverride("font_color", new Color("#e0b75d"));
        title.AddThemeFontSizeOverride("font_size", 16);
        _completeLayer.AddChild(title);

        var body = new Label
        {
            Text = "Aldeia em Cinzas -> Capitao do Ferro -> Mata Fechada.\n\nA proxima regiao do prototipo ainda nao existe.\nPressione R para rejogar desde a Aldeia.",
            Position = new Vector2(48, 104),
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(384, 0)
        };
        body.AddThemeColorOverride("font_color", new Color("#9ab58a"));
        _completeLayer.AddChild(body);

        AddChild(_completeLayer);
    }

    private void HideSliceCompleteScreen()
    {
        _completeLayer?.QueueFree();
        _completeLayer = null;
    }

    private void BuildTransitionBanner()
    {
        _transitionLayer = new CanvasLayer { Name = "TransitionLayer", Layer = 12 };
        _transitionBanner = new Label
        {
            Name = "TransitionBanner",
            Visible = false,
            HorizontalAlignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart,
            CustomMinimumSize = new Vector2(420, 0),
            Position = new Vector2(30, 110)
        };
        _transitionBanner.AddThemeColorOverride("font_color", new Color("#f0d7aa"));
        _transitionBanner.AddThemeFontSizeOverride("font_size", 16);
        _transitionLayer.AddChild(_transitionBanner);
        AddChild(_transitionLayer);
    }

    private void ShowTransitionBanner(string scenePath)
    {
        if (_transitionBanner is null)
        {
            return;
        }

        var label = scenePath.Contains("MataFechada")
            ? "Entrando na Mata Fechada..."
            : scenePath.Contains("IronCaptain")
                ? "Entrando na arena do Capitao..."
                : "Carregando proxima area...";
        _transitionBanner.Text = label;
        _transitionBanner.Visible = true;
    }

    private void HideTransitionBanner()
    {
        if (_transitionBanner is not null)
        {
            _transitionBanner.Visible = false;
        }
    }
}
