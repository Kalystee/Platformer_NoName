using System.Collections.Generic;
using System.Linq;

public class Path
{
    public List<Node> NodePath { get; private set; }
    public int Cost { get { return NodePath.Sum(n => n.movementPenalty); } }

    public Path(List<Node> path)
    {
        NodePath = path;
    }

    public Node GetNodeWithCostAt(int cost)
    {
        if(NodePath.Count == 0)
        {
            return null;
        }
        if(cost > Cost)
        {
            return NodePath.Last();
        }
        else
        {
            int i = 0;
            int currentCost = 0;
            while(currentCost < cost)
            {
                currentCost += NodePath[i].movementPenalty;
                i++;
            }
            if(i - 1 < 0)
            {
                return null;
            }
            return NodePath[i - 1];
        }
    }

    public int GetCostTo(Node target)
    {
        int currentCost = 0;
        foreach (Node n in NodePath)
        {
            currentCost += n.movementPenalty;
            if (n == target)
            {
                return currentCost;
            }
        }
        return currentCost;
    }
}
