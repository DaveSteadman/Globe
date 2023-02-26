using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DotNetMath;

public class GlobeManager : MonoBehaviour
{
    public GameObject RenderOrigin;
    public GameObject GlobeRoot;
    public GameObject FocusRoot;
    public GameObject MapDisplayRoot;
    private GameObject FocusRootCylinder;

    // Start is called before the first frame update
    void Start()
    {
        SetupPredefPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    void SetupPredefPoints()
    {
        bool showDebugMarkers = true;
        {
            RenderOrigin = new GameObject("RenderOrigin", typeof(MeshFilter), typeof(MeshRenderer));
            RenderOrigin.transform.parent = transform;

            GameObject RenderOriginMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            RenderOriginMarker.name = "RenderOriginMarker (Red)";
            RenderOriginMarker.transform.localScale = new Vector3(1f, 1f, 1f);
            RenderOriginMarker.transform.parent = RenderOrigin.transform;

            var RenderOriginMarkerRenderer = RenderOriginMarker.GetComponent<Renderer>();
            RenderOriginMarkerRenderer.material.SetColor("_Color", Color.red);
            RenderOriginMarkerRenderer.enabled = showDebugMarkers;
        }

        {
            GlobeRoot = new GameObject("GlobeRoot", typeof(MeshFilter), typeof(MeshRenderer));
            GlobeRoot.transform.parent = transform;

            GameObject GlobeRootMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GlobeRootMarker.name = "GlobeRootMarker (Blue)";
            GlobeRootMarker.transform.localScale = new Vector3(1f, 1f, 1f);
            GlobeRootMarker.transform.parent = GlobeRoot.transform;

            var GlobeRootMarkerRenderer = GlobeRootMarker.GetComponent<Renderer>();
            GlobeRootMarkerRenderer.material.SetColor("_Color", Color.blue);
            GlobeRootMarkerRenderer.enabled = showDebugMarkers;
        }

        {
            FocusRoot = new GameObject("FocusRoot", typeof(MeshFilter), typeof(MeshRenderer));
            FocusRoot.transform.parent = transform;

            GameObject FocusRootMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            FocusRootMarker.name = "FocusRootMarker (Yellow)";
            FocusRootMarker.transform.localScale = new Vector3(1f, 1f, 1f);
            FocusRootMarker.transform.parent = FocusRoot.transform;

            var FocusRootMarkerRenderer = FocusRootMarker.GetComponent<Renderer>();
            FocusRootMarkerRenderer.material.SetColor("_Color", Color.yellow);
            FocusRootMarkerRenderer.enabled = showDebugMarkers;
        }

        {
            MapDisplayRoot = new GameObject("MapDisplayRoot", typeof(MeshFilter), typeof(MeshRenderer));
            MapDisplayRoot.transform.parent = transform;

            GameObject MapDisplayRootMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            MapDisplayRootMarker.name = "MapDisplayRootMarker (Cyan)";
            MapDisplayRootMarker.transform.localScale = new Vector3(1f, 1f, 1f);
            MapDisplayRootMarker.transform.parent = MapDisplayRoot.transform;

            var MapDisplayRootMarkerRenderer = MapDisplayRootMarker.GetComponent<Renderer>();
            MapDisplayRootMarkerRenderer.material.SetColor("_Color", Color.cyan);
            MapDisplayRootMarkerRenderer.enabled = showDebugMarkers;
        }

        {
            FocusRootCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            FocusRootCylinder.name = "FocusRootCylinder";
            FocusRootCylinder.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            FocusRootCylinder.transform.position = new Vector3(0f, 0f, 0f);
            FocusRootCylinder.transform.parent = FocusRoot.transform;

            var FocusRootCylinderRenderer = FocusRootCylinder.GetComponent<Renderer>();
            FocusRootCylinderRenderer.material.SetColor("_Color", Color.white);
            FocusRootCylinderRenderer.enabled = showDebugMarkers;
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public void CreateTerrainFileFor5DegTile(MapTileCode tileCode)
    {
        int meshSize = MapTileConsts.meshSizePerLvl[tileCode.mapLvlNum];
        Int2DArray heightMap = new Int2DArray(meshSize, meshSize);

        LatLonBox llbox = TileCodeUtils.boundsForCode(tileCode);

    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 



}
