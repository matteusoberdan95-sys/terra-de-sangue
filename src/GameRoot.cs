using Godot;

[GlobalClass]
public partial class GameRoot : Node2D
{
    private Node? _currentLevel;

    public override void _Ready()
    {
        AddToGroup("game_flow");
        Engine.MaxFps = 60;
        MemoryRegistry.Reset();
        AddChild(new CombatHud { Name = "CombatHud" });
        LoadLevel("res://scenes/levels/AldeiaEmCinzas.tscn");
    }

    public void OnAldeiaPhaseComplete()
    {
        CallDeferred(MethodName.LoadLevel, "res://scenes/levels/IronCaptainArena.tscn");
    }

    public void OnBossVictory()
    {
        CallDeferred(MethodName.LoadLevel, "res://scenes/levels/MataFechada.tscn");
    }

    public void OnMataFechadaComplete()
    {
        GD.Print("Terra Sangrada: progressao das fases 1-2 concluida no prototipo.");
    }

    private void LoadLevel(string scenePath)
    {
        _currentLevel?.QueueFree();
        var packed = ResourceLoader.Load<PackedScene>(scenePath);
        _currentLevel = packed.Instantiate();
        AddChild(_currentLevel);
    }
}
