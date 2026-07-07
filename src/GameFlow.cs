using Godot;

public static class GameFlow
{
    public static void RequestBossVictory(Node context)
    {
        RequestLevel(context, "res://scenes/levels/MataFechada.tscn");
    }

    public static void RequestAldeiaComplete(Node context)
    {
        RequestLevel(context, "res://scenes/levels/IronCaptainArena.tscn");
    }

    public static void RequestMataComplete(Node context)
    {
        if (TryGetGameRoot(context, out var root))
        {
            root.OnMataFechadaComplete();
        }
    }

    private static void RequestLevel(Node context, string scenePath)
    {
        if (TryGetGameRoot(context, out var root))
        {
            GD.Print($"GameFlow: transicao via GameRoot -> {scenePath}");
            root.RequestLevel(scenePath);
            return;
        }

        GD.PrintErr($"Terra Sangrada: GameRoot ausente. Carregando {scenePath} direto.");
        context.GetTree().ChangeSceneToFile(scenePath);
    }

    private static bool TryGetGameRoot(Node context, out GameRoot root)
    {
        if (GameRoot.Instance is not null)
        {
            root = GameRoot.Instance;
            return true;
        }

        foreach (var node in context.GetTree().GetNodesInGroup("game_flow"))
        {
            if (node is GameRoot gameRoot)
            {
                root = gameRoot;
                return true;
            }
        }

        var main = context.GetTree().Root.GetNodeOrNull<GameRoot>("Main");
        if (main is not null)
        {
            root = main;
            return true;
        }

        root = null!;
        return false;
    }
}
