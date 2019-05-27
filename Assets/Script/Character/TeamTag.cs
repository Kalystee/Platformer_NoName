using UnityEngine;

public class TeamTag
{
    public Color color { get; private set; }

    public TeamTag(Color color)
    {
        this.color = color;
    }

    public TeamTag(byte r, byte g, byte b)
    {
        this.color = new Color32(r, g, b, 255);
    }

    public override bool Equals(object obj)
    {
        if (obj is TeamTag)
        {
            TeamTag tg = obj as TeamTag;
            return tg.color == color;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static TeamTag Ally = new TeamTag(45, 255, 25);
    public static TeamTag Neutral = new TeamTag(80, 80, 80);
    public static TeamTag Enemy = new TeamTag(255, 50, 25);
}
