using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DotNetMath;

public class UnityMathUtils
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public static Vector3 XYZPosToVector3(XYZPos xyzPos)
    {
        return new Vector3((float)xyzPos.XM, (float)xyzPos.YM, (float)xyzPos.ZM);
    }

    public static XYZPos Vector3ToXYZPos(Vector3 v3xyz)
    {
        return new XYZPos(v3xyz.x, v3xyz.y, v3xyz.z);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public static Vector3 LLAToXYZPos(LLAPos lla)
    {
        return UnityMathUtils.XYZPosToVector3(PosUtils.LlaToXyz(lla));
    }


    public static Vector3 LLAToXYZPos(double latDegs, double lonDegs, double altMeters)
    {
        LLAPos lla = new LLAPos(latDegs * MathUtils.DegsToRadsMultiplier, lonDegs * MathUtils.DegsToRadsMultiplier, altMeters + PosUtils.EarthRadiusM);
        return XYZPosToVector3(PosUtils.LlaToXyz(lla));
    }

}
