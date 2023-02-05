using System.Collections;
using System.Collections.Generic;

namespace DotNetMath
{
    public class MathUtils
    {
        public const double RadsToDegsMultiplier = 180.0 / System.Math.PI;
        public const double DegsToRadsMultiplier = System.Math.PI / 180.0;

        public const double TwoPi = 2.0 * System.Math.PI;
        public const double HalfPi = 0.5 * System.Math.PI;
            
        // ========================================================================
        // Single Value checks and adjustments
        // ========================================================================

        public static double RandInRange(double fmin, double fmax)
        {

            System.Random RndGen = new System.Random();
            double frange = fmax - fmin;

            double val = (double)RndGen.NextDouble();

            return fmin + (frange * val);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static double Interpolate(double startVal, double endVal, double frac)
        {
            double diff = endVal - startVal;
            return startVal + (diff * frac);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static double LimitToRange(double val, double fmin, double fmax)
        {
            if (val < fmin) return fmin;
            if (val > fmax) return fmax;
            return val;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static double WrapToRange(double val, double fmin, double fmax)
        {
            double diff = fmax - fmin;

            double outval = val;
            while (val < fmin) outval += diff;
            while (val > fmax) outval -= diff;
            return outval;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static bool IsInRange(double val, double fmin, double fmax)
        {
            if (val < fmin) return false;
            if (val > fmax) return false;
            return true;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public static double ScaleVal(double inval, double inmin, double inmax, double outmin, double outmax)
        {
            inval = LimitToRange(inval, inmin, inmax);

            double indiff = inmax - inmin;
            double outdiff = outmax - outmin;

            double diffratio = outdiff / indiff;
            double diffoffset = outmin - inmin;

            double outval = ((inval - inmin) * diffratio) + outmin;
            outval = LimitToRange(outval, outmin, outmax);
            return outval;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    }
}
