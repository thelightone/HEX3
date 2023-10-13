using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BeAim : MonoBehaviour
{
    [HideInInspector]
    public UnitMoveController moveController;
    [HideInInspector]
    public HexTile curHex;


    public HexTile rightTop;

    public HexTile right;

    public HexTile rightBot;

    public HexTile leftBot;

    public HexTile left;

    public HexTile leftTop;

    public GameObject rightTopPart;
    public GameObject rightPart;
    public GameObject rightBotPart;
    public GameObject leftBotPart;
    public GameObject leftPart;
    public GameObject leftTopPart;

    public GameObject shootPart;
    public GameObject skillPart;

    public Dictionary<GameObject, HexTile> dict = new Dictionary<GameObject, HexTile>();

    public bool enemyNear;

    private void Start()
    {
        moveController = GetComponentInParent<UnitMoveController>();

        dict.Add(rightTopPart, rightTop);
        dict.Add(rightPart, right);
        dict.Add(rightBotPart, rightBot);
        dict.Add(leftBotPart, leftBot);
        dict.Add(leftPart, left);
        dict.Add(leftTopPart, leftTop);
    }

    public void UpdateCoord()
    {
        enemyNear = false;
        curHex = moveController.currentTile;
        int x = curHex.offsetCoord.x;
        int y = curHex.offsetCoord.y;

        rightTop = null;
        right = null;
        rightBot = null;
        leftBot = null;
        left = null;
        leftTop = null;

        if (y % 2 == 1)
        {
            foreach (var tile in curHex.neigh)
            {
                if (tile.unitOn)
                {
                    if (tile.unitOn.player == BattleSystem.Instance.curPlayer)
                    {
                        continue;
                    }
                    else
                    {
                        enemyNear = true;
                    }
                }
                if (tile.offsetCoord == new Vector2Int(x, y - 1))
                {
                    rightTop = tile;
                    rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x + 1, y))
                {
                    right = tile;
                    rightPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x, y + 1))
                {
                    rightBot = tile;
                    rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y + 1))
                {
                    leftBot = tile;
                    leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y))
                {
                    left = tile;
                    leftPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y - 1))
                {
                    leftTop = tile;
                    leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var tile in curHex.neigh)
            {
                if (tile.unitOn)
                {
                    if (tile.unitOn.player == BattleSystem.Instance.curPlayer)
                    {
                        continue;
                    }
                    else
                    {
                        enemyNear = true;
                    }
                }
                if (tile.offsetCoord == new Vector2Int(x + 1, y - 1))
                {
                    rightTop = tile;
                    rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x + 1, y))
                {
                    right = tile;
                    rightPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x + 1, y + 1))
                {
                    rightBot = tile;
                    rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x, y + 1))
                {
                    leftBot = tile;
                    leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y))
                {
                    left = tile;
                    leftPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x, y - 1))
                {
                    leftTop = tile;
                    leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        dict.Clear();

        dict.Add(rightTopPart, rightTop);
        dict.Add(rightPart, right);
        dict.Add(rightBotPart, rightBot);
        dict.Add(leftBotPart, leftBot);
        dict.Add(leftPart, left);
        dict.Add(leftTopPart, leftTop);

    }

    public void DislightAim(int i)
    {
        rightTopPart.transform.GetChild(i).gameObject.SetActive(false);
        rightPart.transform.GetChild(i).gameObject.SetActive(false);
        rightBotPart.transform.GetChild(i).gameObject.SetActive(false);
        leftBotPart.transform.GetChild(i).gameObject.SetActive(false);
        leftPart.transform.GetChild(i).gameObject.SetActive(false);
        leftTopPart.transform.GetChild(i).gameObject.SetActive(false);
        shootPart.transform.GetChild(i).gameObject.SetActive(false);
        skillPart.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Attack(BeAim beAim, GameObject sector)
    {
        if (sector == shootPart && !TileManager.Instance.activeUnit.fightController.melee && !TileManager.Instance.activeUnit.beAim.enemyNear)
        {
            TileManager.Instance.activeUnit.fightController.PreStartShoot(curHex);
        }

        foreach (var key in beAim.dict.Keys)
        {
            if (sector == key)
            {
                TileManager.Instance.activeUnit.fightController.AttackMove(beAim.dict[key], curHex);
            }
        }
    }

    public void LightAim(GameObject selectSector)
    {
        if (dict.ContainsKey(selectSector))
        {
            if (TileManager.Instance.activeUnit.currentRange.Contains(dict[selectSector]))
            {
                DislightAim(1);
                selectSector.transform.GetChild(1).gameObject.SetActive(true);
                TileManager.Instance.ChangeCursor(dict[selectSector], TileManager.CursorState.highlight);
            }
        }
        else if (selectSector == shootPart && !TileManager.Instance.activeUnit.fightController.melee && !TileManager.Instance.activeUnit.beAim.enemyNear)
        {
            DislightAim(1);
            selectSector.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void LightSkill()
    {
        DislightAim(1);
        skillPart.transform.GetChild(1).gameObject.SetActive(true);
    }
}