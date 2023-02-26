using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetMath
{
    public struct LLAPos
    {
        // values stored in rads and earth radius to optimise trig
        public double LatRads { get; set; }
        public double LonRads { get; set; }
        public double RadiusM  { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public double LatDegs { 
            get { return LatRads * MathUtils.RadsToDegsMultiplier; } 
            set { LatRads = value * MathUtils.DegsToRadsMultiplier; } 
        } 
        public double LonDegs { 
            get { return LonRads * MathUtils.RadsToDegsMultiplier; } 
            set { LonRads = value * MathUtils.DegsToRadsMultiplier; } 
        }
        public double AltMslM { 
            get { return RadiusM - PosUtils.EarthRadiusM; } 
            set { RadiusM = value + PosUtils.EarthRadiusM; } 
        }
        public double AltMslKm { 
            get { return (RadiusM - PosUtils.EarthRadiusM)/1000.0; } 
            set { RadiusM = (value*1000.0) + PosUtils.EarthRadiusM; } 
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public LLAPos(double la, double lo, double ra)
        {
            this.LatRads = la;
            this.LonRads = lo;
            this.RadiusM  = ra;
        }

        public LLAPos(double la, double lo)
        {
            this.LatRads = la;
            this.LonRads = lo;
            this.RadiusM = PosUtils.EarthRadiusM;
        }

        public LLAPos(LLAPos lla)
        {
            this.LatRads = lla.LatRads;
            this.LonRads = lla.LonRads;
            this.RadiusM  = lla.RadiusM;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public double DistanceToM(LLAPos pos1)
        {
            return DistanceBetweenM(this, pos1);
        }
        public double DistanceToKm(LLAPos pos1)
        {
            return (DistanceBetweenM(this, pos1) / 1000.0);
        }

        // ========================================================================================
        // Static Routines
        // ========================================================================================

        public static LLAPos LlaPlusRangeBearing(LLAPos lla, double rangeM, double bearingDegs)
        {
            double lat = lla.LatRads;
            double lon = lla.LonRads;
            double radius = lla.RadiusM;

            double bearingRads = bearingDegs * MathUtils.DegsToRadsMultiplier;
            double cosBearing = Math.Cos(bearingRads);
            double sinBearing = Math.Sin(bearingRads);

            double cosLat = Math.Cos(lat);
            double sinLat = Math.Sin(lat);

            double cosRange = Math.Cos(rangeM / radius);
            double sinRange = Math.Sin(rangeM / radius);

            double lat2 = Math.Asin(sinLat * cosRange + cosLat * sinRange * cosBearing);
            double lon2 = lon + Math.Atan2(sinBearing * sinRange * cosLat, cosRange - sinLat * Math.Sin(lat2));

            return new LLAPos(lat2, lon2, radius);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static LLAPos InterpolateTowards(LLAPos lla1, LLAPos lla2, double fraction)
        {
            double lat1 = lla1.LatRads;
            double lon1 = lla1.LonRads;
            double radius1 = lla1.RadiusM;

            double lat2 = lla2.LatRads;
            double lon2 = lla2.LonRads;
            double radius2 = lla2.RadiusM;

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;
            double dRadius = radius2 - radius1;

            double lat3 = lat1 + dLat * fraction;
            double lon3 = lon1 + dLon * fraction;
            double radius3 = radius1 + dRadius * fraction;

            return new LLAPos(lat3, lon3, radius3);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static double DistanceBetweenM(LLAPos lla1, LLAPos lla2)
        {
            double lat1 = lla1.LatRads;
            double lon1 = lla1.LonRads;
            double radius1 = lla1.RadiusM;

            double lat2 = lla2.LatRads;
            double lon2 = lla2.LonRads;
            double radius2 = lla2.RadiusM;

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;
            double dRadius = radius2 - radius1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = radius1 * c;

            return d;
        }

    }
}