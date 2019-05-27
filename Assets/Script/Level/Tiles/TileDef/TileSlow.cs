public class TileSlow : TileBase
{
    public int penaltyPA { get; protected set; }

    public TileSlow(string name, string spriteName, int penaltyPA, bool blockingSight = false) : base(name, spriteName, true, blockingSight)
    {
        this.penaltyPA = penaltyPA;
    }
}
