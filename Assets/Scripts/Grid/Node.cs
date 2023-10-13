using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Node
    {
        public Node parent;
        public HexTile target;
        public HexTile destination;
        public HexTile origin;

        public int baseCost;
        public int costFromOrigin;
        public int costToDestination;
        public int pathCost;

        public Node(HexTile current, HexTile origin, HexTile destination, int pathCost)
        {
        parent = null;
        this.target = current;
        this.origin = origin;
        this.destination = destination;

        baseCost = 1;
        costFromOrigin = (int)Vector3Int.Distance(current.cubeCoord, origin.cubeCoord);
        costToDestination = (int)Vector3Int.Distance(current.cubeCoord, destination.cubeCoord);
        this.pathCost = pathCost;
    }

        public int GetCost()
        {
            return pathCost + baseCost + costToDestination + costFromOrigin;
        }

        public void SetParent(Node node)
        {
            this.parent = node;
        }
    }
