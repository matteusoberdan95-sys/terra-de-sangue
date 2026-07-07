using Godot;

[GlobalClass]
public partial class VegetationBloodStain : Node2D
{
    private Polygon2D? _stain;

    public override void _Ready()
    {
        AddToGroup("gore_effect");
        _stain = new Polygon2D
        {
            Name = "LeafStain",
            Color = new Color("#4a1814", 0.78f),
            Polygon = new[]
            {
                new Vector2(-10, -2),
                new Vector2(4, -6),
                new Vector2(14, 0),
                new Vector2(6, 8),
                new Vector2(-8, 6)
            }
        };
        AddChild(_stain);
        Rotation = (float)GD.RandRange(-0.8, 0.8);
        Scale = Vector2.One * (float)GD.RandRange(0.75, 1.2);
    }
}
