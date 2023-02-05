using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopGO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh h = GlobeGeometryOps.CreateMeshHoop(1.2f, 0.8f, 0.1f, 32);

        transform.position = new Vector3(0, 0, -10);


        GameObject GO1 = new GameObject("pF1", typeof(MeshFilter), typeof(MeshRenderer));
        
        MeshRenderer meshRenderer = GO1.GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color", Color.magenta);

        GO1.GetComponent<MeshFilter>().sharedMesh = h;
        
        GO1.transform.localPosition = new Vector3(0, 0, 0);
        GO1.transform.parent = transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
