using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetMath
{
    public class PosUtils
    {
        public static readonly double EarthRadiusM = 6371000.0;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static XYZPos LlaToXyz(LLAPos lla)
        {
            double cosLat = Math.Cos(lla.LatRads);
            double sinLat = Math.Sin(lla.LatRads);
            double cosLon = Math.Cos(lla.LonRads);
            double sinLon = Math.Sin(lla.LonRads);

            double x = lla.RadiusM * cosLat * cosLon;
            double y = lla.RadiusM * cosLat * sinLon;
            double z = lla.RadiusM * sinLat;

            return new XYZPos(x, y, z);
        }

        public static LLAPos XyzToLla(XYZPos xyz)
        {
            double p = Math.Sqrt(xyz.XM * xyz.XM + xyz.YM * xyz.YM);
            double lat = Math.Atan2(xyz.ZM, p);
            double lon = Math.Atan2(xyz.YM, xyz.XM);
            double radius = Math.Sqrt(xyz.XM * xyz.XM + xyz.YM * xyz.YM + xyz.ZM * xyz.ZM);

            return new LLAPos(lat, lon, radius);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    }
}
