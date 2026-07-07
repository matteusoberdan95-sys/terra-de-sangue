using Godot;

[GlobalClass]
public partial class BloodSplatter : Node2D
{
    private const float LifetimeSeconds = 0.42f;

    private readonly Polygon2D[] _drops = new Polygon2D[4];
    private float _life = LifetimeSeconds;
    private Vector2 _velocity;

    public void Launch(Vector2 direction)
    {
        _velocity = direction.Normalized() * (float)GD.RandRange(48, 96);
    }

    public override void _Ready()
    {
        for (var i = 0; i < _drops.Length; i++)
        {
            _drops[i] = new Polygon2D
            {
                Color = new Color("#b51f1f", 0.85f),
                Polygon = new[]
                {
                    Vector2.Zero,
                    new Vector2(4, -2),
                    new Vector2(8, 1),
                    new Vector2(2, 4)
                },
                Position = new Vector2(i * 5 - 8, -i * 3)
            };
            AddChild(_drops[i]);
        }
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _life -= dt;
        Position += _velocity * dt;
        _velocity = _velocity.Lerp(Vector2.Zero, 8f * dt);

        var alpha = Mathf.Clamp(_life / LifetimeSeconds, 0f, 1f);
        foreach (var drop in _drops)
        {
            drop.Modulate = new Color(1f, 1f, 1f, alpha);
        }

        if (_life <= 0f)
        {
            QueueFree();
        }
    }
}
