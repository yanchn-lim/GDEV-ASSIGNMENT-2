using System;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    GameObject[] battleObjects;
    GameObject[] restObjects;
    GameObject[] eventObjects;

    private EventManager eventManager = EventManager.Instance; 

    private void Start()
    {
        battleObjects = GameObject.FindGameObjectsWithTag("Battle");
        SetInactive(battleObjects);

        restObjects = GameObject.FindGameObjectsWithTag("Rest");
        SetInactive(restObjects);

        eventObjects = GameObject.FindGameObjectsWithTag("Events");
        SetInactive(eventObjects);

        //EVENTS
        eventManager.AddListener<Node>(Event.MAP_NODE_CLICKED,StartEncounter);
        eventManager.AddListener(Event.REST_FINISHED, EndRest);
        eventManager.AddListener(Event.BATTLE_END, EndBattle);
    }

    private void OnDestroy()
    {
        eventManager.RemoveListener<Node>(Event.MAP_NODE_CLICKED, StartEncounter);
        eventManager.RemoveListener(Event.REST_FINISHED, EndRest);
        eventManager.RemoveListener(Event.BATTLE_END, EndBattle);
    }
    private void StartEncounter(Node node)
    {
        Debug.Log("start encounter");
        switch (node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                StartBattle(node);
                eventManager.TriggerEvent(Event.BATTLE_START);
                break;

            case Node.Encounter.ELITE:
                StartBattle(node);
                eventManager.TriggerEvent(Event.BATTLE_START);
                break;

            case Node.Encounter.EVENT:
                StartRandomEvent(node);
                break;

            case Node.Encounter.REST:
                StartRest(node);
                break;

            case Node.Encounter.BOSS:
                StartBattle(node);
                eventManager.TriggerEvent(Event.BATTLE_START);
                break;
        }
        //DISABLE MAP CLICKY
        //CLOSE MAP
    }
    
    private void SetInactive(GameObject[] list)
    {
        foreach (GameObject item in list)
        {
            item.SetActive(false);
        }
    }

    #region Random Event
    private void StartRandomEvent(Node node)
    {
        foreach (GameObject item in eventObjects)
        {
            item.SetActive(true);
        }
        eventManager.TriggerEvent<Node>(Event.RAND_EVENT_INITIALIZE, node);

    }
    #endregion

    #region Rest
    private void StartRest(Node node)
    {
        Debug.Log("start rest");
        //enabling all rest ui
        foreach (GameObject item in restObjects)
        {
            Debug.Log(item.name);
            item.SetActive(true);
        }

        //initialize rest event
        eventManager.TriggerEvent<Node>(Event.REST_INITIALIZE,node);
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
        //close map
    }

    private void EndRest()
    {
        //DISABLING ALL REST UI
        SetInactive(restObjects);

        //open map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }
    #endregion

    #region BATTLE
    private void StartBattle(Node node)
    {
        Debug.Log("start battle");
        //SET ALL BATTLE-RELATED OBJECTS TO ACTIVE
        foreach (GameObject item in battleObjects)
        {
            item.SetActive(true);
        }

        //spawn enemy in the container
        GameObject enemyContainer = GameObject.Find("EnemySpot");
        Debug.Log(enemyContainer.name);
        foreach (GameObject enemy in node.EnemyList)
        {
            Instantiate(enemy, enemyContainer.transform);
        }
        //close the map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }

    private void EndBattle()
    {
        //remove enemies
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            Destroy(enemy);
        }
        //hide battle ui
        foreach (GameObject item in battleObjects)
        {
            item.SetActive(false);
        }

        //open map
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
    }
    #endregion

}
