public class EnemyTurn : State
{ 
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
    {

    }

    public override void Start()
    {
        if (!BattleSystem.rightFighter.ActiveUnits())
        {
            if (!BattleSystem.leftFighter.ActiveUnits())
                BattleSystem.SetState(new NewRound(BattleSystem));
            else
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
        else
        {
            BattleSystem.leftFighter.DisLight();
            actions = 0;
            BattleSystem.curPlayer = BattleSystem.rightFighter;
            BattleSystem.rightFighter.HighLight();
        }
    }

    public override void Act()
    {
        actions++;

        BattleSystem.rightFighter.DisLight();
        BattleSystem.rightFighter.HighLight();

        if (actions > 1)
        {
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }

    }
}

