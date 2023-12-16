using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// all the clickable monobehaviors extend from this class
[Serializable]
public abstract class clickable : MonoBehaviour,IClickable, IComparable<clickable>
{

    public interactionType myInteractionType;

    public interactionType returnMyType()
    {
        return myInteractionType;
    }

    public virtual void executeAction()
    {
        Debug.Log("item pick up : " + myInteractionType);
    }


    public int CompareTo(clickable other)
    {
        if (this.myInteractionType > other.myInteractionType)
        {
            return 1;
        }
        else if (this.myInteractionType < other.myInteractionType)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
