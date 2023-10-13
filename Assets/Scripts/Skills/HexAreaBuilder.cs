using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static SkillParent;

public class HexAreaBuilder
{
    private List<HexTile> list = new List<HexTile>();
    private HexTile tile;
    private HexArea form;

    public List<HexTile> Builder(HexTile startTile, HexArea startForm)
    {
        form = startForm;
        tile = startTile;
        list.Clear();
        Vector3Int[] neighCoord = null;

        switch (form)
        {
            case HexArea.All:
                return TileManager.Instance.tiles.Values.ToList();
            case HexArea.Circle:
                return tile.neigh;
            case HexArea.Hex:
                list.Add(tile);
                list.AddRange(tile.neigh);
                return list;
            case HexArea.LeftDiag5:
                neighCoord = new Vector3Int[]
{
            new Vector3Int (0, 1,-1),
            new Vector3Int (0, -1,1),
};
                break;
            case HexArea.LeftDiag3:
               neighCoord = new Vector3Int[]
{
            new Vector3Int (0, 1,-1),
            new Vector3Int (0, -1,1),
};
                break;
            case HexArea.RightDiag5:
                neighCoord = new Vector3Int[]
{
 new Vector3Int (1,-1,0),
            new Vector3Int (-1,1,0),
};
                break;
            case HexArea.RightDiag3:
               neighCoord = new Vector3Int[]
{
 new Vector3Int (1,-1,0),
            new Vector3Int (-1,1,0),
};
                break;
            case HexArea.Hor5:
             neighCoord = new Vector3Int[]
{
            new Vector3Int (1,0,-1),
            new Vector3Int (-1,0,1),
};
                break;
            case HexArea.Hor3:
               neighCoord = new Vector3Int[]
{
 new Vector3Int (1,0,-1),
            new Vector3Int (-1,0,1),
};
                break;
        }

        list.Add(tile);

        foreach (var coord in neighCoord)
        {
            Vector3Int tileCoord = tile.cubeCoord;
            if (TileManager.Instance.tiles.TryGetValue(tileCoord + coord, out HexTile neighbour))
            {
                list.Add(neighbour);
                Vector3Int tileCoordNeigh = neighbour.cubeCoord;

                if ((form == HexArea.LeftDiag5 || form == HexArea.RightDiag5 || form == HexArea.Hor5) && TileManager.Instance.tiles.TryGetValue(tileCoordNeigh + coord, out HexTile neighbour2))
                {
                    list.Add(neighbour2);
                }
            }
        }

        return list;
    }
}
