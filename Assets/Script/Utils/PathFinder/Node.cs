public class Node : IHeapItem<Node>
{
    public bool IsWalkable { get; private set; }
    public Int2 NodePosition { get; private set; }
    public int gCost { get; set; }
    public int hCost { get; set; }
    public Node parent { get; set; }
    public int movementPenalty { get; private set; }

    public Node(bool walkable, Int2 position, int penalty)
    {
        IsWalkable = walkable;
        NodePosition = position;
        movementPenalty = penalty;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex { get; set; }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
