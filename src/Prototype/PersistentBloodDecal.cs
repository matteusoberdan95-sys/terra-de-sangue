using Godot;

[GlobalClass]
public partial class PersistentBloodDecal : Node2D
{
    private Polygon2D? _stain;

    public override void _Ready()
    {
        _stain = new Polygon2D
        {
            Name = "PersistentStain",
            Color = new Color("#6b1510", 0.72f),
            Polygon = new[]
            {
                new Vector2(-20, 2),
                new Vector2(-6, -4),
                new Vector2(18, 0),
                new Vector2(10, 10),
                new Vector2(-14, 8)
            }
        };
        AddChild(_stain);
        Rotation = (float)GD.RandRange(-0.5, 0.5);
        Scale = Vector2.One * (float)GD.RandRange(0.9, 1.35);
    }
}
