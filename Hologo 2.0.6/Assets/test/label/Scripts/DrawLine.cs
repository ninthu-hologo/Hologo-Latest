using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

            // Reference to the first GameObject
    public GameObject gameObject2;          // Reference to the second GameObject
    public Material LineMaterial;

    private LineRenderer line;                           // Line Renderer

    // Use this for initialization
    void Start()
    {
        // Add a Line Renderer to the GameObject
        line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.startWidth = 0.001f;
        line.positionCount = 2;
        line.startColor = Color.white;
        line.material = LineMaterial;

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the GameObjects are not null
        if (gameObject2 != null)
        {
            // Update position of the two vertex of the Line Renderer
            line.SetPosition(0, transform.position);
            line.SetPosition(1, gameObject2.transform.position);
        }
    }
}
