using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifyParameterSkill", menuName = "Skills/ModifyParameter", order = 51)]
public class ModifyParameterSkill : SkillParent
{
    public List<Effect> changes = new List<Effect>();

    private float _parameter;

    public override void OnActivate()
    {
        if (aimType == AimType.unit)
        {
            invoker.moveController.TurnToEnemy(unitAims[0].moveController.currentTile, false, false);
        }
        else if (aimType == AimType.oneTimeHex || aimType == AimType.oneTimeHexes || aimType == AimType.staticHex || aimType == AimType.staticHexes)
        {
            invoker.moveController.TurnToEnemy(hexAims[0], false, false);
        }

        animator.SetTrigger("Skill");
    }

    public override void OnAnimEvent()
    {
        effectsController.PlayInvoker();

        if (effectShoot == EffectShoot.noShoot)
            effectsController.StartCor(IOnReactivate(), pauseBeforeEffect);
        else
            effectsController.StartCorShoot(IOnReactivate(), pauseBeforeEffect, invoker.gameObject, unitAims, hexAims);
    }

    public override void OnReactivate()
    {
        effectsController.StartCor(IOnReactivate(), 0);
    }

    public IEnumerator IOnReactivate()
    {

        effectsController.PlayAimUnits(unitAims);

        if (unitAims.Count == 0 && hexAims.Count > 0)
        {
            unitAims = HexToUnitConverter(hexAims);
        }

        Modification();

        if (effectPositive == EffectPositive.negative)
            unitAims.ToList().ForEach(i => i.ReceiveDamage(false));

        if (aimType == AimType.staticHex || aimType == AimType.staticHexes)
        {
            unitAims.Clear();
        }
        yield return null;
    }



    private void Change(Effect change)
    {
        _parameter += change.IncreaseBy;
        _parameter *= change.MultiplyBy;
    }

    public List<UnitFightController> HexToUnitConverter(List<HexTile> hexes)
    {
        var units = new List<UnitFightController>();
        foreach (var hex in hexes)
        {
            if (hex.busy)
            {
                if ((aimPlayer == AimPlayer.self && hex.unitOn.player == invoker.moveController.player)
                || (aimPlayer == AimPlayer.enemy && hex.unitOn.player != invoker.moveController.player)
                || aimPlayer == AimPlayer.both)
                {
                    units.Add(hex.unitOn.fightController);
                }
            }
        }
        return units;
    }

    public void Modification()
    {
        foreach (var unitAim in unitAims)
        {
            foreach (Effect change in changes)
            {
                if (change.changedParameter == UnitParameter.melee)
                {
                    unitAim.melee = !unitAim.melee;
                }

                _parameter = 0;

                switch (change.changedParameter)
                {
                    case UnitParameter.health:
                        _parameter = unitAim.health;
                        Change(change);
                        unitAim.health = _parameter;
                        break;
                    case UnitParameter.damageMelee:
                        _parameter = unitAim.damageMelee;
                        Change(change);
                        unitAim.damageMelee = _parameter;
                        break;
                    case UnitParameter.damageRange:
                        _parameter = unitAim.damageRange;
                        Change(change);
                        unitAim.damageRange = _parameter;
                        break;
                    case UnitParameter.armor:
                        _parameter = unitAim.armor;
                        Change(change);
                        unitAim.armor = _parameter;
                        break;
                    case UnitParameter.goRange:
                        _parameter = unitAim.goRange;
                        Change(change);
                        unitAim.goRange = _parameter;
                        break;
                    case UnitParameter.missAbil:
                        _parameter = unitAim.missAbil;
                        Change(change);
                        unitAim.missAbil = _parameter;
                        break;
                    case UnitParameter.critChance:
                        _parameter = unitAim.critChance;
                        Change(change);
                        unitAim.critChance = _parameter;
                        break;
                    case UnitParameter.critModif:
                        _parameter = unitAim.critModif;
                        Change(change);
                        unitAim.critModif = _parameter;
                        break;
                    case UnitParameter.vamp:
                        _parameter = unitAim.vamp;
                        Change(change);
                        unitAim.vamp = _parameter;
                        break;
                    case UnitParameter.cost:
                        _parameter = unitAim.cost;
                        Change(change);
                        unitAim.cost = (int)_parameter;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [System.Serializable]
    public struct Effect
    {
        public UnitParameter changedParameter;
        public float IncreaseBy;
        public float MultiplyBy;
    }

    public enum UnitParameter
    {
        health,
        damageMelee,
        damageRange,
        armor,
        goRange,
        missAbil,
        critChance,
        critModif,
        vamp,
        cost,
        melee
    }
}
