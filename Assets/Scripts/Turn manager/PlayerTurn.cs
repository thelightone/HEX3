public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
    {
        
    }
    public override void Start()
    {
        if (!BattleSystem.leftFighter.ActiveUnits())
        {
            if (!BattleSystem.rightFighter.ActiveUnits())
                BattleSystem.SetState(new NewRound(BattleSystem));
            else
                BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }
        else
        {
            BattleSystem.rightFighter.DisLight();
            actions = 0;
            BattleSystem.curPlayer = BattleSystem.leftFighter;
            BattleSystem.leftFighter.HighLight();
        }
    }

    public override void Act()
    {
        actions++;

        BattleSystem.leftFighter.DisLight();
        BattleSystem.leftFighter.HighLight();

        if (actions > 1)
        {
           
            BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }

    }
}
