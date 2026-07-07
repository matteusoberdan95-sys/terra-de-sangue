public sealed class WeaponProfile
{
    public int Tier { get; init; }

    public int LightDamageBonus => Tier >= 1 ? 1 : 0;

    public float ComboWindowBonus => Tier switch
    {
        >= 1 => 0.02f,
        _ => 0f
    };

    public bool HeavyAppliesBleed => Tier >= 1;

    public string DisplayName => Tier switch
    {
        1 => "Tacape de Osso",
        2 => "Tacape de Pedra-Rape",
        3 => "Tacape de Raiz",
        4 => "Tacape Ancestral",
        _ => "Tacape Ritual"
    };

    public static WeaponProfile Default => ForTier(0);

    public static WeaponProfile ForTier(int tier)
    {
        return new WeaponProfile { Tier = tier < 0 ? 0 : tier };
    }
}
