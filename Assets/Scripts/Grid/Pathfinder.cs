using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Pathfinder
{
    

    public static List<HexTile> FindPath(HexTile origin, HexTile destination, bool move)
    {
        Debug.Log("2");
        var nodesNotEvaluated = new Dictionary<HexTile, Node>();
        var nodesAlreadyEvaluated = new Dictionary<HexTile, Node>();
        Debug.Log("3");
        Node startNode = new Node(origin, origin, destination, 0);
        nodesNotEvaluated.Add(origin, startNode);
        Debug.Log("4");
        bool gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, move, out List<HexTile> path);
        Debug.Log("5");
        while (!gotPath)
        {
            gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination,move, out path);
        }
        return path;
    }

    private static bool EvaluateNextNode(Dictionary<HexTile, Node> nodesNotEvaluated, Dictionary<HexTile, Node> nodesAlreadyEvaluated, HexTile origin, HexTile destination, bool move, out List<HexTile> path)
    {
       


        var currentNode = GetCheapestNode(nodesNotEvaluated.Values.ToArray());

        if (currentNode == null)
        {
            path = new List<HexTile>();
            return false;
        }

        nodesNotEvaluated.Remove(currentNode.target);
        nodesAlreadyEvaluated.Add(currentNode.target, currentNode);

        path = new List<HexTile>();

        if (currentNode.target == destination)
        {
            path.Add(currentNode.target);

            while (currentNode.target != origin)
            {
                path.Add(currentNode.parent.target);
                currentNode = currentNode.parent;
            }
            return true;
        }

        //otherwise
        List<Node> neighs = new List<Node>();

        foreach (HexTile tile in currentNode.target.neigh)
        {
           
                Node node = new Node(tile, origin, destination, currentNode.GetCost());
                neighs.Add(node);
            
        }

        foreach (Node neigh in neighs)
        {
            if (move)
            {
                if (neigh.target.busy)
                {
                    continue;
                }
            }
            if (nodesAlreadyEvaluated.Keys.Contains(neigh.target))
            {
                continue;
            }

            if (neigh.GetCost() < currentNode.GetCost() || !nodesNotEvaluated.Keys.Contains(neigh.target))
            {
                neigh.SetParent(currentNode);
                if (!nodesNotEvaluated.Keys.Contains(neigh.target))
                {
                    nodesNotEvaluated.Add(neigh.target, neigh);
                }
            }
        }

        return false;
    }

    private static Node GetCheapestNode(Node[] nodesNotEvaluated)
    {
        if (nodesNotEvaluated.Length == 0)
        {
            return null;
        }

        Node selectedNode = nodesNotEvaluated[0];

        for (int i = 1; i < nodesNotEvaluated.Length; i++)
        {
            var currentNode = nodesNotEvaluated[i];
            if (
                currentNode.GetCost() < selectedNode.GetCost()
                ||
                (currentNode.GetCost() == selectedNode.GetCost()) && currentNode.costToDestination < selectedNode.costToDestination
                )
            {
                selectedNode = currentNode;
            }
        }
        return selectedNode;
    }
}



