using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    private Grid associatedGrid;

    public PathFinder (Level level)
    {
        associatedGrid = new Grid(level);
    }

    public void ModifyGrid(Int2 pos, Node newNode)
    {
        associatedGrid.nodes[pos.x, pos.y] = newNode;
    }

    public void ModifyGrid (int x, int y, Node newNode)
    {
        associatedGrid.nodes[x, y] = newNode;
    }

    public Path FindPath(Int2 startPos, Int2 endPos)
    {
        Node startNode = associatedGrid.GetNodeAt(startPos);
        Node endNode = associatedGrid.GetNodeAt(endPos);

        Heap<Node> openSet = new Heap<Node>(associatedGrid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.Pop();
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return new Path(RetracePath(startNode, endNode));
            }

            foreach (Node neighbour in associatedGrid.GetNeighbours(currentNode))
            {
                if ((!neighbour.IsWalkable && neighbour != endNode) || closedSet.Contains(neighbour))
                {
                    continue; 
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        return null;
    }

    public bool IsStuck(Int2 pos)
    {
        Node node = associatedGrid.GetNodeAt(pos);
        if(associatedGrid.GetNeighbours(node).Length == 0)
        {
            return true;
        }
        else
        {
            Node[] neighbours = associatedGrid.GetNeighbours(node);
            if(neighbours.Count(n => n.IsWalkable) == 0)
            {
                return true;
            }
        }
        return false;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        if(!currentNode.IsWalkable)
        {
            currentNode = endNode.parent;
        }
        while (currentNode != startNode && currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.NodePosition.x - nodeB.NodePosition.x);
        int dstY = Mathf.Abs(nodeA.NodePosition.y - nodeB.NodePosition.y);

        return dstX + dstY;
    }
}
