using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DotNetMath;

public class GlobeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTerrainFileFor5DegTile(MapTileCode tileCode)
    {
        int meshSize = MapTileConsts.meshSizePerLvl[tileCode.mapLvlNum];
        Int2DArray heightMap = new Int2DArray(meshSize, meshSize);

        LatLonBox llbox = TileCodeUtils.boundsForCode(tileCode);

    }


}
