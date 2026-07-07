using Godot;

public enum EnemyVisualArchetype
{
    Mercenary,
    Brute,
    MiniBoss,
    IronCaptain
}

public sealed class CharacterSilhouetteParts
{
    public Polygon2D Body { get; init; } = null!;
    public Polygon2D? Head { get; init; }
    public Polygon2D? Arm { get; init; }
    public Polygon2D? Weapon { get; init; }
    public Polygon2D? Shadow { get; init; }
}

public static class SilhouetteArt
{
    public static CharacterSilhouetteParts BuildEnemy(Node2D parent, EnemyVisualArchetype archetype, Color bodyTone)
    {
        return archetype switch
        {
            EnemyVisualArchetype.Brute => BuildBrute(parent, bodyTone),
            EnemyVisualArchetype.MiniBoss => BuildMiniBoss(parent, bodyTone),
            EnemyVisualArchetype.IronCaptain => BuildMiniBoss(parent, bodyTone),
            _ => BuildMercenary(parent, bodyTone)
        };
    }

    public static void EnsurePlayerVisualRig(PlayerController player)
    {
        if (player.GetNodeOrNull<Node2D>("VisualRig") is not null)
        {
            return;
        }

        var rig = new Node2D { Name = "VisualRig" };
        player.AddChild(rig);
        rig.SetAsTopLevel(false);

        foreach (var childName in new[]
                 {
                     "GroundShadow", "Tacape", "WarriorPlaceholder", "HairSilhouette", "WarCloth", "Paint",
                     "PaintLeg", "BoneTalisman", "AttackSlash"
                 })
        {
            var child = player.GetNodeOrNull<Node>(childName);
            if (child is not null && child.GetParent() == player)
            {
                player.RemoveChild(child);
                rig.AddChild(child);
            }
        }
    }

    private static CharacterSilhouetteParts BuildMercenary(Node2D parent, Color bodyTone)
    {
        var shadow = Add(parent, "Shadow", new Color("#0a0806", 0.5f), new[]
        {
            new Vector2(-14, 14), new Vector2(0, 8), new Vector2(16, 14), new Vector2(0, 18)
        }, 0);

        var backLeg = Add(parent, "BackLeg", Darken(bodyTone, 0.75f), new[]
        {
            new Vector2(-8, 8), new Vector2(-4, 8), new Vector2(-2, 16), new Vector2(-10, 16)
        }, 1);

        var cape = Add(parent, "Cape", new Color("#2b1b16"), new[]
        {
            new Vector2(-12, -8), new Vector2(-14, 10), new Vector2(-4, 12), new Vector2(-2, -6)
        }, 2);

        var torso = Add(parent, "Torso", bodyTone, new[]
        {
            new Vector2(-10, -10), new Vector2(-11, 8), new Vector2(8, 10), new Vector2(10, -8), new Vector2(2, -18), new Vector2(-6, -16)
        }, 3);

        var frontLeg = Add(parent, "FrontLeg", Darken(bodyTone, 0.85f), new[]
        {
            new Vector2(2, 8), new Vector2(8, 8), new Vector2(10, 16), new Vector2(0, 16)
        }, 4);

        var boot = Add(parent, "Boot", new Color("#1a1410"), new[]
        {
            new Vector2(-10, 14), new Vector2(12, 14), new Vector2(10, 18), new Vector2(-8, 18)
        }, 5);

        var belt = Add(parent, "Belt", new Color("#3a2a20"), new[]
        {
            new Vector2(-9, 2), new Vector2(9, 2), new Vector2(8, 6), new Vector2(-8, 6)
        }, 6);

        var head = Add(parent, "Head", new Color("#6a5048"), new[]
        {
            new Vector2(-6, -18), new Vector2(0, -24), new Vector2(7, -18), new Vector2(5, -12), new Vector2(-5, -12)
        }, 7);

        var cap = Add(parent, "Cap", new Color("#3a3438"), new[]
        {
            new Vector2(-8, -20), new Vector2(10, -20), new Vector2(8, -14), new Vector2(-6, -14)
        }, 8);

        var arm = Add(parent, "Arm", Darken(bodyTone, 0.9f), new[]
        {
            new Vector2(6, -8), new Vector2(14, -6), new Vector2(12, 2), new Vector2(4, 0)
        }, 9);

        var musket = Add(parent, "Musket", new Color("#5f6970"), new[]
        {
            new Vector2(10, -4), new Vector2(34, -10), new Vector2(36, -6), new Vector2(12, 0)
        }, 10);

        var stock = Add(parent, "MusketStock", new Color("#2b1b16"), new[]
        {
            new Vector2(8, -2), new Vector2(14, -4), new Vector2(12, 2), new Vector2(6, 2)
        }, 11);

        _ = backLeg;
        _ = cape;
        _ = frontLeg;
        _ = boot;
        _ = belt;
        _ = cap;
        _ = musket;
        _ = stock;

        return new CharacterSilhouetteParts
        {
            Body = torso,
            Head = head,
            Arm = arm,
            Weapon = musket,
            Shadow = shadow
        };
    }

