using Godot;

public static class PlaceholderSfx
{
    private const int MixRate = 22050;

    public static AudioStreamWav CreateLightSwing() => CreateBurst(0.06f, 118f, 0.42f, 0.18f);

    public static AudioStreamWav CreateHeavySwing() => CreateBurst(0.12f, 52f, 0.62f, 0.35f);

    public static AudioStreamWav CreateComboSwing() => CreateBurst(0.09f, 86f, 0.58f, 0.28f);

    public static AudioStreamWav CreateHitLight() => CreateBurst(0.07f, 104f, 0.5f, 0.22f);

    public static AudioStreamWav CreateHitHeavy() => CreateBurst(0.11f, 64f, 0.68f, 0.38f);

    public static AudioStreamWav CreateHitCombo() => CreateBurst(0.09f, 88f, 0.6f, 0.3f);

    public static AudioStreamWav CreateDeathCrunch() => CreateBurst(0.16f, 58f, 0.7f, 0.4f);

    public static AudioStreamWav CreateExecutionCrunch() => CreateBurst(0.2f, 48f, 0.75f, 0.5f);

    public static AudioStreamWav CreateGutRip() => CreateBurst(0.18f, 42f, 0.72f, 0.55f);

    public static AudioStreamWav CreateSkullCrush() => CreateBurst(0.14f, 34f, 0.8f, 0.48f);

    public static AudioStreamWav CreateDismemberRip() => CreateBurst(0.14f, 36f, 0.8f, 0.62f);

    public static AudioStreamWav CreatePlayerHurtLight() => CreateBurst(0.09f, 82f, 0.4f, 0.26f);

    public static AudioStreamWav CreatePlayerHurtHeavy() => CreateBurst(0.13f, 58f, 0.55f, 0.36f);

    public static AudioStreamWav CreateEnemyTelegraph() => CreateSweep(0.14f, 180f, 62f, 0.38f);

    public static AudioStreamWav CreateEnemySwing() => CreateBurst(0.08f, 76f, 0.52f, 0.32f);

    public static AudioStreamWav CreateEnemySwingHeavy() => CreateBurst(0.1f, 48f, 0.64f, 0.4f);

    public static AudioStreamWav CreateMemoryCollect() => CreateSweep(0.22f, 320f, 520f, 0.45f);

    public static AudioStreamWav CreateMiniBossIntro() => CreateBurst(0.24f, 38f, 0.72f, 0.44f);

    public static AudioStreamWav CreateEncounterPulse() => CreateSweep(0.12f, 240f, 140f, 0.32f);

    public static AudioStreamWav CreateDodgeRoll() => CreateSweep(0.1f, 200f, 90f, 0.36f);

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

        return CreateWav(data);
    }

    private static AudioStreamWav CreateSweep(float duration, float startFrequency, float endFrequency, float volume)
    {
        var sampleCount = Mathf.Max(1, (int)(MixRate * duration));
        var data = new byte[sampleCount];

        for (var i = 0; i < sampleCount; i++)
        {
            var progress = i / (float)sampleCount;
            var envelope = 1f - progress * 0.85f;
            var frequency = Mathf.Lerp(startFrequency, endFrequency, progress);
            var t = i / (float)MixRate;
            var wave = Mathf.Sin(t * frequency * Mathf.Tau) * envelope;
            var sample = wave * volume;
            data[i] = (byte)Mathf.Clamp(128 + sample * 127f, 0, 255);
        }

        return CreateWav(data);
    }

    private static AudioStreamWav CreateWav(byte[] data)
    {
        return new AudioStreamWav
        {
            Format = AudioStreamWav.FormatEnum.Format8Bits,
            MixRate = MixRate,
            Stereo = false,
            Data = data
        };
    }
}
