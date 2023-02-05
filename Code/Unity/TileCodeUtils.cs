using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DotNetMath;


public class MapTileConsts
{
    public static double[] tileWidthPerLvl  = { 45.0, 5.0, 1.0, 0.1 };
    public static double[] tileHeightPerLvl = { 40.0, 5.0, 1.0, 0.1 };

    public static int[] numTilesHorizPerLvl = { 8, 9, 5, 10 };
    public static int[] numTilesVertPerLvl  = { 4, 8, 5, 10 };

    public static int[] meshSizePerLvl = { 20, 40, 60, 80 };

    public static int[] SeaTileHorizPerLvl = { 8, 72, 360, 3600 };
    public static int[] SeaTileVertPerLvl  = { 4, 36, 160, 1600 };

    public static double minLatDegs =  -80.0;
    public static double minLonDegs = -180.0;
    public static double maxLatDegs =   80.0;
    public static double maxLonDegs =  180.0;

    public static char[] letterLookup = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
}

// ================================================================================================

public struct MapTileCode
{
    public int mapLvlNum;
    public int lvl1X, lvl1Y;
    public int lvl2X, lvl2Y;
    public int lvl3X, lvl3Y;
    public int lvl4X, lvl4Y;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public void init()
    {
        mapLvlNum = 0;
        lvl1X = lvl1Y = 0;
        lvl2X = lvl2Y = 0;
        lvl3X = lvl3Y = 0;
        lvl4X = lvl4Y = 0;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public MapTileCode(MapTileCode prevTileCode)
    {
        mapLvlNum = prevTileCode.mapLvlNum;
        lvl1X = prevTileCode.lvl1X;
        lvl1Y = prevTileCode.lvl1Y;
        lvl2X = prevTileCode.lvl2X;
        lvl2Y = prevTileCode.lvl2Y;
        lvl3X = prevTileCode.lvl3X;
        lvl3Y = prevTileCode.lvl3Y;
        lvl4X = prevTileCode.lvl4X;
        lvl4Y = prevTileCode.lvl4Y;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public bool validate()
    {
        if (mapLvlNum > 4)
            return false;

        if (mapLvlNum >= 1)
        {
            if (lvl1X > MapTileConsts.numTilesHorizPerLvl[mapLvlNum - 1]) return false;
            if (lvl1Y > MapTileConsts.numTilesVertPerLvl[mapLvlNum - 1]) return false;
        }
        if (mapLvlNum >= 2)
        {
            if (lvl2X > MapTileConsts.numTilesHorizPerLvl[mapLvlNum - 1]) return false;
            if (lvl2Y > MapTileConsts.numTilesVertPerLvl[mapLvlNum - 1]) return false;
        }
        if (mapLvlNum >= 3)
        {
            if (lvl3X > MapTileConsts.numTilesHorizPerLvl[mapLvlNum - 1]) return false;
            if (lvl3Y > MapTileConsts.numTilesVertPerLvl[mapLvlNum - 1]) return false;
        }
        if (mapLvlNum >= 4)
        {
            if (lvl4X > MapTileConsts.numTilesHorizPerLvl[mapLvlNum - 1]) return false;
            if (lvl4Y > MapTileConsts.numTilesVertPerLvl[mapLvlNum - 1]) return false;
        }
        return true;
    }
}

// ================================================================================================

public class TileCodeUtils 
{
    public static string TileCodeString(int mapLvlNum, int row, int col)
    {
        string RetStr = "<Undefined>";

        if (mapLvlNum == 1) RetStr = string.Format("{0}{1}", MapTileConsts.letterLookup[row], col + 1);
        if (mapLvlNum == 2) RetStr = string.Format("{0}{1}", MapTileConsts.letterLookup[row], col + 1);
        if (mapLvlNum == 3) RetStr = string.Format("{0}{1:0}", MapTileConsts.letterLookup[row], col + 1);
        if (mapLvlNum == 4) RetStr = string.Format("{0}{1:00}", MapTileConsts.letterLookup[row], col + 1);

        return RetStr;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public static string TileCodeToString(MapTileCode inCode)
    {
        string retStr = "";
        if (inCode.mapLvlNum >= 1) retStr +=       TileCodeString(1, inCode.lvl1X, inCode.lvl1Y);
        if (inCode.mapLvlNum >= 2) retStr += "_" + TileCodeString(2, inCode.lvl2X, inCode.lvl2Y);
        if (inCode.mapLvlNum >= 3) retStr += "_" + TileCodeString(3, inCode.lvl3X, inCode.lvl3Y);
        if (inCode.mapLvlNum >= 4) retStr += "_" + TileCodeString(4, inCode.lvl4X, inCode.lvl4Y);
        return retStr;
    }
    
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static LatLonBox boundsForCode(MapTileCode inCode)
    {
        double minLat = 0.0;
        double minLon = 0.0;
        double maxLat = 0.0;
        double maxLon = 0.0;

        if (inCode.mapLvlNum >= 1)
        {
            minLat = MapTileConsts.minLatDegs + (MapTileConsts.tileWidthPerLvl[0] * inCode.lvl1Y);
            maxLat = minLat + MapTileConsts.tileWidthPerLvl[0];
            minLon = MapTileConsts.minLonDegs + (MapTileConsts.tileWidthPerLvl[0] * inCode.lvl1X);
            maxLon = minLon + MapTileConsts.tileWidthPerLvl[0];
        }
        if (inCode.mapLvlNum >= 2)
        {
            minLat = minLat + (MapTileConsts.tileHeightPerLvl[1] * inCode.lvl2Y);
            maxLat = minLat + MapTileConsts.tileHeightPerLvl[1];
            minLon = minLon + (MapTileConsts.tileWidthPerLvl[1] * inCode.lvl2X);
            maxLon = minLon + MapTileConsts.tileWidthPerLvl[1];
        }
        if (inCode.mapLvlNum >= 3)
        {
            minLat = minLat + (MapTileConsts.tileHeightPerLvl[2] * inCode.lvl3Y);
            maxLat = minLat + MapTileConsts.tileHeightPerLvl[2];
            minLon = minLon + (MapTileConsts.tileWidthPerLvl[2] * inCode.lvl3X);
            maxLon = minLon + MapTileConsts.tileWidthPerLvl[2];
        }
        if (inCode.mapLvlNum >= 4)
        {
            minLat = minLat + (MapTileConsts.tileHeightPerLvl[3] * inCode.lvl4Y);
            maxLat = minLat + MapTileConsts.tileHeightPerLvl[3];
            minLon = minLon + (MapTileConsts.tileWidthPerLvl[3] * inCode.lvl4X);
            maxLon = minLon + MapTileConsts.tileWidthPerLvl[3];
        }

        minLat = minLat * MathUtils.DegsToRadsMultiplier;
        minLon = minLon * MathUtils.DegsToRadsMultiplier;
        maxLat = maxLat * MathUtils.DegsToRadsMultiplier;
        maxLon = maxLon * MathUtils.DegsToRadsMultiplier;

        return new LatLonBox(minLat, minLon, maxLat, maxLon);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static LLAPos centreLLAForCode(MapTileCode inCode)
    {
        double resLat = 0;
        double resLon = 0;

        if (inCode.mapLvlNum >= 1)
        {
            resLat = MapTileConsts.minLatDegs + (MapTileConsts.tileHeightPerLvl[0] * inCode.lvl1Y) + (MapTileConsts.tileHeightPerLvl[0] / 2.0);
            resLon = MapTileConsts.minLonDegs + (MapTileConsts.tileWidthPerLvl[0] * inCode.lvl1X) + (MapTileConsts.tileWidthPerLvl[0] / 2.0);
        }
        if (inCode.mapLvlNum >= 2)
        {
            resLat = resLat + (MapTileConsts.tileHeightPerLvl[1] * inCode.lvl2Y) + (MapTileConsts.tileHeightPerLvl[1] / 2.0);
            resLon = resLon + (MapTileConsts.tileWidthPerLvl[1] * inCode.lvl2X) + (MapTileConsts.tileWidthPerLvl[1] / 2.0);
        }
        if (inCode.mapLvlNum >= 3)
        {
            resLat = resLat + (MapTileConsts.tileHeightPerLvl[2] * inCode.lvl3Y) + (MapTileConsts.tileHeightPerLvl[2] / 2.0);
            resLon = resLon + (MapTileConsts.tileWidthPerLvl[2] * inCode.lvl3X) + (MapTileConsts.tileWidthPerLvl[2] / 2.0);
        }
        if (inCode.mapLvlNum >= 4)
        {
            resLat = resLat + (MapTileConsts.tileHeightPerLvl[3] * inCode.lvl4Y) + (MapTileConsts.tileHeightPerLvl[3] / 2.0);
            resLon = resLon + (MapTileConsts.tileWidthPerLvl[3] * inCode.lvl4X) + (MapTileConsts.tileWidthPerLvl[3] / 2.0);
        }

        resLat = resLat * MathUtils.DegsToRadsMultiplier;
        resLon = resLon * MathUtils.DegsToRadsMultiplier;
        
        return new LLAPos(resLat, resLon, 0);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static MapTileCode codeFromLatLon(int inMapLvl, double inLat, double inLon)
    {
        MapTileCode retCode = new MapTileCode();

        if (inMapLvl > 4) inMapLvl = 4;
        retCode.mapLvlNum = inMapLvl;

        double lvl1Lat = 0.0;
        double lvl1Lon = 0.0;
        double lvl2Lat = 0.0;
        double lvl2Lon = 0.0;
        double lvl3Lat = 0.0;
        double lvl3Lon = 0.0;

        if (inMapLvl >= 1)
        {
            retCode.lvl1Y = (int)((inLat - MapTileConsts.minLatDegs) / MapTileConsts.tileHeightPerLvl[0]);
            retCode.lvl1X = (int)((inLon - MapTileConsts.minLonDegs) / MapTileConsts.tileWidthPerLvl[0]);
        }
        if (inMapLvl >= 2)
        {
            lvl1Lat = MapTileConsts.minLatDegs + ((double)retCode.lvl1Y * MapTileConsts.tileHeightPerLvl[0]);
            lvl1Lon = MapTileConsts.minLonDegs + ((double)retCode.lvl1X * MapTileConsts.tileWidthPerLvl[0]);
            retCode.lvl2Y = (int)((inLat - lvl1Lat)  / MapTileConsts.tileHeightPerLvl[1]);
            retCode.lvl2X = (int)((inLon - lvl1Lon)  / MapTileConsts.tileWidthPerLvl[1]);
        }
        if (inMapLvl >= 3)
        {
            lvl2Lat = lvl1Lat + ((double)retCode.lvl2Y * MapTileConsts.tileHeightPerLvl[1]);
            lvl2Lon = lvl1Lon + ((double)retCode.lvl2X * MapTileConsts.tileWidthPerLvl[1]);
            retCode.lvl3Y = (int)((inLat - lvl2Lat) / MapTileConsts.tileHeightPerLvl[2]);
            retCode.lvl3X = (int)((inLon - lvl2Lon) / MapTileConsts.tileWidthPerLvl[2]);
        }
        if (inMapLvl >= 4)
        {
            lvl3Lat = lvl2Lat + ((double)retCode.lvl3Y * MapTileConsts.tileHeightPerLvl[2]);
            lvl3Lon = lvl2Lon + ((double)retCode.lvl3X * MapTileConsts.tileWidthPerLvl[2]);
            retCode.lvl4Y = (int)((inLat - lvl3Lat) / MapTileConsts.tileHeightPerLvl[3]);
            retCode.lvl4X = (int)((inLon - lvl3Lon) / MapTileConsts.tileWidthPerLvl[3]);
        }
        return retCode;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

}
