using Godot;

[GlobalClass]
public partial class GameRoot : Node2D
{
    public override void _Ready()
    {
        Engine.MaxFps = 60;

        var levelScene = ResourceLoader.Load<PackedScene>("res://scenes/levels/AldeiaEmCinzas.tscn");
        AddChild(levelScene.Instantiate());
    }
}
