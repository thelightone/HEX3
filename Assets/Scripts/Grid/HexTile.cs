using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector2Int offsetCoord;
    public Vector3Int cubeCoord;
    public List<HexTile> neigh;
    public bool busy;
    public UnitMoveController unitOn;



    public void OnDrawGizmosSelected()
    {
        foreach (var neig in neigh)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, neig.transform.position);
        }
    }

    public void Highlight()
    {
        if (!TileManager.Instance.activeUnit.isMoving)
        {
            if (!busy && TileManager.Instance.activeUnit.currentRange.Contains(this))
                TileManager.Instance.ChangeCursor(this, TileManager.CursorState.highlight);
            else
                TileManager.Instance.ChangeCursor(this, TileManager.CursorState.busy);
        }
    }

    public void Select()
    {
        TileManager.Instance.Select(this);
    }

    public void MakeFree()
    {
        busy = false;
        unitOn = null;
    }
}
