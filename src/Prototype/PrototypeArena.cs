using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class PrototypeArena : Node2D
{
    private static readonly Rect2 PlayArea = new(new Vector2(-380, 124), new Vector2(760, 92));

    private Camera2D? _camera;
    private PlayerController? _player;
    private readonly List<AmbientParticle> _ambientParticles = new();
    private float _shakeTimer;
    private float _shakeStrength;
    private float _ambientTime;

    private sealed class AmbientParticle
    {
        public AmbientParticle(Polygon2D node, Vector2 origin, float speed, float amplitude, float phase)
        {
            Node = node;
            Origin = origin;
            Speed = speed;
            Amplitude = amplitude;
            Phase = phase;
        }

        public Polygon2D Node { get; }
        public Vector2 Origin { get; }
        public float Speed { get; }
        public float Amplitude { get; }
        public float Phase { get; }
    }

    public override void _Ready()
    {
        AddToGroup("arena_feedback");
        BuildBackground();
        BuildPlayAreaGuides();
        SpawnPlayer();
        BuildPhaseDirector();
        BuildImpactFeedback();
        CacheAmbientParticles();
        BuildCamera();
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        UpdateAmbientParticles(dt);

        if (_camera is null || _player is null)
        {
            return;
        }

        var target = _player.GlobalPosition + new Vector2(40, -18);
        _camera.GlobalPosition = _camera.GlobalPosition.Lerp(target, 8f * dt);

        if (_shakeTimer > 0f)
        {
            _shakeTimer = Mathf.Max(0f, _shakeTimer - dt);
            _camera.Offset = new Vector2(
                (float)GD.RandRange(-_shakeStrength, _shakeStrength),
                (float)GD.RandRange(-_shakeStrength, _shakeStrength));
        }
        else
        {
            _camera.Offset = Vector2.Zero;
        }
    }

    private void CacheAmbientParticles()
    {
        _ambientParticles.Clear();

        foreach (var child in GetChildren())
        {
            if (child is not Polygon2D polygon)
            {
                continue;
            }

            var name = polygon.Name.ToString();
            if (!name.StartsWith("Ember") && !name.StartsWith("AshFlake"))
            {
                continue;
            }

            var seed = Mathf.Abs(name.Hash());
            var speed = name.StartsWith("Ember") ? 18f + seed % 9 : 8f + seed % 5;
            var amplitude = name.StartsWith("Ember") ? 4f + seed % 4 : 8f + seed % 6;
            var phase = seed % 628 / 100f;
            _ambientParticles.Add(new AmbientParticle(polygon, polygon.Position, speed, amplitude, phase));
        }
    }

    private void UpdateAmbientParticles(float dt)
    {
        _ambientTime += dt;

        foreach (var particle in _ambientParticles)
        {
            var rise = (_ambientTime * particle.Speed + particle.Phase * 10f) % 72f;
            var sway = Mathf.Sin(_ambientTime * 2.2f + particle.Phase) * particle.Amplitude;
            particle.Node.Position = particle.Origin + new Vector2(sway, -rise);
            particle.Node.Modulate = new Color(1f, 1f, 1f, 1f - rise / 86f);
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
        if (HasNode("NightCanopy") || HasNode("BloodlitGround"))
        {
            return;
        }

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
        if (HasNode(name))
        {
            return;
        }

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
        var existingPlayer = GetNodeOrNull<PlayerController>("Arandu");
        if (existingPlayer is not null)
        {
            _player = existingPlayer;
            _player.MovementBounds = PlayArea;
            return;
        }

        _player = new PlayerController
        {
            Name = "Arandu",
            GlobalPosition = new Vector2(120, 164),
            MovementBounds = PlayArea
        };
        AddChild(_player);
    }

    public void SpawnEnemy(string kind, Vector2 position)
    {
        EnemyBase enemy = kind switch
        {
            "brute" => new EnemyBrute(),
            "miniboss" => new EnemyMiniBoss(),
            "capitao" => new BossIronCaptain(),
            _ => new EnemyDummy()
        };

        enemy.Name = kind switch
        {
            "brute" => "Brute",
            "miniboss" => "MiniBoss",
            "capitao" => "IronCaptain",
            _ => "Mercenary"
        };
        enemy.GlobalPosition = position;
        enemy.MovementBounds = PlayArea;
        AddChild(enemy);
    }

    public void SpawnMemoryPickup(Vector2 position)
    {
        AddChild(new MemoryPickup
        {
            Name = "BrokenMaskMemory",
            GlobalPosition = position
        });
    }

    private void BuildImpactFeedback()
    {
        if (GetNodeOrNull<ImpactFeedback>("ImpactFeedback") is not null)
        {
            return;
        }

        AddChild(new ImpactFeedback { Name = "ImpactFeedback" });
    }

    private void BuildPhaseDirector()
    {
        if (GetNodeOrNull<PhaseDirector>("PhaseDirector") is not null)
        {
            return;
        }

        AddChild(new PhaseDirector { Name = "PhaseDirector" });
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
