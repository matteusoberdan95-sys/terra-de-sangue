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
    private float _cameraOriginX;
    private bool _ready;

    public bool HasLoadedLayers => _layers.Count > 0;

    public override void _Ready()
    {
        ZAsRelative = false;
        CacheParallaxLayers();
        UpdateGuideVisibility();
    }

    public void SetCameraOrigin(float cameraX)
    {
        _cameraOriginX = cameraX;
    }

    public override void _Process(double delta)
    {
        _ = delta;
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

    private void UpdateGuideVisibility()
    {
        var visible = ShowAlignmentGuides && OS.IsDebugBuild();
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
