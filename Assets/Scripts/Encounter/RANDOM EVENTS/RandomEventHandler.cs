using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomEventHandler : MonoBehaviour
{
    private RandomEvents currentEventName;
    private EventManager eventManager = EventManager.Instance;
    public GameObject[] eventPrefabs;
    private GameObject currentEventObject;

    private void Awake()
    {
        //subscribing to events
        eventManager.AddListener<Node>(Event.RAND_EVENT_INITIALIZE, Initialize);
        eventManager.AddListener(Event.RAND_EVENT_END, EndEvent);
    }

    private void OnDestroy()
    {
        //unsubscribing to events
        eventManager.RemoveListener<Node>(Event.RAND_EVENT_INITIALIZE, Initialize);
        eventManager.RemoveListener(Event.RAND_EVENT_END, EndEvent);
    }

    private void Initialize(Node node) {
        //get current event
        eventManager.TriggerEvent(Event.MAP_NODE_CLICKED);
        currentEventName = node.RandomEvent;

        // starts the corresponding event
        InitializeEvent(currentEventName);
    }

    private void InitializeEvent(RandomEvents eventName)
    {
        switch (eventName)
        {
            case RandomEvents.SpinTheWheel:
                currentEventObject = Instantiate(eventPrefabs[0],this.transform);
                break;
            case RandomEvents.FreeUpgrade:
                currentEventObject = Instantiate(eventPrefabs[1], this.transform);
                break;
            case RandomEvents.ReachInDepth:
                currentEventObject = Instantiate(eventPrefabs[2], this.transform);
                break;
        }
    }

    private void EndEvent()
    {
        //destroys the event object and set the overlay to inactive
        Destroy(currentEventObject);
        this.gameObject.SetActive(false);
    }
    
}
