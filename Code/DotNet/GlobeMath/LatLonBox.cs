using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetMath
{
    public struct LatLonBox
    {
        // values stored in rads to optimise trig
        public double LatMinRads { get; set; }
        public double LatMaxRads { get; set; }
        public double LonMinRads { get; set; }
        public double LonMaxRads { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public double MinLatDegs
        {
            get { return LatMinRads * MathUtils.RadsToDegsMultiplier; }
            set { LatMinRads = value * MathUtils.DegsToRadsMultiplier; }
        }
        
        public double MaxLatDegs
        {
            get { return LatMaxRads * MathUtils.RadsToDegsMultiplier; }
            set { LatMaxRads = value * MathUtils.DegsToRadsMultiplier; }
        }

        public double MinLonDegs
        {
            get { return LonMinRads * MathUtils.RadsToDegsMultiplier; }
            set { LonMinRads = value * MathUtils.DegsToRadsMultiplier; }
        }
        
        public double MaxLonDegs
        {
            get { return LonMaxRads * MathUtils.RadsToDegsMultiplier; }
            set { LonMaxRads = value * MathUtils.DegsToRadsMultiplier; }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public LatLonBox(double latMin, double latMax, double lonMin, double lonMax)
        {
            this.LatMinRads = latMin;
            this.LatMaxRads = latMax;
            this.LonMinRads = lonMin;
            this.LonMaxRads = lonMax;
        }

        public LatLonBox(LatLonBox box)
        {
            this.LatMinRads = box.LatMinRads;
            this.LatMaxRads = box.LatMaxRads;
            this.LonMinRads = box.LonMinRads;
            this.LonMaxRads = box.LonMaxRads;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public double LatMidRads()
        {
            return (LatMinRads + LatMaxRads) / 2.0;
        }

        public double LonMidRads()
        {
            return (LonMinRads + LonMaxRads) / 2.0;
        }

        public LLAPos MidPoint()
        {
            return new LLAPos(LatMidRads(), LonMidRads());
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // rely on the min position to anchor the box position, the max value is an increment on that, even 
        // if it wraps across a date line.

        public void UnwrapAngles()
        {
            if (LonMaxRads < LonMinRads)
            {
                LonMaxRads += MathUtils.TwoPi;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Contains(LLAPos point)
        {
            return (point.LatRads >= LatMinRads && point.LatRads <= LatMaxRads &&
                    point.LonRads >= LonMinRads && point.LonRads <= LonMaxRads);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    }
}