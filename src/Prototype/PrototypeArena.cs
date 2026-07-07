using Godot;

namespace TerraSangrada.Prototype;

public partial class PrototypeArena : Node2D
{
    private static readonly Rect2 PlayArea = new(new Vector2(-380, 124), new Vector2(760, 92));

    private Camera2D? _camera;
    private PlayerController? _player;
    private float _shakeTimer;
    private float _shakeStrength;

    public override void _Ready()
    {
        AddToGroup("arena_feedback");
        BuildBackground();
        BuildPlayAreaGuides();
        SpawnPlayer();
        SpawnEnemy(new Vector2(260, 148));
        SpawnEnemy(new Vector2(340, 176));
        SpawnEnemy(new Vector2(430, 156));
        BuildCamera();
    }

    public override void _Process(double delta)
    {
        if (_camera is null || _player is null)
        {
            return;
        }

        var target = _player.GlobalPosition + new Vector2(40, -18);
        _camera.GlobalPosition = _camera.GlobalPosition.Lerp(target, 8f * (float)delta);

        if (_shakeTimer > 0f)
        {
            _shakeTimer = Mathf.Max(0f, _shakeTimer - (float)delta);
            _camera.Offset = new Vector2(
                (float)GD.RandRange(-_shakeStrength, _shakeStrength),
                (float)GD.RandRange(-_shakeStrength, _shakeStrength));
        }
        else
        {
            _camera.Offset = Vector2.Zero;
        }
    }

    public async void ApplyCombatImpact(float shakeStrength, float hitPauseSeconds)
    {
        _shakeTimer = 0.14f;
        _shakeStrength = Mathf.Max(_shakeStrength, shakeStrength);

        if (Engine.TimeScale < 1f)
        {
            return;
        }

        Engine.TimeScale = 0.08;
        await ToSignal(GetTree().CreateTimer(hitPauseSeconds, true, false, true), SceneTreeTimer.SignalName.Timeout);
        Engine.TimeScale = 1.0;
        _shakeStrength = 0f;
    }

    private void BuildBackground()
    {
        AddChild(new Polygon2D
        {
            Name = "NightCanopy",
            Color = new Color("#15110f"),
            Polygon = new[]
            {
                new Vector2(-500, -300),
                new Vector2(900, -300),
                new Vector2(900, 130),
                new Vector2(-500, 130)
            }
        });

        AddChild(new Polygon2D
        {
            Name = "BloodlitGround",
            Color = new Color("#2b1b16"),
            Polygon = new[]
            {
                new Vector2(-500, 108),
                new Vector2(900, 108),
                new Vector2(900, 260),
                new Vector2(-500, 260)
            }
        });

        AddLayer("DistantTrees", new Color("#1e2a20"), 96, -420, 860, 34);
        AddLayer("AshLine", new Color("#6b3828"), 132, -420, 860, 10);
        AddLayer("WalkBand", new Color("#3f2b20"), 184, -420, 860, 36);
    }

    private void BuildPlayAreaGuides()
    {
        AddLayer("BackDepthLimit", new Color("#463326"), PlayArea.Position.Y, PlayArea.Position.X, PlayArea.Size.X, 2);
        AddLayer("FrontDepthLimit", new Color("#78503a"), PlayArea.End.Y, PlayArea.Position.X, PlayArea.Size.X, 2);
    }

    private void AddLayer(string name, Color color, float y, float x, float width, float height)
    {
        AddChild(new Polygon2D
        {
            Name = name,
            Color = color,
            Polygon = new[]
            {
                new Vector2(x, y),
                new Vector2(x + width, y),
                new Vector2(x + width, y + height),
                new Vector2(x, y + height)
            }
        });
    }

    private void SpawnPlayer()
    {
        _player = new PlayerController
        {
            Name = "Arandu",
            GlobalPosition = new Vector2(120, 164),
            MovementBounds = PlayArea
        };
        AddChild(_player);
    }

    private void SpawnEnemy(Vector2 position)
    {
        var enemy = new EnemyDummy
        {
            Name = "InvaderDummy",
            GlobalPosition = position,
            MovementBounds = PlayArea
        };
        AddChild(enemy);
    }

    private void BuildCamera()
    {
        _camera = new Camera2D
        {
            Name = "PrototypeCamera",
            Zoom = new Vector2(2f, 2f),
            PositionSmoothingEnabled = true,
            PositionSmoothingSpeed = 8f,
            Enabled = true,
            GlobalPosition = _player?.GlobalPosition ?? Vector2.Zero
        };

        AddChild(_camera);
    }
}
