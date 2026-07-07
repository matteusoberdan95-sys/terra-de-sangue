public sealed class WeaponProfile
{
    public int Tier { get; init; }

    public string DisplayName => Tier switch
    {
        1 => "Tacape de Osso",
        2 => "Tacape de Pedra-Rape",
        3 => "Tacape de Raiz",
        4 => "Tacape Ancestral",
        _ => "Tacape Ritual"
    };

    public static WeaponProfile Default => new() { Tier = 0 };
}
