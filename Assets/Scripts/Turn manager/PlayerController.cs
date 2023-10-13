using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    public List <UnitMoveController> units;
    public Player player;

    public enum Player
    {
        left,
        right
    }

    void Start()
    {
        foreach (var unit in units)
        {
            unit.player = this;
        }
    }

    private void ReceiveDamage(float damage, float damageArmor, float damageVamp)
    {

    }

    private void HealthVamp(float netDamage)
    {

    }

    private void Co()
    {

    }

    public void HighLight()
    {
        foreach (var unit in units)
        {
            if (unit.actions>0)
                unit.active.SetActive(true);
            else
                unit.active.SetActive(false);
            unit.beAim.DislightAim(1);
        }
    }

    public void DisLight()
    {
        foreach (var unit in units)
        {
            unit.active.SetActive(false);
        }
    }

    public void Act()
    {

    }

    public bool ActiveUnits()
    {
       return units.Find(unit => unit.actions > 0);
    }

    public void ReactivateUnits()
    {
        foreach (var unit in units)
        {
            unit.actions = 2;
        }
    }

    public List<UnitFightController> FightControllers()
    {
        var fightControllers = new List<UnitFightController>();

        foreach (var item in units)
        {
            fightControllers.Add(item.fightController);
        }

        return fightControllers;
    }
}
