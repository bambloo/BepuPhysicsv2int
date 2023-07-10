using System;

namespace BepuUtilities.Numerics
{
    public struct Vector2
    {
        public Number X;
        public Number Y;

        public static readonly Vector2 Zero = new Vector2(0);
        public static readonly Vector2 One = new Vector2(1);

        public Vector2(Number x)
        {
            X = x;
            Y = x;
        }

        public Vector2(Number x, Number y)
        {
            X = x;
            Y = y;
        }
        public static Vector2 operator +(in Vector2 left, in Vector2 right)
        {
            return new Vector2
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }


        public static Vector2 operator -(in Vector2 left, in Vector2 right)
        {
            return new Vector2
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }
        public static Vector2 operator -(in Vector2 left)
        {
            return new Vector2
            {
                X = -left.X,
                Y = -left.Y
            };
        }

        public static Vector2 operator *(in Vector2 left, Number right)
        {
            return new Vector2
            {
                X = left.X * right,
                Y = left.Y * right
            };
        }

        public static Vector2 operator *(Number right, in Vector2 left)
        {
            return new Vector2
            {
                X = left.X * right,
                Y = left.Y * right
            };
        }

        public static Vector2 operator *(in Vector2 left, in Vector2 right)
        {
            return new Vector2
            {
                X = left.X * right.X,
                Y = left.Y * right.Y
            };
        }

        public static Vector2 operator /(in Vector2 left, Number right)
        {
            return new Vector2
            {
                X = left.X / right,
                Y = left.Y / right
            };
        }

        public static Vector2 operator /(Number right, in Vector2 left)
        {
            return new Vector2
            {
                X = left.X / right,
                Y = left.Y / right
            };
        }

        public static bool operator ==(in Vector2 left, in Vector2 right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(in Vector2 left, in Vector2 right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public static Number Dot(in Vector2 left, in Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        public static Vector2 Normalize(in Vector2 vec)
        {
            return vec / vec.Length();
        }

        public static Vector2 Max(in Vector2 left, in Vector2 right)
        {
            return new Vector2
            {
                X = left.X > right.X ? left.X : right.X,
                Y = left.Y > right.Y ? left.Y : right.Y
            };
        }

        public static Vector2 Min(in Vector2 left, in Vector2 right)
        {
            return new Vector2
            {
                X = left.X < right.X ? left.X : right.X,
                Y = left.Y < right.Y ? left.Y : right.Y
            };
        }

        public static Number Distance(in Vector2 left, in Vector2 right)
        {
            return (left - right).Length();
        }

        public Number Length()
        {
            return Number.Sqrt(LengthSquared());
        }

        public Number LengthSquared()
        {
            return X * X + Y * Y;
        }

        public static Number DistanceSquared(Vector2 a, Vector2 b)
        {
            return (a - b).LengthSquared();
        }
    }
}
