using Godot;

public static class PixelSpritePalette
{
    public static readonly Color Skin = new("#c46f35");
    public static readonly Color SkinShadow = new("#a85f34");
    public static readonly Color Paint = new("#e0b75d");
    public static readonly Color Hair = new("#15110f");
    public static readonly Color Wood = new("#2b1b16");
    public static readonly Color Cloth = new("#8f1f17");
    public static readonly Color Iron = new("#5f6970");
    public static readonly Color Leather = new("#4a3a32");
    public static readonly Color Cape = new("#2b1b16");
    public static readonly Color Transparent = new(0, 0, 0, 0);
}

public static class PixelCanvas
{
    public static Image Create(int width, int height)
    {
        var image = Image.CreateEmpty(width, height, false, Image.Format.Rgba8);
        image.Fill(PixelSpritePalette.Transparent);
        return image;
    }

    public static void Set(Image image, int x, int y, Color color)
    {
        if (x < 0 || y < 0 || x >= image.GetWidth() || y >= image.GetHeight())
        {
            return;
        }

        image.SetPixel(x, y, color);
    }

    public static void FillRect(Image image, int x, int y, int w, int h, Color color)
    {
        for (var py = y; py < y + h; py++)
        {
            for (var px = x; px < x + w; px++)
            {
                Set(image, px, py, color);
            }
        }
    }

    public static ImageTexture ToTexture(Image image)
    {
        return ImageTexture.CreateFromImage(image);
    }
}
