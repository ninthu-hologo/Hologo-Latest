using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 26 july 2019
    /// Modified by: 
    /// event listener class
    /// </summary>
    public class eventListener : MonoBehaviour
    {
        [Tooltip("event to register with")]
        public event_SO gameEvent;

        [Tooltip("response to invoke when event is raised")]
        public eventGameObject gameObjectResponse;
        [Tooltip("response to invoke when event is raised")]
        public eventBool boolEventResponse;


        private void OnEnable()
        {
            gameEvent.registerListener(this);
        }

        private void OnDisable()
        {
            gameEvent.unregisterListener(this);
        }


        public void OnEventRaised(GameObject go)
        {
            Debug.Log(" an event has been called");
            gameObjectResponse.Invoke(go);
        }

        public void OnBoolEventRaised(bool state)
        {
            boolEventResponse.Invoke(state);
        }

    }

    [System.Serializable]
    public class eventGameObject : UnityEvent<GameObject> { };

    [System.Serializable]
    public class eventBool : UnityEvent<bool> { };
}