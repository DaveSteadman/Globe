using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetMath
{
    public struct XYZPos
    {
        // values stored in Metres
        public double XM { get; set; }
        public double YM { get; set; }
        public double ZM { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public XYZPos(double x, double y, double z)
        {
            this.XM = x;
            this.YM = y;
            this.ZM = z;
        }

        public XYZPos(XYZPos xyz)
        {
            this.XM = xyz.XM;
            this.YM = xyz.YM;
            this.ZM = xyz.ZM;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public double Magnitude()
        {
            return Math.Sqrt(XM * XM + YM * YM + ZM * ZM);
        }

        public void Offset(XYZPos xyz)
        {
            this.XM += xyz.XM;
            this.YM += xyz.YM;
            this.ZM += xyz.ZM;
        }
 
        public void Normalise()
        {
            double mag = Magnitude();
            this.XM /= mag;
            this.YM /= mag;
            this.ZM /= mag;
        }

        public XYZPos Diff(XYZPos xyz)
        {
            return new XYZPos(this.XM - xyz.XM, this.YM - xyz.YM, this.ZM - xyz.ZM);
        }

        public XYZPos Sum(XYZPos xyz)
        {
            return new XYZPos(this.XM + xyz.XM, this.YM + xyz.YM, this.ZM + xyz.ZM);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }
}