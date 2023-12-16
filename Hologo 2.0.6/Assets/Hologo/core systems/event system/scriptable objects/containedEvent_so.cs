using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 26 july 2019
    /// Modified by: 
    /// contained event scriptable object
    /// </summary> 
    [CreateAssetMenu(fileName = "newContainedEvent", menuName = "Hologo V2/new containedEvent", order = 2)]
    public class containedEvent_so : ScriptableObject
    {
        [Tooltip("response to invoke when event is raised")]
        public eventGameObject response;

        public void raise(GameObject go)
        {
            response.Invoke(go);
        }
    }
}