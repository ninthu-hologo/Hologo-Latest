using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 26 july 2019
    /// Modified by: 
    /// event scriptable object
    /// </summary>
    [CreateAssetMenu(fileName = "newEvent", menuName = "Hologo V2/new event", order = 1)]
    public class event_SO : ScriptableObject
    {
        // details of the events for the event raiser and receiver for easy reference
        public List<eventDetailsInfo> eventDetails;
        // list of registere listneres
        private readonly List<eventListener> eventListeners = new List<eventListener>();

        // raise the event for all registered listeneres
        public void raise(GameObject go)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnEventRaised(go);
            }
        }

        public void raiseBoolEvent(bool state)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnBoolEventRaised(state);
            }
        }

        // register 
        public void registerListener(eventListener listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        // unregister
        public void unregisterListener(eventListener listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }

    }

    // name of event raiser and event receiver
    [System.Serializable]
    public class eventDetailsInfo
    {
        public string eventRaiser;
        public string eventReceiver;
        public int passedInteger; //Shariz adding a variable to pass for event 27th May 2020 2.0.6
        public string passedMessage; //Shariz adding a variable to pass for event 27th May 2020 2.0.6
    }
}