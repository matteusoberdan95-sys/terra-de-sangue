using Godot;
using System.Collections.Generic;

/// <summary>
/// Cenario da Aldeia montado na cena <c>scenes/levels/AldeiaBackground.tscn</c>.
/// Ajuste posicao/escala dos sprites no editor Godot — nao no codigo.
/// Linhas WalkBandGuide (pes) e HorizonGuide (base das cabanas) ajudam o alinhamento.
/// </summary>
[GlobalClass]
public partial class AldeiaParallaxBackground : Node2D
{
    private sealed class AmbientPolygon
    {
        public AmbientPolygon(Polygon2D polygon, Vector2 anchor, float speed, float amplitude, float phase, float baseAlpha)
        {
            Polygon = polygon;
            Anchor = anchor;
            Speed = speed;
            Amplitude = amplitude;
            Phase = phase;
            BaseAlpha = baseAlpha;
        }

        public Polygon2D Polygon { get; }
        public Vector2 Anchor { get; }
        public float Speed { get; }
        public float Amplitude { get; }
        public float Phase { get; }
        public float BaseAlpha { get; }
    }

    private sealed class Layer
    {
        public Layer(Sprite2D sprite, Vector2 anchor, float parallax)
        {
            Sprite = sprite;
            Anchor = anchor;
            Parallax = parallax;
        }

        public Sprite2D Sprite { get; }
        public Vector2 Anchor { get; }
        public float Parallax { get; }
    }

    [Export]
    public float ScrollFactor { get; set; } = 0.16f;

    [Export]
    public bool ShowAlignmentGuides { get; set; } = true;

    private readonly List<Layer> _layers = new();
    private readonly List<AmbientPolygon> _flames = new();
    private readonly List<AmbientPolygon> _smokeWisps = new();
    private readonly List<AmbientPolygon> _embers = new();
    private float _cameraOriginX;
    private float _ambientTime;
    private bool _ready;

    public bool HasLoadedLayers => _layers.Count > 0;

    public override void _Ready()
    {
        ZAsRelative = false;
        CacheParallaxLayers();
        BuildAmbientLife();
        UpdateGuideVisibility();
    }

    public void SetCameraOrigin(float cameraX)
    {
        _cameraOriginX = cameraX;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _ambientTime += dt;
        AnimateAmbientLife();

        if (!_ready)
        {
            return;
        }

        var camera = GetViewport().GetCamera2D();
        if (camera is null)
        {
            return;
        }

        var deltaX = camera.GlobalPosition.X - _cameraOriginX;
        foreach (var layer in _layers)
        {
            layer.Sprite.Position = layer.Anchor + new Vector2(deltaX * layer.Parallax, 0f);
        }
    }

    private void CacheParallaxLayers()
    {
        _layers.Clear();
        RegisterLayer("AldeiaBackdrop", ScrollFactor);
        RegisterLayer("AldeiaForeground", ScrollFactor);
        _ready = _layers.Count > 0;

        if (_ready)
        {
            GD.Print("AldeiaParallaxBackground: cena carregada — edite AldeiaBackground.tscn no Godot.");
        }
    }

    private void RegisterLayer(string nodeName, float parallax)
    {
        if (GetNodeOrNull<Sprite2D>(nodeName) is not { } sprite)
        {
            return;
        }

        _layers.Add(new Layer(sprite, sprite.Position, parallax));
    }

    private void BuildAmbientLife()
    {
        if (HasNode("AmbientLife"))
        {
            return;
        }

        var ambientRoot = new Node2D
        {
            Name = "AmbientLife",
            ZIndex = -20,
            ZAsRelative = false
        };
        AddChild(ambientRoot);

        AddFire(ambientRoot, "FireLeftWindow", new Vector2(-102f, 134f), 1.15f, 0.2f);
        AddFire(ambientRoot, "FireCenterWindow", new Vector2(69f, 132f), 1.05f, 1.4f);
        AddFire(ambientRoot, "FireRightRuin", new Vector2(315f, 142f), 0.9f, 2.1f);

        AddSmoke(ambientRoot, "SmokeLeftA", new Vector2(-150f, 72f), 0.95f, 0.1f);
        AddSmoke(ambientRoot, "SmokeLeftB", new Vector2(-72f, 58f), 0.8f, 1.3f);
        AddSmoke(ambientRoot, "SmokeRightA", new Vector2(126f, 50f), 0.9f, 2.4f);
        AddSmoke(ambientRoot, "SmokeFarRight", new Vector2(336f, 72f), 0.72f, 3.1f);

        AddEmber(ambientRoot, "EmberFloatA", new Vector2(-188f, 156f), 0.1f);
        AddEmber(ambientRoot, "EmberFloatB", new Vector2(-28f, 170f), 1.2f);
        AddEmber(ambientRoot, "EmberFloatC", new Vector2(142f, 152f), 2.1f);
        AddEmber(ambientRoot, "EmberFloatD", new Vector2(278f, 168f), 3.3f);
        AddEmber(ambientRoot, "EmberFloatE", new Vector2(372f, 146f), 4.2f);
    }

