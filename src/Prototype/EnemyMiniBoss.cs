using Godot;

[GlobalClass]
public partial class EnemyMiniBoss : EnemyBase
{
    protected override int MaxHealthValue => 10;
    protected override float ApproachSpeed => 34f;
    protected override float AttackRange => 40f;
    protected override float AttackCooldownSeconds => 0.95f;
    protected override int AttackDamage => 2;
    protected override Color BodyColor => new("#3a3438");
    protected override Color ApproachColor => new("#4a4248");
    protected override Color AttackColor => new("#8f1f17");
    protected override EnemyVisualArchetype VisualArchetype => EnemyVisualArchetype.MiniBoss;

    public override void _Ready()
    {
        base._Ready();
        Scale = new Vector2(1.28f, 1.28f);
    }
}
