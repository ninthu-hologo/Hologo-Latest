using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setColliderSize : MonoBehaviour
{

    [SerializeField]
    private BoxCollider bCollider;

    public void setCollider(GameObject model)
    {
        BoxCollider boxCol = model.GetComponentInChildren(typeof(BoxCollider)) as BoxCollider;
        bCollider.size = boxCol.size;
        bCollider.center = boxCol.center;
        boxCol.enabled = false;

    }

   
}
