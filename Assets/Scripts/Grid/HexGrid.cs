
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private GameObject mainTile;

    private void Awake()
    {
      //  Layout();
    }
    public void Layout()
    {
        Clear();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.x; y++)
            {
                GameObject tile = new GameObject($"Hex {x},{y}");
                Instantiate(mainTile, tile.transform);
                tile.transform.position = ShiftHex(new Vector2Int(x, y));
                HexTile hex = tile.AddComponent<HexTile>();
                hex.offsetCoord = new Vector2Int(x, y);
                hex.cubeCoord = CubeCoord(hex.offsetCoord);
                tile.transform.SetParent(gameObject.transform);
            }
        }
    }

    private Vector3 ShiftHex(Vector2Int coord)
    {
        int column = coord.x;
        int row = coord.y;

        bool shouldOffset = (row % 2) == 0;
        float width = Mathf.Sqrt(3) / 1.9f;
        float height = 2f / 1.9f;

        float horDistance = width;
        float vertDistance = height * 0.75f;

        float offset = (shouldOffset ? width / 2 : 0);

        float xPos = column * horDistance + offset;
        float yPos = row * vertDistance;

        return new Vector3(xPos, 0, -yPos);
    }

    private Vector3Int CubeCoord (Vector2Int coord)
    {
        var q = coord.x - (coord.y + (coord.y % 2)) / 2;
        var r = coord.y;

        return new Vector3Int(q, r, -q - r);
    }

    private void Clear()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}

