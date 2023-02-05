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
}
