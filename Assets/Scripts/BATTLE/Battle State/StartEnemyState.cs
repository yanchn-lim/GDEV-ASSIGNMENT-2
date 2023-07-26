using System;
using System.Collections.Generic;
using UnityEngine;
public class StartEnemyState : BattleState
{
    public static event Action OnEnterEnemyStart;
    public static event Func<List<Enemy>> OnEnemyStart;
    public static event Action OnEnemyAction;
    
    List<Enemy> enemyList;
    public override void OnEnterState(BattleStateManager battle)
    {
        Debug.Log("enemy start");

        //PERFORM ACTION AT START OF ENEMY TURN
        enemyList = OnEnemyStart?.Invoke();

        foreach (Enemy enemy in enemyList)
        {
            enemy.PerformAction();
           
        }
        OnEnemyAction?.Invoke();
        //MOVE TO END TURN
        battle.SwitchState(battle.EndEnemyState);
    }
}