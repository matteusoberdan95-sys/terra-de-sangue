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
    private enum AmbientMotion
    {
        Fire,
        Smoke,
        Embers
    }

    private sealed class AnimatedSheet
    {
        public AnimatedSheet(Sprite2D sprite, Vector2 anchor, Vector2 frameSize, int frameCount, float fps, float phase, float amplitude, float baseAlpha, AmbientMotion motion)
        {
            Sprite = sprite;
            Anchor = anchor;
            FrameSize = frameSize;
            FrameCount = frameCount;
            Fps = fps;
            Phase = phase;
            Amplitude = amplitude;
            BaseAlpha = baseAlpha;
            Motion = motion;
        }

        public Sprite2D Sprite { get; }
        public Vector2 Anchor { get; }
        public Vector2 FrameSize { get; }
        public int FrameCount { get; }
        public float Fps { get; }
        public float Phase { get; }
        public float Amplitude { get; }
        public float BaseAlpha { get; }
        public AmbientMotion Motion { get; }
    }

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
    private readonly List<AnimatedSheet> _animatedSheets = new();
    private Node2D? _ambientRoot;
    private Vector2 _ambientRootAnchor;
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

        if (_ambientRoot is not null)
        {
            _ambientRoot.Position = _ambientRootAnchor + new Vector2(deltaX * ScrollFactor, 0f);
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

        _ambientRoot = new Node2D
        {
            Name = "AmbientLife",
            ZIndex = -60,
            ZAsRelative = false
        };
        _ambientRootAnchor = _ambientRoot.Position;
        AddChild(_ambientRoot);

        var fireSheet = LoadVfxTexture("res://assets/art/vfx/aldeia_fire_sheet.png");
        var emberSheet = LoadVfxTexture("res://assets/art/vfx/aldeia_embers_sheet.png");
        var smokeSheet = LoadVfxTexture("res://assets/art/vfx/aldeia_smoke_sheet.png");

        if (fireSheet is not null)
        {
            AddSheet(_ambientRoot, "FireLeftWindow", fireSheet, new Vector2(300f, 300f), 8, new Vector2(-214f, 142f), 0.065f, 8f, 0.2f, 0.8f, 0.72f, AmbientMotion.Fire);
            AddSheet(_ambientRoot, "FireCenterWindow", fireSheet, new Vector2(300f, 300f), 8, new Vector2(-76f, 140f), 0.058f, 7.6f, 1.4f, 0.7f, 0.66f, AmbientMotion.Fire);
            AddSheet(_ambientRoot, "FireRightWindow", fireSheet, new Vector2(300f, 300f), 8, new Vector2(90f, 140f), 0.052f, 8.4f, 2.1f, 0.65f, 0.62f, AmbientMotion.Fire);
        }
        else
        {
            AddFire(_ambientRoot, "FireLeftWindow", new Vector2(-214f, 146f), 0.64f, 0.2f);
            AddFire(_ambientRoot, "FireCenterWindow", new Vector2(-76f, 144f), 0.56f, 1.4f);
            AddFire(_ambientRoot, "FireRightWindow", new Vector2(90f, 144f), 0.5f, 2.1f);
        }

        if (smokeSheet is not null)
        {
            AddSheet(_ambientRoot, "SmokeLeftRoof", smokeSheet, new Vector2(222f, 887f), 8, new Vector2(-180f, 64f), 0.045f, 2.8f, 0.1f, 3.2f, 0.16f, AmbientMotion.Smoke);
            AddSheet(_ambientRoot, "SmokeCenterRoof", smokeSheet, new Vector2(222f, 887f), 8, new Vector2(-24f, 58f), 0.04f, 2.5f, 1.3f, 2.8f, 0.13f, AmbientMotion.Smoke);
            AddSheet(_ambientRoot, "SmokeRightRoof", smokeSheet, new Vector2(222f, 887f), 8, new Vector2(132f, 66f), 0.038f, 2.7f, 2.4f, 2.8f, 0.12f, AmbientMotion.Smoke);
        }
        else
        {
            AddSmoke(_ambientRoot, "SmokeLeftRoof", new Vector2(-180f, 64f), 0.46f, 0.1f);
            AddSmoke(_ambientRoot, "SmokeCenterRoof", new Vector2(-24f, 58f), 0.4f, 1.3f);
            AddSmoke(_ambientRoot, "SmokeRightRoof", new Vector2(132f, 66f), 0.38f, 2.4f);
        }

        if (emberSheet is not null)
        {
            AddSheet(_ambientRoot, "EmbersLeft", emberSheet, new Vector2(256f, 256f), 8, new Vector2(-210f, 150f), 0.075f, 6.8f, 0.4f, 3.4f, 0.36f, AmbientMotion.Embers);
            AddSheet(_ambientRoot, "EmbersCenter", emberSheet, new Vector2(256f, 256f), 8, new Vector2(-76f, 148f), 0.07f, 6.4f, 1.8f, 3.2f, 0.32f, AmbientMotion.Embers);
            AddSheet(_ambientRoot, "EmbersRight", emberSheet, new Vector2(256f, 256f), 8, new Vector2(92f, 148f), 0.065f, 7f, 3.0f, 3f, 0.3f, AmbientMotion.Embers);
        }
        else
        {
            AddEmber(_ambientRoot, "EmberFloatA", new Vector2(-210f, 150f), 0.4f);
            AddEmber(_ambientRoot, "EmberFloatB", new Vector2(-76f, 148f), 1.8f);
            AddEmber(_ambientRoot, "EmberFloatC", new Vector2(92f, 148f), 3.0f);
        }
    }

    private static Texture2D? LoadVfxTexture(string path)
    {
        return ResourceLoader.Exists(path) ? GD.Load<Texture2D>(path) : null;
    }

    private void AddSheet(
        Node parent,
        string name,
        Texture2D texture,
        Vector2 frameSize,
        int frameCount,
        Vector2 position,
        float scale,
        float fps,
        float phase,
        float amplitude,
        float alpha,
        AmbientMotion motion)
    {
        var sprite = new Sprite2D
        {
            Name = name,
            Texture = texture,
            Position = position,
            Scale = new Vector2(scale, scale),
            RegionEnabled = true,
            RegionRect = new Rect2(Vector2.Zero, frameSize),
            Modulate = new Color(1f, 1f, 1f, alpha)
        };
        parent.AddChild(sprite);
        _animatedSheets.Add(new AnimatedSheet(sprite, position, frameSize, frameCount, fps, phase, amplitude, alpha, motion));
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
        foreach (var sheet in _animatedSheets)
        {
            AnimateSheet(sheet);
        }

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

    private void AnimateSheet(AnimatedSheet sheet)
    {
        var rawFrame = Mathf.FloorToInt(_ambientTime * sheet.Fps + sheet.Phase);
        var frame = rawFrame % sheet.FrameCount;
        if (frame < 0)
        {
            frame += sheet.FrameCount;
        }

        sheet.Sprite.RegionRect = new Rect2(new Vector2(frame * sheet.FrameSize.X, 0f), sheet.FrameSize);

        var pulse = Mathf.Sin(_ambientTime * 2.4f + sheet.Phase);
        switch (sheet.Motion)
        {
            case AmbientMotion.Fire:
                sheet.Sprite.Position = sheet.Anchor + new Vector2(pulse * sheet.Amplitude * 0.22f, -Mathf.Abs(pulse) * sheet.Amplitude);
                sheet.Sprite.Modulate = new Color(1f, 0.9f + pulse * 0.05f, 0.72f, sheet.BaseAlpha);
                break;
            case AmbientMotion.Smoke:
                sheet.Sprite.Position = sheet.Anchor + new Vector2(pulse * sheet.Amplitude, Mathf.Sin(_ambientTime * 1.1f + sheet.Phase) * -2f);
                sheet.Sprite.Rotation = pulse * 0.025f;
                sheet.Sprite.Modulate = new Color(0.62f, 0.62f, 0.58f, sheet.BaseAlpha + pulse * 0.035f);
                break;
            case AmbientMotion.Embers:
                var rise = (_ambientTime * 9f + sheet.Phase * 15f) % 34f;
                sheet.Sprite.Position = sheet.Anchor + new Vector2(pulse * sheet.Amplitude, -rise);
                sheet.Sprite.Modulate = new Color(1f, 0.88f, 0.58f, Mathf.Max(0.08f, sheet.BaseAlpha * (1f - rise / 48f)));
                break;
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
