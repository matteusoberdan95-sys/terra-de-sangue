using Godot;

[GlobalClass]
public partial class EnemyBrute : EnemyBase
{
    protected override int MaxHealth => 6;
    protected override float ApproachSpeed => 28f;
    protected override float AttackRange => 38f;
    protected override float AttackCooldownSeconds => 1.5f;
    protected override int AttackDamage => 2;
    protected override Color BodyColor => new("#4a3a32");
    protected override Color ApproachColor => new("#5c4a40");
    protected override Color AttackColor => new("#6b1a14");

    public override void _Ready()
    {
        base._Ready();
        Scale = new Vector2(1.18f, 1.18f);
    }
}
