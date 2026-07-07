using Godot;

public static class PlaceholderSfx
{
    private const int MixRate = 22050;

    public static AudioStreamWav CreateExecutionCrunch()
    {
        return CreateBurst(0.2f, 48f, 0.75f, 0.5f);
    }

    public static AudioStreamWav CreateHitThud()
    {
        return CreateBurst(0.08f, 92f, 0.55f, 0.25f);
    }

    public static AudioStreamWav CreateDeathCrunch()
    {
        return CreateBurst(0.16f, 58f, 0.7f, 0.4f);
    }

    public static AudioStreamWav CreatePlayerHurt()
    {
        return CreateBurst(0.1f, 74f, 0.45f, 0.3f);
    }

    private static AudioStreamWav CreateBurst(float duration, float frequency, float volume, float noiseAmount)
    {
        var sampleCount = Mathf.Max(1, (int)(MixRate * duration));
        var data = new byte[sampleCount];

        for (var i = 0; i < sampleCount; i++)
        {
            var t = i / (float)MixRate;
            var envelope = 1f - i / (float)sampleCount;
            var wave = Mathf.Sin(t * frequency * Mathf.Tau) * envelope;
            var noise = (float)GD.RandRange(-1, 1) * envelope * noiseAmount;
            var sample = (wave * 0.55f + noise) * volume;
            data[i] = (byte)Mathf.Clamp(128 + sample * 127f, 0, 255);
        }

        return new AudioStreamWav
        {
            Format = AudioStreamWav.FormatEnum.Format8Bits,
            MixRate = MixRate,
            Stereo = false,
            Data = data
        };
    }
}
