using System.Collections.Generic;

public class Grid
{
    public Node[,] nodes { get; set; }
    public int Width { get { return nodes.GetLength(0); } }
    public int Height { get { return nodes.GetLength(1); } }

    public Grid(Level level)
    {
        int width = level.arena.Width;
        int height = level.arena.Height;
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new Node(level.IsTileWalkable(x, y), new Int2(x, y), level.arena.GetTileMovementCost(x,y));
            }
        }
    }

    public bool IsInsideRange(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public int MaxSize
    {
        get { return Width * Height; }
    }

    public Node GetNodeAt(Int2 pos)
    {
        return GetNodeAt(pos.x, pos.y);
    }

    public Node GetNodeAt(int x, int y)
    {
        if (IsInsideRange(x, y))
            return nodes[x, y];
        else
            return null;
    }

    public Node[] GetNeighbours(Node node)
    {
        int x = node.NodePosition.x;
        int y = node.NodePosition.y;
        List<Node> neighbours = new List<Node>();
        if(GetNodeAt(x + 1,y) != null)
        {
            neighbours.Add(GetNodeAt(x + 1, y));
        }
        if (GetNodeAt(x, y + 1) != null)
        {
            neighbours.Add(GetNodeAt(x, y + 1));
        }
        if (GetNodeAt(x - 1, y) != null)
        {
            neighbours.Add(GetNodeAt(x - 1, y));
        }
        if (GetNodeAt(x, y - 1) != null)
        {
            neighbours.Add(GetNodeAt(x, y - 1));
        }
        return neighbours.ToArray();
    }
}
