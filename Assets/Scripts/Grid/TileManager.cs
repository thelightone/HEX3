using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public Dictionary<Vector3Int, HexTile> tiles;

    [SerializeField]
    private GameObject cursor;

    private GameObject selectUnit;
    [SerializeField]
    private GameObject lightUnit;
    [SerializeField]
    private GameObject lightEnemy;

    public UnitMoveController activeUnit;
    public SkillParent currentSkill;


    private void Awake()
    {
        Instance = this;
        tiles = new Dictionary<Vector3Int, HexTile>();

        HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
        foreach (var hextile in hexTiles)
        {
            tiles.Add(hextile.cubeCoord, hextile);
        }

        foreach (var hextile in hexTiles)
        {
            List<HexTile> neighs = GetNeigh(hextile);
            hextile.neigh = neighs;
        }
    }

    private List<HexTile> GetNeigh(HexTile tile)
    {
        List<HexTile> neighs = new List<HexTile>();
        Vector3Int[] neighCoord = new Vector3Int[]
        {
            new Vector3Int (1,-1,0),
            new Vector3Int (1,0,-1),
            new Vector3Int (0, 1,-1),
            new Vector3Int (-1,1,0),
            new Vector3Int (-1,0,1),
            new Vector3Int (0, -1,1),
        };

        foreach (var coord in neighCoord)
        {
            Vector3Int tileCoord = tile.cubeCoord;
            if (tiles.TryGetValue(tileCoord + coord, out HexTile neighbour))
            {
                neighs.Add(neighbour);
            }

        }
        return neighs;
    }

    public void ChangeCursor(HexTile tile, CursorState state)
    {
        SwitchCursor(state);
        cursor.transform.position = tile.transform.position;
    }

    public void Select(HexTile tile)
    {
        if (!tile.busy)
        {
            ChangeCursor(tile, CursorState.select);
            activeUnit.Move(tile, null);
        }
    }

    public void DisSelect()
    {
        SwitchCursor(CursorState.none);
    }

    public void ChooseUnit(UnitMoveController unit)
    {
        if (activeUnit != null)
        {
            DisChooseUnit(activeUnit);
        }
        lightUnit.SetActive(false);
        activeUnit = unit;
        activeUnit.beAim.UpdateCoord();
        activeUnit.choose.SetActive(true);
        activeUnit.Range(activeUnit.currentTile);
    }

    public void DisChooseUnit(UnitMoveController unit)
    {
        if (activeUnit == unit)
        {
            activeUnit.choose.SetActive(false);
            activeUnit.UnRange();
            activeUnit = null;
        }
    }

    public void LightUnit(UnitMoveController unit)
    {
        if (unit != activeUnit)
        {
            lightUnit.SetActive(true);
            lightUnit.transform.position = unit.transform.position - new Vector3(0, 0.9f, 0);
        }
    }
    public void DisLightUnit()
    {
        lightUnit.SetActive(false);
    }

    public void LightEnemy(UnitMoveController unit)
    {
        if (activeUnit.currentRange.Contains(unit.currentTile))
        {
            lightEnemy.SetActive(true);
            lightEnemy.transform.position = unit.transform.position - new Vector3(0, 0.9f, 0);
        }
    }
    public void DisLightEnemy()
    {
        lightEnemy.SetActive(false);
    }

    public enum CursorState
    {
        highlight,
        select,
        busy,
        skill,
        none
    }

    public void SwitchCursor(CursorState state)
    {
        for (int i = 0; i < cursor.transform.childCount; i++)
        {
            cursor.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (state != CursorState.none)
            cursor.transform.GetChild((int)state)?.gameObject.SetActive(true);

        if (state == CursorState.skill)
        {
            var skill = cursor.transform.GetChild((int)state);
            for (int i = 0; i < skill.childCount; i++)
            {
                 skill.transform.GetChild(i).gameObject.SetActive(false);
            }

            switch (currentSkill.hexArea)
            {
                case SkillParent.HexArea.Hex:
                    skill.GetChild(0).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.Circle:
                    skill.GetChild(1).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.Hor3:
                    skill.GetChild(2).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.Hor5:
                    skill.GetChild(3).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.LeftDiag3:
                    skill.GetChild(4).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.LeftDiag5:
                    skill.GetChild(5).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.RightDiag3:
                    skill.GetChild(6).gameObject.SetActive(true);
                    break;
                case SkillParent.HexArea.RightDiag5:
                    skill.GetChild(7).gameObject.SetActive(true);
                    break;
                default:
                    skill.GetChild(8).gameObject.SetActive(true);
                    break;
            }

        }
    }
}
