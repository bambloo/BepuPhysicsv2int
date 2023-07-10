using System.Runtime.CompilerServices;
using BepuUtilities.Utils;

namespace BepuUtilities.Numerics
{
    public struct Vector4
    {
        public Number X;
        public Number Y;
        public Number Z;
        public Number W;

        public static readonly Vector4 Zero = new Vector4(0);
        public static readonly Vector4 One = new Vector4(1);

        public Vector4(Number x)
        {
            X = x; Y = x; Z = x;W = x;
        }

        public Vector4(in Vector3 vec, Number w)
        {
            X = vec.X; Y = vec.Y; Z = vec.Z; W = w;
        }

        public Vector4(Number x, Number y, Number z, Number w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        public static Vector4 operator -(in Vector4 l, in Vector4 r)
        {
            return new Vector4
            {
                X = l.X - r.X,
                Y = l.Y - r.Y,
                Z = l.Z - r.Z,
                W = l.W - r.W,
            };
        }

        public static Vector4 operator *(in Vector4 l, in Vector4 r)
        {
            return new Vector4
            {
                X = l.X * r.X,
                Y = l.Y * r.Y,
                Z = l.Z * r.Z,
                W = l.W * r.W,
            };
        }

        public static Vector4 operator *(Number l, in Vector4 r)
        {
            return new Vector4
            {
                X = l * r.X,
                Y = l * r.Y,
                Z = l * r.Z,
                W = l * r.W,
            };
        }

        public static Vector4 operator *(in Vector4 r, Number l)
        {
            return new Vector4
            {
                X = l * r.X,
                Y = l * r.Y,
                Z = l * r.Z,
                W = l * r.W,
            };
        }

        public static Vector4 operator +(in Vector4 l, in Vector4 r)
        {
            return new Vector4
            {
                X = l.X + r.X,
                Y = l.Y + r.Y,
                Z = l.Z + r.Z,
                W = l.W + r.W,
            };
        }

        public static Vector4 operator /(in Vector4 l, Number r)
        {
            return new Vector4
            {
                X = l.X / r,
                Y = l.Y / r,
                Z = l.Z / r,
                W = l.W / r
            };
        }

        public static Vector4 operator /(in Vector4 l,  in Vector4 r)
        {
            return new Vector4
            {
                X = l.X / r.X,
                Y = l.Y / r.Y,
                Z = l.Z / r.Z,
                W = l.W / r.W
            };
        }

        public static Vector4 Max(in Vector4 l, in Vector4 r)
        {
            return new Vector4
            {
                X = Math.Max(l.X, r.X),
                Y = Math.Max(l.Y, r.Y),
                Z = Math.Max(l.Z, r.Z),
                W = Math.Max(l.W, r.W)
            };
        }

        public static Vector4 Min(in Vector4 l, in Vector4 r)
        {
            return new Vector4
            {
                X = Math.Min(l.X, r.X),
                Y = Math.Min(l.Y, r.Y),
                Z = Math.Min(l.Z, r.Z),
                W = Math.Min(l.W, r.W)
            };
        }

        public static Number Dot(in Vector4 l, in Vector4 r)
        {
            return l.X * r.X + l.Y * r.Y + l.Z * r.Z + l.W * r.W;
        }

        public Number Length()
        {
            return Number.Sqrt(LengthSquared());
        }

        public Number LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        public static Vector4 SquareRoot(in Vector4 vector)
        {
            return new Vector4
            {
                X = Number.Sqrt(vector.X),
                Y = Number.Sqrt(vector.Y),
                Z = Number.Sqrt(vector.Z),
                W = Number.Sqrt(vector.W),
            };
        }

        public Vector128<Number> AsVector128()
        {
            return Vector128.Create(X, Y, Z, W);
        }
    }
}
