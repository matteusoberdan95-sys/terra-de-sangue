using Godot;

namespace TerraSangrada.Prototype;

public partial class PlayerController : CharacterBody2D
{
    private const float Speed = 120f;
    private const float AttackRange = 46f;
    private const float AttackCooldownSeconds = 0.28f;

    private Polygon2D? _body;
    private Polygon2D? _attackSlash;
    private Vector2 _facing = Vector2.Right;
    private float _attackCooldown;
    private float _slashTimer;

    public override void _Ready()
    {
        AddToGroup("player");
        EnsureInputMap();
        BuildVisuals();
        BuildCollision();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _attackCooldown = Mathf.Max(0f, _attackCooldown - dt);
        _slashTimer = Mathf.Max(0f, _slashTimer - dt);

        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (input.LengthSquared() > 0.01f)
        {
            _facing = input.X < -0.1f ? Vector2.Left : input.X > 0.1f ? Vector2.Right : _facing;
        }

        Velocity = input.Normalized() * Speed;
        MoveAndSlide();
        ZIndex = Mathf.RoundToInt(GlobalPosition.Y);

        if (Input.IsActionJustPressed("attack_light") && _attackCooldown <= 0f)
        {
            Attack();
        }

        if (_attackSlash is not null)
        {
            _attackSlash.Visible = _slashTimer > 0f;
            _attackSlash.Scale = new Vector2(_facing.X, 1f);
        }
    }

    private void Attack()
    {
        _attackCooldown = AttackCooldownSeconds;
        _slashTimer = 0.12f;

        foreach (var node in GetTree().GetNodesInGroup("enemy"))
        {
            if (node is not EnemyDummy enemy)
            {
                continue;
            }

            var toEnemy = enemy.GlobalPosition - GlobalPosition;
            var inFront = Mathf.Sign(toEnemy.X == 0 ? _facing.X : toEnemy.X) == Mathf.Sign(_facing.X);
            var closeEnough = Mathf.Abs(toEnemy.X) <= AttackRange && Mathf.Abs(toEnemy.Y) <= 24f;

            if (inFront && closeEnough)
            {
                enemy.TakeHit(_facing * 42f);
            }
        }
    }

    private void BuildVisuals()
    {
        _body = new Polygon2D
        {
            Name = "WarriorPlaceholder",
            Color = new Color("#c46f35"),
            Polygon = new[]
            {
                new Vector2(-10, 12),
                new Vector2(-7, -18),
                new Vector2(0, -30),
                new Vector2(10, -18),
                new Vector2(12, 12)
            }
        };
        AddChild(_body);

        AddChild(new Polygon2D
        {
            Name = "Paint",
            Color = new Color("#e0b75d"),
            Polygon = new[]
            {
                new Vector2(-7, -10),
                new Vector2(8, -14),
                new Vector2(7, -9),
                new Vector2(-8, -5)
            }
        });

        _attackSlash = new Polygon2D
        {
            Name = "AttackSlash",
            Color = new Color("#b51f1f"),
            Visible = false,
            Polygon = new[]
            {
                new Vector2(10, -16),
                new Vector2(58, -5),
                new Vector2(54, 8),
                new Vector2(12, 3)
            }
        };
        AddChild(_attackSlash);
    }

    private void BuildCollision()
    {
        var shape = new CollisionShape2D
        {
            Name = "Collision",
            Shape = new RectangleShape2D
            {
                Size = new Vector2(18, 28)
            },
            Position = new Vector2(0, -4)
        };

        AddChild(shape);
    }

    private static void EnsureInputMap()
    {
        AddKeyAction("move_left", Key.A);
        AddKeyAction("move_right", Key.D);
        AddKeyAction("move_up", Key.W);
        AddKeyAction("move_down", Key.S);
        AddKeyAction("attack_light", Key.J);
    }

    private static void AddKeyAction(string action, Key key)
    {
        if (!InputMap.HasAction(action))
        {
            InputMap.AddAction(action);
        }

        foreach (var existingEvent in InputMap.ActionGetEvents(action))
        {
            if (existingEvent is InputEventKey existingKey && existingKey.Keycode == key)
            {
                return;
            }
        }

        InputMap.ActionAddEvent(action, new InputEventKey { Keycode = key });
    }
}