    private static CharacterSilhouetteParts BuildBrute(Node2D parent, Color bodyTone)
    {
        var shadow = Add(parent, "Shadow", new Color("#0a0806", 0.55f), new[]
        {
            new Vector2(-18, 16), new Vector2(0, 8), new Vector2(20, 16), new Vector2(0, 22)
        }, 0);

        var backLeg = Add(parent, "BackLeg", Darken(bodyTone, 0.7f), new[]
        {
            new Vector2(-10, 10), new Vector2(-2, 10), new Vector2(0, 20), new Vector2(-12, 20)
        }, 1);

        var torso = Add(parent, "Torso", bodyTone, new[]
        {
            new Vector2(-14, -8), new Vector2(-16, 12), new Vector2(12, 14), new Vector2(14, -6), new Vector2(4, -20), new Vector2(-8, -18)
        }, 2);

        var frontLeg = Add(parent, "FrontLeg", Darken(bodyTone, 0.8f), new[]
        {
            new Vector2(4, 10), new Vector2(12, 10), new Vector2(14, 20), new Vector2(2, 20)
        }, 3);

        var chain = Add(parent, "Chain", new Color("#707880"), new[]
        {
            new Vector2(10, -6), new Vector2(18, -2), new Vector2(16, 6), new Vector2(8, 2),
            new Vector2(12, -2), new Vector2(14, 4)
        }, 4);

        var head = Add(parent, "Head", Darken(bodyTone, 1.1f), new[]
        {
            new Vector2(-8, -20), new Vector2(0, -28), new Vector2(10, -20), new Vector2(6, -14), new Vector2(-6, -14)
        }, 5);

        var scar = Add(parent, "Scar", new Color("#560f0b", 0.8f), new[]
        {
            new Vector2(-2, -22), new Vector2(4, -18), new Vector2(2, -16), new Vector2(-4, -20)
        }, 6);

        var arm = Add(parent, "Arm", Darken(bodyTone, 0.85f), new[]
        {
            new Vector2(8, -6), new Vector2(18, -4), new Vector2(16, 6), new Vector2(6, 4)
        }, 7);

        var club = Add(parent, "Club", new Color("#2b1b16"), new[]
        {
            new Vector2(14, -2), new Vector2(22, -18), new Vector2(28, -16), new Vector2(18, 4)
        }, 8);

        _ = backLeg;
        _ = frontLeg;
        _ = chain;
        _ = scar;
        _ = club;

        return new CharacterSilhouetteParts
        {
            Body = torso,
            Head = head,
            Arm = arm,
            Weapon = club,
            Shadow = shadow
        };
    }

    private static CharacterSilhouetteParts BuildMiniBoss(Node2D parent, Color bodyTone)
    {
        var shadow = Add(parent, "Shadow", new Color("#0a0806", 0.55f), new[]
        {
            new Vector2(-18, 16), new Vector2(0, 8), new Vector2(22, 16), new Vector2(0, 22)
        }, 0);

        var coatTail = Add(parent, "CoatTail", new Color("#2a2228"), new[]
        {
            new Vector2(-12, 4), new Vector2(-16, 16), new Vector2(-4, 14), new Vector2(-2, 6)
        }, 1);

        var torso = Add(parent, "Torso", bodyTone, new[]
        {
            new Vector2(-11, -12), new Vector2(-12, 10), new Vector2(10, 12), new Vector2(12, -10), new Vector2(4, -22), new Vector2(-6, -20)
        }, 2);

        var sash = Add(parent, "Sash", new Color("#8f1f17"), new[]
        {
            new Vector2(-10, -2), new Vector2(10, -4), new Vector2(8, 2), new Vector2(-8, 4)
        }, 3);

        var pauldron = Add(parent, "Pauldron", new Color("#5f6970"), new[]
        {
            new Vector2(-14, -12), new Vector2(-6, -20), new Vector2(4, -16), new Vector2(2, -6), new Vector2(-12, -4)
        }, 4);

        var head = Add(parent, "Head", new Color("#5a4a48"), new[]
        {
            new Vector2(-6, -20), new Vector2(0, -28), new Vector2(8, -20), new Vector2(6, -14), new Vector2(-4, -14)
        }, 5);

        var hat = Add(parent, "Hat", new Color("#1a1418"), new[]
        {
            new Vector2(-10, -22), new Vector2(12, -22), new Vector2(8, -16), new Vector2(-6, -16)
        }, 6);

        var feather = Add(parent, "Feather", new Color("#8f1f17"), new[]
        {
            new Vector2(6, -24), new Vector2(10, -34), new Vector2(14, -24)
        }, 7);

        var arm = Add(parent, "Arm", Darken(bodyTone, 0.9f), new[]
        {
            new Vector2(8, -8), new Vector2(16, -6), new Vector2(14, 2), new Vector2(6, 0)
        }, 8);

        var saber = Add(parent, "Saber", new Color("#8a9098"), new[]
        {
            new Vector2(12, -4), new Vector2(38, -14), new Vector2(40, -10), new Vector2(14, 0)
        }, 9);

        var medal = Add(parent, "Medal", new Color("#e0b75d"), new[]
        {
            new Vector2(-4, -4), new Vector2(0, -8), new Vector2(4, -4), new Vector2(0, 0)
        }, 10);

        _ = coatTail;
        _ = sash;
        _ = pauldron;
        _ = hat;
        _ = feather;
        _ = saber;
        _ = medal;

        return new CharacterSilhouetteParts
        {
            Body = torso,
            Head = head,
            Arm = arm,
            Weapon = saber,
            Shadow = shadow
        };
    }

    private static Polygon2D Add(Node2D parent, string name, Color color, Vector2[] polygon, int order)
    {
        var node = new Polygon2D
        {
            Name = name,
            Color = color,
            Polygon = polygon,
            ZIndex = order
        };
        parent.AddChild(node);
        return node;
    }

    private static Color Darken(Color color, float factor)
    {
        return new Color(color.R * factor, color.G * factor, color.B * factor, color.A);
    }
}
