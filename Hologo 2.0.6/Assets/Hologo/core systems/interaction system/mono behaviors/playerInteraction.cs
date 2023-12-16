using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// all mouse clicks are handled in this script
public class playerInteraction : MonoBehaviour
{
    public LayerMask clickableLayer;
    public Camera mainCamera;
    public List<clickable> clickableObjects;
    //  public inventoryManager inventory;
    //enemies , npcs and quest givers, activators, loot boxes, items, resources ,movment grid
    // here click detects if it has the relevent layer and finds its component and sends it to the relevent system
    // this will be always selected if there are no other selectibles
    public LeanSelectable arModelSelectable;
    // event invoked when a label is detected .
    public eventGameObject onDetectLabel;
    // event invoked when a quiz pick is detetected
    public eventGameObject onDetectQuizPick;
    // event invoked when a ar model is detected
    public eventGameObject onDetectARModel;
    // event invoked when a ar model is detected
    public eventGameObject onDetectPortal;


    // ALERT : editor the parameter for the IsPointerOverGameObject needs to be -1. 
    private int fingerID = -1; 

    private void Awake()
    {
        // changing to 1 if not editor
#if !UNITY_EDITOR
     fingerID = 0; 
#endif
    }

    // all clicks are registered here
    void Update()
    {
        // checking if user has pressed the mouse
        if (Input.GetMouseButtonDown(0))
        {
            // a simple check to see if pointer is over canvas
            if (EventSystem.current.IsPointerOverGameObject(fingerID))
                return;

                Debug.Log("clicked");
            RaycastHit[] hits;

            // getting a list of all the hits the ray made.
            hits = Physics.RaycastAll(mainCamera.ScreenPointToRay(Input.mousePosition), 1000, clickableLayer);
            
            // we are converting all the hits to gameobjects
            convertRaycastHitsToGameObjects(hits);
            // if nothing clickable was detected we will return
            if (clickableObjects.Count <=0)
            {
                arModelSelectable.Select(true);
                return;
            }
                
            // if there are clickables. and always picking the first in the list and rest are ignored.
            // we check its interaction type and execute the relevent methods.
            switch (clickableObjects[0].myInteractionType)
            {
                case interactionType.LABEL:
                    onDetectLabel.Invoke(clickableObjects[0].gameObject);
                    Debug.Log("clicked label");
                    break;
                case interactionType.QUIZPICK:
                    onDetectQuizPick.Invoke(clickableObjects[0].gameObject);
                    Debug.Log("clicked quiz pick");
                    break;
                case interactionType.ARMODEL:
                   onDetectARModel.Invoke(clickableObjects[0].gameObject);
                    Debug.Log("clicked ar model");
                    break;
                case interactionType.PORTAL:
                    onDetectPortal.Invoke(clickableObjects[0].gameObject);
                    Debug.Log("clicked portal");
                    break;
                default:
                    arModelSelectable.Select(true);
                    break;
            }

        }

    }

    // converting all the hits to gameobjects
    void convertRaycastHitsToGameObjects(RaycastHit[] hits)
    {
        clickableObjects = new List<clickable>();
        for (int i = 0; i < hits.Length; i++)
        {
            clickableObjects.Add(hits[i].collider.gameObject.GetComponent<clickable>());
        }
        // here we are sorting the list according to the most important clicked item by way of InteractionType.
        clickableObjects.Sort();

       // var nn = clickableObjects.Find(m => m.gameObject.GetComponent<clickable>().myInteractionType == interactionType.activator);
    }

}

[System.Serializable]
public class eventGameObject : UnityEvent<GameObject> { };

//