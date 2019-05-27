public struct EffectZone
{
    //Propriétés : Zone d'effet (true = effet)
    public bool[,] effectZone { get; private set; }

    public int Width { get { return effectZone.GetLength(0); } }
    public int Height { get { return effectZone.GetLength(1); } }

    public Int2 Center { get { return new Int2(Width / 2, Height / 2); } }

    /// <summary>
    /// Création d'une zone d'effet
    /// </summary>
    /// <param name="customEffectZone">Zone d'effet custom</param>
    public EffectZone(bool[,] customEffectZone)
    {
        effectZone = customEffectZone;
    }

    /// <summary>
    /// Création d'une zone d'effet
    /// </summary>
    /// <param name="type">Type de zone d'effet</param>
    /// <param name="radius">Rayon de la zone d'effet</param>
    public EffectZone(EffectZoneType type, int radius)
    {
        effectZone = new bool[radius * 2 + 1, radius * 2 + 1];
        int width = effectZone.GetLength(0);
        int height = effectZone.GetLength(1);
        if (type == EffectZoneType.Radius)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Int2.Distance(new Int2(x, y), new Int2(radius, radius)) <= radius)
                    {
                        effectZone[x, y] = true;
                    }
                }
            }
        }
        else if (type == EffectZoneType.Line)
        {
            int x = radius;
            for (int y = 0; y < height; y++)
            {
                effectZone[x, y] = true;
            }
        }
        else if (type == EffectZoneType.Parallel_Line)
        {
            int y = height;
            for (int x = 0; x < width; x++)
            {
                effectZone[x, y] = true;
            }
        }
        else if (type == EffectZoneType.Cross)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == radius || y == radius)
                    {
                        effectZone[x, y] = true;
                    }
                }
            }
        }
        else if (type == EffectZoneType.Diagonals)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == y || x + y == radius * 2)
                    {
                        effectZone[x, y] = true;
                    }
                }
            }
        }
        else if (type == EffectZoneType.Double_Cross)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == y || x + y == radius * 2 || x == radius || y == radius)
                    {
                        effectZone[x, y] = true;
                    }
                }
            }
        }
    }
}

public enum EffectZoneType
{
    Radius,
    Line,
    Parallel_Line,
    Cross,
    Diagonals,
    Double_Cross,
}
