using System.Collections;
using System.Collections.Generic;

namespace DotNetMath
{
    public struct RPYAttitude
    {
        // values stored in Rads to optimise trig
        public double RollRads  { get; set; }
        public double PitchRads { get; set; }
        public double YawRads   { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public RPYAttitude(double roll, double pitch, double yaw)
        {
            this.RollRads  = roll;
            this.PitchRads = pitch;
            this.YawRads   = yaw;
        }

        public RPYAttitude(RPYAttitude rpy)
        {
            this.RollRads  = rpy.RollRads;
            this.PitchRads = rpy.PitchRads;
            this.YawRads   = rpy.YawRads;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public double RollDegs()
        {
            return RollRads * MathUtils.RadsToDegsMultiplier;
        }

        public double PitchDegs()
        {
            return PitchRads * MathUtils.RadsToDegsMultiplier;
        }

        public double YawDegs()
        {
            return YawRads * MathUtils.RadsToDegsMultiplier;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public void SetRollDegs(double roll)
        {
            this.RollRads = roll * MathUtils.DegsToRadsMultiplier;
        }

        public void SetPitchDegs(double pitch)
        {
            this.PitchRads = pitch * MathUtils.DegsToRadsMultiplier;
        }

        public void SetYawDegs(double yaw)
        {
            this.YawRads = yaw * MathUtils.DegsToRadsMultiplier;
        }

        public void SetRollPitchYawDegs(double roll, double pitch, double yaw)
        {
            this.RollRads  = roll  * MathUtils.DegsToRadsMultiplier;
            this.PitchRads = pitch * MathUtils.DegsToRadsMultiplier;
            this.YawRads   = yaw   * MathUtils.DegsToRadsMultiplier;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }
}