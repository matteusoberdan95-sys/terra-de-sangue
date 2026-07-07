using Godot;

namespace TerraSangrada;

public partial class GameRoot : Node2D
{
    public override void _Ready()
    {
        Engine.MaxFps = 60;

        var arenaScene = ResourceLoader.Load<PackedScene>("res://scenes/levels/PrototypeArena.tscn");
        AddChild(arenaScene.Instantiate());
    }
}
