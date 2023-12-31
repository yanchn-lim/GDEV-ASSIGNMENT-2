using TMPro;
using UnityEngine;

public class BattleStateManager : MonoBehaviour
{
    public BattleState CurrentState;
    public NoBattleState NoBattleState = new();
    public StartBattleState StartBattleState = new();
    public StartPlayerState StartPlayerState = new();
    public EndPlayerState EndPlayerState = new();
    public StartEnemyState StartEnemyState = new();
    public EndEnemyState EndEnemyState = new();
    public EndBattleState EndBattleState = new();
    public int NumberOfTurns = 1;
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        eventManager.AddListener(Event.BATTLE_START, StartBattle);
        eventManager.AddListener(Event.ENEMY_DEATH, EndBattle);
    }
    private void StartBattle()
    {
        NumberOfTurns = 1;
        GameObject.Find("TurnText").GetComponent<TMP_Text>().text = "TURN " + NumberOfTurns.ToString();
        SwitchState(StartBattleState);
    }
    private void EndBattle()
    {
        SwitchState(EndBattleState);
    }
    public void SwitchState(BattleState state)
    {
        CurrentState = state;
        state.OnEnterState(this);
    }

    public void PlayerEndTurn()
    {
        if(CurrentState == StartPlayerState)
        {
            SwitchState(EndPlayerState);
        }
    }

    private void Start()
    {
        SwitchState(NoBattleState);
    }

    public void UpdateTurnNumber()
    { 
        NumberOfTurns += 1;
        GameObject.Find("TurnText").GetComponent<TMP_Text>().text = "TURN "+ NumberOfTurns.ToString();
    }

}
