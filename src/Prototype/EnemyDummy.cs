using Godot;

[GlobalClass]
public partial class EnemyDummy : EnemyBase
{
    protected override int MaxHealthValue => 3;
    protected override float ApproachSpeed => 42f;
    protected override float AttackRange => 34f;
    protected override float AttackCooldownSeconds => 1.1f;
    protected override int AttackDamage => 1;
    protected override Color BodyColor => new("#5f6970");
    protected override Color ApproachColor => new("#707a80");
    protected override Color AttackColor => new("#8f1f17");
    protected override EnemyVisualArchetype VisualArchetype => EnemyVisualArchetype.Mercenary;

    protected override bool UsePixelSprite => true;

    protected override float GetAttackStartup() => 0.3f;
    protected override float GetAttackRecovery() => 0.32f;
}
