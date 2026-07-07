using Godot;

[GlobalClass]
public partial class SeveredLimb : Node2D
{
    private const float LifetimeSeconds = 2.4f;

    private Polygon2D? _mesh;
    private float _life = LifetimeSeconds;
    private Vector2 _velocity;
    private float _spin;

    public void Configure(LimbKind kind, Vector2 impulse)
    {
        _velocity = impulse;
        _spin = impulse.X < 0f ? -9f : 9f;
        BuildMesh(kind);
        Rotation = (float)GD.RandRange(-0.4, 0.4);
    }

    public override void _Ready()
    {
        AddToGroup("gore_effect");
        ZIndex += 2;
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _life -= dt;
        Position += _velocity * dt;
        _velocity = _velocity.Lerp(new Vector2(0f, 42f), 5f * dt);
        Rotation += _spin * dt;
        _spin = Mathf.Lerp(_spin, 0f, 3f * dt);

        var alpha = Mathf.Clamp(_life / LifetimeSeconds, 0f, 1f);
        Modulate = new Color(1f, 1f, 1f, alpha);

        if (_life <= 0f)
        {
            QueueFree();
        }
    }

    private void BuildMesh(LimbKind kind)
    {
        _mesh = new Polygon2D { Name = "LimbMesh" };
        AddChild(_mesh);

        switch (kind)
        {
            case LimbKind.Head:
                _mesh.Color = new Color("#6a3a34");
                _mesh.Polygon = new[]
                {
                    new Vector2(-7, -4),
                    new Vector2(0, -10),
                    new Vector2(8, -4),
                    new Vector2(6, 4),
                    new Vector2(-6, 4)
                };
                Scale = Vector2.One * 0.9f;
                break;
            case LimbKind.TorsoChunk:
                _mesh.Color = new Color("#560f0b");
                _mesh.Polygon = new[]
                {
                    new Vector2(-10, -6),
                    new Vector2(10, -4),
                    new Vector2(8, 8),
                    new Vector2(-8, 10)
                };
                Scale = Vector2.One * 1.1f;
                break;
            default:
                _mesh.Color = new Color("#5a2a24");
                _mesh.Polygon = new[]
                {
                    new Vector2(-4, -8),
                    new Vector2(6, -6),
                    new Vector2(5, 10),
                    new Vector2(-5, 8)
                };
                break;
        }

        var wound = new Polygon2D
        {
            Color = new Color("#8f1f17", 0.9f),
            Polygon = new[]
            {
                new Vector2(-2, -2),
                new Vector2(4, -1),
                new Vector2(2, 3),
                new Vector2(-3, 2)
            }
        };
        AddChild(wound);
    }
}