    private void AddFire(Node parent, string name, Vector2 position, float scale, float phase)
    {
        var glow = new Polygon2D
        {
            Name = name,
            Position = position,
            Scale = new Vector2(scale, scale),
            Color = new Color("#e06b2f", 0.46f),
            Polygon =
            [
                new Vector2(-14f, 10f), new Vector2(-8f, -6f), new Vector2(-2f, -18f),
                new Vector2(5f, -4f), new Vector2(12f, 8f), new Vector2(2f, 14f)
            ]
        };
        parent.AddChild(glow);
        _flames.Add(new AmbientPolygon(glow, position, 7.8f, 4.5f, phase, 0.46f));

        var core = new Polygon2D
        {
            Name = $"{name}Core",
            Position = position + new Vector2(1f, -1f),
            Scale = new Vector2(scale * 0.62f, scale * 0.62f),
            Color = new Color("#e0b75d", 0.66f),
            Polygon =
            [
                new Vector2(-7f, 7f), new Vector2(-3f, -6f), new Vector2(2f, -14f),
                new Vector2(7f, 2f), new Vector2(2f, 9f)
            ]
        };
        parent.AddChild(core);
        _flames.Add(new AmbientPolygon(core, core.Position, 9.4f, 2.5f, phase + 0.8f, 0.66f));
    }

    private void AddSmoke(Node parent, string name, Vector2 position, float scale, float phase)
    {
        var smoke = new Polygon2D
        {
            Name = name,
            Position = position,
            Scale = new Vector2(scale, scale),
            Color = new Color("#5f6970", 0.18f),
            Polygon =
            [
                new Vector2(-20f, 18f), new Vector2(-30f, -2f), new Vector2(-14f, -20f),
                new Vector2(6f, -26f), new Vector2(28f, -8f), new Vector2(22f, 14f),
                new Vector2(2f, 24f)
            ]
        };
        parent.AddChild(smoke);
        _smokeWisps.Add(new AmbientPolygon(smoke, position, 0.62f, 8f, phase, 0.18f));
    }

    private void AddEmber(Node parent, string name, Vector2 position, float phase)
    {
        var ember = new Polygon2D
        {
            Name = name,
            Position = position,
            Color = new Color("#e0b75d", 0.72f),
            Polygon =
            [
                new Vector2(-2f, 0f), new Vector2(1f, -3f), new Vector2(4f, 1f), new Vector2(0f, 4f)
            ]
        };
        parent.AddChild(ember);
        _embers.Add(new AmbientPolygon(ember, position, 14f, 7f, phase, 0.72f));
    }

    private void AnimateAmbientLife()
    {
        foreach (var flame in _flames)
        {
            var pulse = Mathf.Sin(_ambientTime * flame.Speed + flame.Phase);
            var warm = 0.82f + pulse * 0.18f;
            flame.Polygon.Position = flame.Anchor + new Vector2(pulse * 0.8f, -Mathf.Abs(pulse) * flame.Amplitude);
            flame.Polygon.Modulate = new Color(warm, 0.86f + pulse * 0.08f, 0.72f, flame.BaseAlpha);
        }

        foreach (var smoke in _smokeWisps)
        {
            var drift = Mathf.Sin(_ambientTime * smoke.Speed + smoke.Phase);
            var rise = Mathf.Sin(_ambientTime * smoke.Speed * 0.7f + smoke.Phase) * 3f;
            smoke.Polygon.Position = smoke.Anchor + new Vector2(drift * smoke.Amplitude, -rise);
            smoke.Polygon.Rotation = drift * 0.045f;
            smoke.Polygon.Modulate = new Color(1f, 1f, 1f, smoke.BaseAlpha + drift * 0.035f);
        }

        foreach (var ember in _embers)
        {
            var rise = (_ambientTime * ember.Speed + ember.Phase * 18f) % 62f;
            var sway = Mathf.Sin(_ambientTime * 2.4f + ember.Phase) * ember.Amplitude;
            ember.Polygon.Position = ember.Anchor + new Vector2(sway, -rise);
            ember.Polygon.Modulate = new Color(1f, 0.88f, 0.58f, Mathf.Max(0f, ember.BaseAlpha * (1f - rise / 70f)));
        }

        if (GetNodeOrNull<Sprite2D>("AldeiaForeground") is { } foreground)
        {
            foreground.Rotation = Mathf.Sin(_ambientTime * 0.75f) * 0.0035f;
            foreground.Offset = new Vector2(Mathf.Sin(_ambientTime * 0.9f) * 1.2f, 0f);
        }
    }

    private void UpdateGuideVisibility()
    {
        var visible = ShowAlignmentGuides && Engine.IsEditorHint();
        SetGuideVisible("WalkBandGuide", visible);
        SetGuideVisible("HorizonGuide", visible);
    }

    private void SetGuideVisible(string nodeName, bool visible)
    {
        if (GetNodeOrNull<Line2D>(nodeName) is { } guide)
        {
            guide.Visible = visible;
        }
    }
}
