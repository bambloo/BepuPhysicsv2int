using BepuUtilities.Utils;

namespace BepuUtilities.Numerics
{
    public struct Quaternion
    {
        public Number X;
        public Number Y;
        public Number Z;
        public Number W;

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        public Quaternion(Number x, Number y, Number z, Number w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        public static bool operator ==(in Quaternion a, in Quaternion b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
        }

        public static bool operator !=(in Quaternion a, in Quaternion b)
        {
            return !(a == b);
        }

        public Number Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public Number LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>Creates a quaternion from a unit vector and an angle to rotate around the vector.</summary>
        /// <param name="axis">The unit vector to rotate around.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <returns>The newly created quaternion.</returns>
        /// <remarks><paramref name="axis" /> vector must be normalized before calling this method or the resulting <see cref="Quaternion" /> will be incorrect.</remarks>
        public static Quaternion CreateFromAxisAngle(Vector3 axis, Number angle)
        {
            Quaternion ans;

            Number halfAngle = angle * Constants.C0p5;
            Number s = MathF.Sin(halfAngle);
            Number c = MathF.Cos(halfAngle);

            ans.X = axis.X * s;
            ans.Y = axis.Y * s;
            ans.Z = axis.Z * s;
            ans.W = c;

            return ans;
        }

        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion
            {
                X = -q.X,
                Y = -q.Y,
                Z = -q.Z,
                W = q.W,
            };
        }

        /// <summary>Concatenates two quaternions.</summary>
        /// <param name="value1">The first quaternion rotation in the series.</param>
        /// <param name="value2">The second quaternion rotation in the series.</param>
        /// <returns>A new quaternion representing the concatenation of the <paramref name="value1" /> rotation followed by the <paramref name="value2" /> rotation.</returns>
        public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
        {
            Quaternion ans;

            // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
            // So that's why value2 goes q1 and value1 goes q2.
            Number q1x = value2.X;
            Number q1y = value2.Y;
            Number q1z = value2.Z;
            Number q1w = value2.W;

            Number q2x = value1.X;
            Number q2y = value1.Y;
            Number q2z = value1.Z;
            Number q2w = value1.W;

            // cross(av, bv)
            Number cx = q1y * q2z - q1z * q2y;
            Number cy = q1z * q2x - q1x * q2z;
            Number cz = q1x * q2y - q1y * q2x;

            Number dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        public static Quaternion CreateFromYawPitchRoll(Number yaw, Number pitch, Number roll)
        {
            Number sr, cr, sp, cp, sy, cy;

            Number halfRoll = roll * Constants.C0p5;
            sr = MathF.Sin(halfRoll);
            cr = MathF.Cos(halfRoll);

            Number halfPitch = pitch * Constants.C0p5;
            sp = MathF.Sin(halfPitch);
            cp = MathF.Cos(halfPitch);

            Number halfYaw = yaw * Constants.C0p5;
            sy = MathF.Sin(halfYaw);
            cy = MathF.Cos(halfYaw);

            Quaternion result;

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;

            return result;
        }

        public static implicit operator System.Numerics.Quaternion(in Quaternion q) {
            return new System.Numerics.Quaternion
            {
                X = (float)q.X,
                Y = (float)q.Y,
                Z = (float)q.Z,
                W = (float)q.W,
            };
        }

        public override string ToString()
        {
            return $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";
        }
    }
}
