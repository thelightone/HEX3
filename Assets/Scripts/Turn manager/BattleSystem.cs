
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : StateMachine
{
    public PlayerController leftFighter;
    public PlayerController rightFighter;
    public PlayerController curPlayer;
    public float round=1;

    public static BattleSystem Instance;

    public void Start()
    {
        Instance = this;
        SetState(new PlayerTurn(this));
    }

    public void OnAct()
    {
        State.Act();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
