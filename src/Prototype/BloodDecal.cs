using Godot;

[GlobalClass]
public partial class BloodDecal : Node2D
{
    private const float LifetimeSeconds = 9f;

    private Polygon2D? _stain;
    private float _life = LifetimeSeconds;

    public override void _Ready()
    {
        AddToGroup("gore_effect");
        _stain = new Polygon2D
        {
            Name = "Stain",
            Color = new Color("#8f1f17", 0.62f),
            Polygon = new[]
            {
                new Vector2(-16, 2),
                new Vector2(-4, -3),
                new Vector2(14, 1),
                new Vector2(8, 8),
                new Vector2(-10, 7)
            }
        };
        AddChild(_stain);
        Rotation = (float)GD.RandRange(-0.4, 0.4);
        Scale = Vector2.One * (float)GD.RandRange(0.8, 1.25);
    }

    public override void _Process(double delta)
    {
        _life -= (float)delta;
        if (_stain is not null)
        {
            var alpha = Mathf.Clamp(_life / LifetimeSeconds, 0f, 1f) * 0.62f;
            _stain.Color = new Color("#8f1f17", alpha);
        }

        if (_life <= 0f)
        {
            QueueFree();
        }
    }
}
