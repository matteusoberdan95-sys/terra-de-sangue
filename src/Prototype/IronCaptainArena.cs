using Godot;

[GlobalClass]
public partial class IronCaptainArena : PrototypeArena
{
    protected override bool ShouldSpawnPhaseDirector => false;

    public override void _Ready()
    {
        base._Ready();
        BuildIronDecor();
        BuildBossDirector();
        SpawnArrowPickup(new Vector2(160, 158), 2);
    }

    protected override void ConfigurePlayerLoadout(PlayerController player)
    {
        if (WeaponProgression.TacapeTier >= 1)
        {
            player.AddArrows(2);
        }
    }

    private void BuildIronDecor()
    {
        AddChild(new Polygon2D
        {
            Name = "IronPillarLeft",
            Color = new Color("#2e2a30"),
            Polygon = new[]
            {
                new Vector2(-360, 96),
                new Vector2(-330, 96),
                new Vector2(-328, 220),
                new Vector2(-362, 220)
            }
        });
        AddChild(new Polygon2D
        {
            Name = "IronPillarRight",
            Color = new Color("#2e2a30"),
            Polygon = new[]
            {
                new Vector2(350, 96),
                new Vector2(380, 96),
                new Vector2(382, 220),
                new Vector2(348, 220)
            }
        });
        AddChild(new Polygon2D
        {
            Name = "ChainArc",
            Color = new Color("#5f6970"),
            Polygon = new[]
            {
                new Vector2(-120, 88),
                new Vector2(0, 72),
                new Vector2(120, 88),
                new Vector2(0, 104)
            }
        });
    }

    private void BuildBossDirector()
    {
        if (GetNodeOrNull<BossDirector>("BossDirector") is not null)
        {
            return;
        }

        AddChild(new BossDirector { Name = "BossDirector" });
    }
}
