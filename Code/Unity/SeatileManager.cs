using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DotNetMath;

public class SeaTileManager : MonoBehaviour
{
    public Int2DArray SeaTileList_5deg;
    public Int2DArray SeaTileList_1deg;
    public Int2DArray SeaTileList_0p1deg;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public SeaTileManager()
    {
        SeaTileList_5deg = new Int2DArray(MapTileConsts.SeaTileHorizPerLvl[1], MapTileConsts.SeaTileVertPerLvl[1]);
        SeaTileList_1deg = new Int2DArray(MapTileConsts.SeaTileHorizPerLvl[2], MapTileConsts.SeaTileVertPerLvl[2]);
        SeaTileList_0p1deg = new Int2DArray(MapTileConsts.SeaTileHorizPerLvl[3], MapTileConsts.SeaTileVertPerLvl[3]);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public void LoadSeaTileLists()
    {
        SeaTileList_5deg.GridFromBinary(FilePathManager.MapRootDir + FilePathManager.Filename_SeaTile_5deg_Binary);
        SeaTileList_1deg.GridFromBinary(FilePathManager.MapRootDir + FilePathManager.Filename_SeaTile_1deg_Binary);
        SeaTileList_0p1deg.GridFromBinary(FilePathManager.MapRootDir + FilePathManager.Filename_SeaTile_0p1deg_Binary);
    }

    public void SaveSeaTileLists()
    {
        SeaTileList_5deg.GridToBinary(FilePathManager.MapRootDir + FilePathManager.Filename_SeaTile_5deg_Binary);
        SeaTileList_1deg.GridToBinary(FilePathManager.MapRootDir + FilePathManager.Filename_SeaTile_1deg_Binary);
        SeaTileList_0p1deg.GridToBinary(FilePathManager.MapRootDir + FilePathManager.Filename_SeaTile_0p1deg_Binary);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public int SeaTileX(double inLon, int maplevel)
    {
        int checkX = (int)MathUtils.ScaleVal(inLon, -180.0, +180.0, 0, MapTileConsts.SeaTileHorizPerLvl[maplevel-1]);
        return checkX;
    }

    public int SeaTileY(double inLat, int maplevel)
    {
        int checkY = (int)MathUtils.ScaleVal(inLat, -80.0, +80.0, 0, MapTileConsts.SeaTileVertPerLvl[maplevel-1]);
        return checkY;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public bool IsSeaTile(LLAPos inPos, int maplevel)
    {
        int checkX = SeaTileX(inPos.LonDegs, maplevel);
        int checkY = SeaTileY(inPos.LatDegs, maplevel);

        int checkVal = 0;

        if (maplevel == 1)
        {
            checkVal = SeaTileList_5deg.QuickGetVal(checkX, checkY);
        }
        else if (maplevel == 2)
        {
            checkVal = SeaTileList_1deg.QuickGetVal(checkX, checkY);
        }
        else if (maplevel == 3)
        {
            checkVal = SeaTileList_0p1deg.QuickGetVal(checkX, checkY);
        }

        return (checkVal == 0);
    }

    public void SetSeaTile(LLAPos inPos, int maplevel, bool isSea)
    {
        int checkX = SeaTileX(inPos.LonDegs, maplevel);
        int checkY = SeaTileY(inPos.LatDegs, maplevel);

        if (maplevel == 1)
        {
            SeaTileList_5deg.QuickSetVal(checkX, checkY, (isSea ? 0 : 1));
        }
        else if (maplevel == 2)
        {
            SeaTileList_1deg.QuickSetVal(checkX, checkY, (isSea ? 0 : 1));
        }
        else if (maplevel == 3)
        {
            SeaTileList_0p1deg.QuickSetVal(checkX, checkY, (isSea ? 0 : 1));
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


}
