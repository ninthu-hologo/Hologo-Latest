using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfadeLerp : MonoBehaviour {

    private Material material;
    private MeshRenderer mr;
    private SkinnedMeshRenderer smr;
    public float fadeSpeed = 0.5f;
    public Color EmmissionColor = Color.green;
    private float floor = 0.2f;
    private float ceiling = 0.7f;
    public bool IsASkinnedMesh;
    void Start()
    {
        Renderer renderer1 = GetComponent<Renderer>();
      
        material = renderer1.material;
        if (!IsASkinnedMesh)
        {
            mr = GetComponent<MeshRenderer>();
        }
        else
        {
            smr = GetComponent<SkinnedMeshRenderer>();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!IsASkinnedMesh)
        {
            if (mr.enabled == true)
            {
                colorFade();
            }
        }
        else
        {
            if (smr.enabled == true)
            {
                colorFade();
            }
        }

    }

    private void colorFade()
    {
        float emission = floor + Mathf.PingPong(Time.time * fadeSpeed, ceiling - floor);
        material.SetColor("_EmissionColor", EmmissionColor * Mathf.LinearToGammaSpace(emission));
    }

}
