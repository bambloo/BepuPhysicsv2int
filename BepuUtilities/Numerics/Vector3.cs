using System.Runtime.CompilerServices;

namespace BepuUtilities.Numerics
{
    public struct Vector3
    {
        public Number X;
        public Number Y;
        public Number Z;

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        public Vector3(Number x)
        {
            X = x; Y = x; Z = x;
        }

        public Vector3(in Vector2 a, Number z)
        {
            X = a.X; Y = a.Y; Z = z;
        }

        public Vector3(Number x, Number y, Number z)
        {
            X = x; Y = y; Z = z;
        }

        public static Vector3 operator +(in Vector3 left, in Vector3 right)
        {
            return new Vector3
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3 operator -(in Vector3 left, in Vector3 right)
        {
            return new Vector3
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
        }

        public static Vector3 operator -(in Vector3 value)
        {
            return new Vector3
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public static Vector3 operator *(Number left, in Vector3 right)
        {
            return new Vector3
            {
                X = left * right.X,
                Y = left * right.Y,
                Z = left * right.Z
            };
        }

        public static Vector3 operator *(in Vector3 right, Number left)
        {
            return new Vector3
            {
                X = left * right.X,
                Y = left * right.Y,
                Z = left * right.Z
            };
        }

        public static bool operator ==(in Vector3 left, in Vector3 right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(in Vector3 left, in Vector3 right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }

        public static Vector3 operator *(in Vector3 left, in Vector3 right)
        {
            return new Vector3
            {
                X = left.X * right.X,
                Y = left.Y * right.Y,
                Z = left.Z * right.Z
            };
        }

        public static Vector3 operator /(in Vector3 left, in Vector3 right)
        {
            return new Vector3
            {
                X = left.X / right.X,
                Y = left.Y / right.Y,
                Z = left.Z / right.Z
            };
        }

        public static Vector3 operator /(in Vector3 left, Number right)
        {
            return new Vector3
            {
                X = left.X / right,
                Y = left.Y / right,
                Z = left.Z / right
            };
        }

        public static Vector3 Min(in Vector3 v1, in Vector3 v2)
        {
            return new Vector3
            {
                X = v1.X < v2.X ? v1.X : v2.X,
                Y = v1.Y < v2.Y ? v1.Y : v2.Y,
                Z = v1.Z < v2.Z ? v1.Z : v2.Z
            };
        }

        public static Vector3 Max(in Vector3 v1, in Vector3 v2)
        {
            return new Vector3
            {
                X = v1.X > v2.X ? v1.X : v2.X,
                Y = v1.Y > v2.Y ? v1.Y : v2.Y,
                Z = v1.Z > v2.Z ? v1.Z : v2.Z
            };
        }

        public static Vector3 Abs(in Vector3 v)
        {
            return new Vector3
            {
                X = Number.Abs(v.X),
                Y = Number.Abs(v.Y),
                Z = Number.Abs(v.Z)
            };
        }

        public static Vector3 Cross(in Vector3 v1, in Vector3 v2)
        {
            return new Vector3
            {
                X = v1.Y * v2.Z - v1.Z * v2.Y,
                Y = v1.Z * v2.X - v1.X * v2.Z,
                Z = v1.X * v2.Y - v1.Y * v2.X
            };
        }

        public static Vector3 SquareRoot(in Vector3 v)
        {
            return new Vector3
            {
                X = Number.Sqrt(v.X),
                Y = Number.Sqrt(v.Y),
                Z = Number.Sqrt(v.Z),
            };
        }

        public static Number Dot(in Vector3 v1, in Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public Number LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public Number Length()
        {
            return Number.Sqrt(LengthSquared());
        }

        public static Vector3 Normalize(in Vector3 value)
        {
            return value / value.Length();
        }

        public static Number Distance(in Vector3 a, in Vector3 b)
        {
            return (a - b).Length();
        }

        /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Transform(Vector3 value, Quaternion rotation)
        {
            Number x2 = rotation.X + rotation.X;
            Number y2 = rotation.Y + rotation.Y;
            Number z2 = rotation.Z + rotation.Z;

            Number wx2 = rotation.W * x2;
            Number wy2 = rotation.W * y2;
            Number wz2 = rotation.W * z2;
            Number xx2 = rotation.X * x2;
            Number xy2 = rotation.X * y2;
            Number xz2 = rotation.X * z2;
            Number yy2 = rotation.Y * y2;
            Number yz2 = rotation.Y * z2;
            Number zz2 = rotation.Z * z2;

            return new Vector3(
                value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
                value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2) + value.Z * (yz2 - wx2),
                value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0f - xx2 - yy2)
            );
        }

        public static Number DistanceSquared(in Vector3 a, in Vector3 b)
        {
            return (a - b).LengthSquared();
        }

        public static implicit operator System.Numerics.Vector3(Vector3 value)
        {
            return new System.Numerics.Vector3((float)value.X, (float)value.Y, (float)value.Z);
        }

        public static implicit operator Vector3(System.Numerics.Vector3 value)
        {
            return new Vector3((float)value.X, (float)value.Y, (float)value.Z);
        }

        public override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }

    }
}
