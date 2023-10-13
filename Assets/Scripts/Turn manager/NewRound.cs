using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRound : State
{
    public NewRound(BattleSystem battleSystem) : base(battleSystem)
    {

    }

    public override void Start()
    {
        BuffManager.Instance.NewRound();

        BattleSystem.round++;
        BattleSystem.rightFighter.ReactivateUnits();
        BattleSystem.leftFighter.ReactivateUnits();
        BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }

}

