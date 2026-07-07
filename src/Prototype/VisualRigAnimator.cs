using Godot;

[GlobalClass]
public partial class VisualRigAnimator : Node
{
    private Node2D? _rig;
    private float _phase;
    private bool _moving;
    private bool _attacking;
    private float _attackPulse;

    public void Bind(Node2D rig)
    {
        _rig = rig;
    }

    public void SetMoving(bool moving)
    {
        _moving = moving;
    }

    public void SetFacing(float directionX)
    {
        if (_rig is null)
        {
            return;
        }

        _rig.Scale = new Vector2(directionX < 0f ? -1f : 1f, 1f);
    }

    public void PulseAttack()
    {
        _attacking = true;
        _attackPulse = 0.16f;
    }

    public override void _Process(double delta)
    {
        if (_rig is null)
        {
            return;
        }

        var dt = (float)delta;
        _phase += dt;

        if (_attackPulse > 0f)
        {
            _attackPulse = Mathf.Max(0f, _attackPulse - dt);
            if (_attackPulse <= 0f)
            {
                _attacking = false;
            }
        }

        var bob = _moving ? Mathf.Sin(_phase * 11f) * 2.2f : Mathf.Sin(_phase * 2f) * 0.4f;
        var lunge = _attacking ? Mathf.Sin((0.16f - _attackPulse) / 0.16f * Mathf.Pi) * 3f : 0f;
        _rig.Position = new Vector2(lunge, bob);
    }
}
