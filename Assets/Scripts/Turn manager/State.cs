

public abstract class State  
{
    protected BattleSystem BattleSystem;
    public int actions = 0;

    public State (BattleSystem battleSystem)
    {
        BattleSystem = battleSystem;
    }

    public virtual void Start()
    {
        return;
    }
    public virtual void Act()
    {
        return;
    }
}
