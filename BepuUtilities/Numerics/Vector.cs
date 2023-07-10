using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BepuUtilities.Numerics
{
    public class Vector
    {
        public static Vector<int> AndNot(in Vector<int> a, in Vector<int> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 & ~b.A1,
                A2 = a.A2 & ~b.A2,
                A3 = a.A3 & ~b.A3,
                A4 = a.A4 & ~b.A4,
                A5 = a.A5 & ~b.A5,
                A6 = a.A6 & ~b.A6,
                A7 = a.A7 & ~b.A7,
                A8 = a.A8 & ~b.A8,
            };
        }

        public static Vector<int> AsVectorInt32(in Vector<uint> a)
        {
            return new Vector<int>
            {
                A1 = (int)a.A1,
                A2 = (int)a.A2,
                A3 = (int)a.A3,
                A4 = (int)a.A4,
                A5 = (int)a.A5,
                A6 = (int)a.A6,
                A7 = (int)a.A7,
                A8 = (int)a.A8,
            };
        }

        public static Vector<uint> AsVectorUInt32(in Vector<int> a)
        {
            return new Vector<uint>
            {
                A1 = (uint)a.A1,
                A2 = (uint)a.A2,
                A3 = (uint)a.A3,
                A4 = (uint)a.A4,
                A5 = (uint)a.A5,
                A6 = (uint)a.A6,
                A7 = (uint)a.A7,
                A8 = (uint)a.A8,
            };
        }

        public static Vector<int> BitwiseAnd(in Vector<int> a, in Vector<int> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 & b.A1,
                A2 = a.A2 & b.A2,
                A3 = a.A3 & b.A3,
                A4 = a.A4 & b.A4,
                A5 = a.A5 & b.A5,
                A6 = a.A6 & b.A6,
                A7 = a.A7 & b.A7,
                A8 = a.A8 & b.A8,
            };
        }

        public static Vector<int> BitwiseOr(in Vector<int> a, in Vector<int> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 | b.A1,
                A2 = a.A2 | b.A2,
                A3 = a.A3 | b.A3,
                A4 = a.A4 | b.A4,
                A5 = a.A5 | b.A5,
                A6 = a.A6 | b.A6,
                A7 = a.A7 | b.A7,
                A8 = a.A8 | b.A8,
            };
        }

        public static Vector<Number> BitwiseOr(in Vector<Number> a, in Vector<Number> b)
        {
            return new Vector<Number>
            {
                A1 = a.A1 | b.A1,
                A2 = a.A2 | b.A2,
                A3 = a.A3 | b.A3,
                A4 = a.A4 | b.A4,
                A5 = a.A5 | b.A5,
                A6 = a.A6 | b.A6,
                A7 = a.A7 | b.A7,
                A8 = a.A8 | b.A8,
            };
        }

        public static int Dot(in Vector<int> a, in Vector<int> b)
        {
            return 
                a.A1 * b.A1 + a.A2 * b.A2 + a.A3 * b.A3 + a.A4 * b.A4 + 
                a.A5 * b.A5 + a.A6 + b.A6 + a.A7 * b.A7 + a.A8 * b.A8;
        }

        public static Vector<int> Equals(in Vector<Number> a, in Vector<Number> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 == b.A1 ? -1 : 0,
                A2 = a.A2 == b.A2 ? -1 : 0,
                A3 = a.A3 == b.A3 ? -1 : 0,
                A4 = a.A4 == b.A4 ? -1 : 0,
                A5 = a.A5 == b.A5 ? -1 : 0,
                A6 = a.A6 == b.A6 ? -1 : 0,
                A7 = a.A7 == b.A7 ? -1 : 0,
                A8 = a.A8 == b.A8 ? -1 : 0,
            };
        }

        public static Vector<int> Equals(in Vector<int> a, in Vector<int> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 == b.A1 ? -1 : 0,
                A2 = a.A2 == b.A2 ? -1 : 0,
                A3 = a.A3 == b.A3 ? -1 : 0,
                A4 = a.A4 == b.A4 ? -1 : 0,
                A5 = a.A5 == b.A5 ? -1 : 0,
                A6 = a.A6 == b.A6 ? -1 : 0,
                A7 = a.A7 == b.A7 ? -1 : 0,
                A8 = a.A8 == b.A8 ? -1 : 0,
            };
        }

        public static bool EqualsAll(in Vector<int> a, in Vector<int> b)
        {
            if (a.A1 != b.A1) return false;
            if (a.A2 != b.A2) return false;
            if (a.A3 != b.A3) return false;
            if (a.A4 != b.A4) return false;
            if (a.A5 != b.A5) return false;
            if (a.A6 != b.A6) return false;
            if (a.A7 != b.A7) return false;
            if (a.A8 != b.A8) return false;
            return true;
        }

        public static bool EqualsAny(in Vector<int> a, in Vector<int> b)
        {
            if (a.A1 == b.A1) return true;
            if (a.A2 == b.A2) return true;
            if (a.A3 == b.A3) return true;
            if (a.A4 == b.A4) return true;
            if (a.A5 == b.A5) return true;
            if (a.A6 == b.A6) return true;
            if (a.A7 == b.A7) return true;
            if (a.A8 == b.A8) return true;
            return false;
        }

        public static Vector<int> GreaterThan(in Vector<Number> left, in Vector<Number> right)
        {
            return new Vector<int>
            {
                A1 = left.A1 > right.A1 ? -1 : 0,
                A2 = left.A2 > right.A2 ? -1 : 0,
                A3 = left.A3 > right.A3 ? -1 : 0,
                A4 = left.A4 > right.A4 ? -1 : 0,
                A5 = left.A5 > right.A5 ? -1 : 0,
                A6 = left.A6 > right.A6 ? -1 : 0,
                A7 = left.A7 > right.A7 ? -1 : 0,
                A8 = left.A8 > right.A8 ? -1 : 0
            };
        }

        public static Vector<int> GreaterThan(in Vector<int> left, in Vector<int> right)
        {
            return new Vector<int>
            {
                A1 = left.A1 > right.A1 ? -1 : 0,
                A2 = left.A2 > right.A2 ? -1 : 0,
                A3 = left.A3 > right.A3 ? -1 : 0,
                A4 = left.A4 > right.A4 ? -1 : 0,
                A5 = left.A5 > right.A5 ? -1 : 0,
                A6 = left.A6 > right.A6 ? -1 : 0,
                A7 = left.A7 > right.A7 ? -1 : 0,
                A8 = left.A8 > right.A8 ? -1 : 0
            };
        }

        public static bool GreaterThanAny(in Vector<Number> a, in Vector<Number> b)
        {
            if (a.A1 > b.A1) return true;
            if (a.A2 > b.A2) return true;
            if (a.A3 > b.A3) return true;
            if (a.A4 > b.A4) return true;
            if (a.A5 > b.A5) return true;
            if (a.A6 > b.A6) return true;
            if (a.A7 > b.A7) return true;
            if (a.A8 > b.A8) return true;
            return false;
        }

        public static Vector<int> GreaterThanOrEqual(in Vector<int> a, in Vector<int> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 >= b.A1 ? -1 : 0,
                A2 = a.A2 >= b.A2 ? -1 : 0,
                A3 = a.A3 >= b.A3 ? -1 : 0,
                A4 = a.A4 >= b.A4 ? -1 : 0,
                A5 = a.A5 >= b.A5 ? -1 : 0,
                A6 = a.A6 >= b.A6 ? -1 : 0,
                A7 = a.A7 >= b.A7 ? -1 : 0,
                A8 = a.A8 >= b.A8 ? -1 : 0,
            };
        }

        public static Vector<int> GreaterThanOrEqual(in Vector<Number> a, in Vector<Number> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 >= b.A1 ? -1 : 0,
                A2 = a.A2 >= b.A2 ? -1 : 0,
                A3 = a.A3 >= b.A3 ? -1 : 0,
                A4 = a.A4 >= b.A4 ? -1 : 0,
                A5 = a.A5 >= b.A5 ? -1 : 0,
                A6 = a.A6 >= b.A6 ? -1 : 0,
                A7 = a.A7 >= b.A7 ? -1 : 0,
                A8 = a.A8 >= b.A8 ? -1 : 0,
            };
        }

        public static bool LessThanAny(in Vector<int> a, in Vector<int> b)
        {
            if (a.A1 < b.A1) return true;
            if (a.A2 < b.A2) return true;
            if (a.A3 < b.A3) return true;
            if (a.A4 < b.A4) return true;
            if (a.A5 < b.A5) return true;
            if (a.A6 < b.A6) return true;
            if (a.A7 < b.A7) return true;
            if (a.A8 < b.A8) return true;
            return false;
        }

        public static bool LessThanAny(in Vector<Number> a, in Vector<Number> b)
        {
            if (a.A1 < b.A1) return true;
            if (a.A2 < b.A2) return true;
            if (a.A3 < b.A3) return true;
            if (a.A4 < b.A4) return true;
            if (a.A5 < b.A5) return true;
            if (a.A6 < b.A6) return true;
            if (a.A7 < b.A7) return true;
            if (a.A8 < b.A8) return true;
            return false;
        }

        public static bool LessThanAll(in Vector<int> a, in Vector<int> b)
        {
            if (a.A1 >= b.A1) return false;
            if (a.A2 >= b.A2) return false;
            if (a.A3 >= b.A3) return false;
            if (a.A4 >= b.A4) return false;
            if (a.A5 >= b.A5) return false;
            if (a.A6 >= b.A6) return false;
            if (a.A7 >= b.A7) return false;
            if (a.A8 >= b.A8) return false;
            return true;
        }

        public static Vector<int> LessThanOrEqual(in Vector<Number> a, in Vector<Number> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 <= b.A1 ? -1 : 0,
                A2 = a.A2 <= b.A2 ? -1 : 0,
                A3 = a.A3 <= b.A3 ? -1 : 0,
                A4 = a.A4 <= b.A4 ? -1 : 0,
                A5 = a.A5 <= b.A5 ? -1 : 0,
                A6 = a.A6 <= b.A6 ? -1 : 0,
                A7 = a.A7 <= b.A7 ? -1 : 0,
                A8 = a.A8 <= b.A8 ? -1 : 0,
            };
        }

        public static Vector<int> LessThanOrEqual(in Vector<int> a, in Vector<int> b)
        {
            return new Vector<int>
            {
                A1 = a.A1 <= b.A1 ? -1 : 0,
                A2 = a.A2 <= b.A2 ? -1 : 0,
                A3 = a.A3 <= b.A3 ? -1 : 0,
                A4 = a.A4 <= b.A4 ? -1 : 0,
                A5 = a.A5 <= b.A5 ? -1 : 0,
                A6 = a.A6 <= b.A6 ? -1 : 0,
                A7 = a.A7 <= b.A7 ? -1 : 0,
                A8 = a.A8 <= b.A8 ? -1 : 0,
            };
        }

        public static Vector<T> Max<T>(in Vector<T> left, in Vector<T> right)
            where T : struct
        {
            return new Vector<T>
            {
                A1 = Scalar<T>.GreaterThan(left.A1, right.A1) ? left.A1 : right.A1,
                A2 = Scalar<T>.GreaterThan(left.A2, right.A2) ? left.A2 : right.A2,
                A3 = Scalar<T>.GreaterThan(left.A3, right.A3) ? left.A3 : right.A3,
                A4 = Scalar<T>.GreaterThan(left.A4, right.A4) ? left.A4 : right.A4,
                A5 = Scalar<T>.GreaterThan(left.A5, right.A5) ? left.A5 : right.A5,
                A6 = Scalar<T>.GreaterThan(left.A6, right.A6) ? left.A6 : right.A6,
                A7 = Scalar<T>.GreaterThan(left.A7, right.A7) ? left.A7 : right.A7,
                A8 = Scalar<T>.GreaterThan(left.A8, right.A8) ? left.A8 : right.A8
            };
        }

        public static Vector<T> Min<T>(in Vector<T> left, in Vector<T> right)
            where T : struct
        {
            return new Vector<T>
            {
                A1 = Scalar<T>.LessThan(left.A1, right.A1) ? left.A1 : right.A1,
                A2 = Scalar<T>.LessThan(left.A2, right.A2) ? left.A2 : right.A2,
                A3 = Scalar<T>.LessThan(left.A3, right.A3) ? left.A3 : right.A3,
                A4 = Scalar<T>.LessThan(left.A4, right.A4) ? left.A4 : right.A4,
                A5 = Scalar<T>.LessThan(left.A5, right.A5) ? left.A5 : right.A5,
                A6 = Scalar<T>.LessThan(left.A6, right.A6) ? left.A6 : right.A6,
                A7 = Scalar<T>.LessThan(left.A7, right.A7) ? left.A7 : right.A7,
                A8 = Scalar<T>.LessThan(left.A8, right.A8) ? left.A8 : right.A8
            };
        }

        public static Vector<int> OnesComplement(in Vector<int> vec)
        {
            return new Vector<int>
            {
                A1 = ~vec.A1,
                A2 = ~vec.A2,
                A3 = ~vec.A3,
                A4 = ~vec.A4,
                A5 = ~vec.A5,
                A6 = ~vec.A6,
                A7 = ~vec.A7,
                A8 = ~vec.A8,
            };
        }

        public static Vector<T> Abs<T>(in Vector<T> left)
            where T : struct
        {
            return new Vector<T>
            {
                A1 = Scalar<T>.Abs(left.A1),
                A2 = Scalar<T>.Abs(left.A2),
                A3 = Scalar<T>.Abs(left.A3),
                A4 = Scalar<T>.Abs(left.A4),
                A5 = Scalar<T>.Abs(left.A5),
                A6 = Scalar<T>.Abs(left.A6),
                A7 = Scalar<T>.Abs(left.A7),
                A8 = Scalar<T>.Abs(left.A8)
            };
        }

        public static Vector<T> ConditionalSelect<T>(in Vector<int> condition, in Vector<T> a, in Vector<T> b) where T : struct
        {
            return new Vector<T>
            {
                A1 = condition.A1 == 0 ? b.A1 : a.A1,
                A2 = condition.A2 == 0 ? b.A2 : a.A2,
                A3 = condition.A3 == 0 ? b.A3 : a.A3,
                A4 = condition.A4 == 0 ? b.A4 : a.A4,
                A5 = condition.A5 == 0 ? b.A5 : a.A5,
                A6 = condition.A6 == 0 ? b.A6 : a.A6,
                A7 = condition.A7 == 0 ? b.A7 : a.A7,
                A8 = condition.A8 == 0 ? b.A8 : a.A8
            };
        }

        public static Vector<int> LessThan(in Vector<Number> left, in Vector<Number> right)
        {
            return new Vector<int>
            {
                A1 = left.A1 < right.A1 ? -1 : 0,
                A2 = left.A2 < right.A2 ? -1 : 0,
                A3 = left.A3 < right.A3 ? -1 : 0,
                A4 = left.A4 < right.A4 ? -1 : 0,
                A5 = left.A5 < right.A5 ? -1 : 0,
                A6 = left.A6 < right.A6 ? -1 : 0,
                A7 = left.A7 < right.A7 ? -1 : 0,
                A8 = left.A8 < right.A8 ? -1 : 0
            };
        }

        public static Vector<uint> LessThan(in Vector<uint> left, in Vector<uint> right)
        {
            return new Vector<uint>
            {
                A1 = left.A1 < right.A1 ? 0xffffffff : 0,
                A2 = left.A2 < right.A2 ? 0xffffffff : 0,
                A3 = left.A3 < right.A3 ? 0xffffffff : 0,
                A4 = left.A4 < right.A4 ? 0xffffffff : 0,
                A5 = left.A5 < right.A5 ? 0xffffffff : 0,
                A6 = left.A6 < right.A6 ? 0xffffffff : 0,
                A7 = left.A7 < right.A7 ? 0xffffffff : 0,
                A8 = left.A8 < right.A8 ? 0xffffffff : 0
            };
        }

        public static Vector<int> LessThan(in Vector<int> left, in Vector<int> right)
        {
            return new Vector<int>
            {
                A1 = left.A1 < right.A1 ? -1 : 0,
                A2 = left.A2 < right.A2 ? -1 : 0,
                A3 = left.A3 < right.A3 ? -1 : 0,
                A4 = left.A4 < right.A4 ? -1 : 0,
                A5 = left.A5 < right.A5 ? -1 : 0,
                A6 = left.A6 < right.A6 ? -1 : 0,
                A7 = left.A7 < right.A7 ? -1 : 0,
                A8 = left.A8 < right.A8 ? -1 : 0
            };
        }

        public static Vector<Number> SquareRoot(in Vector<Number> a)
        {
            return new Vector<Number>
            {
                A1 = Number.Sqrt(a.A1),
                A2 = Number.Sqrt(a.A2),
                A3 = Number.Sqrt(a.A3),
                A4 = Number.Sqrt(a.A4),
                A5 = Number.Sqrt(a.A5),
                A6 = Number.Sqrt(a.A6),
                A7 = Number.Sqrt(a.A7),
                A8 = Number.Sqrt(a.A8)
            };
        }

        public static Vector<Number> Floor(in Vector<Number> a)
        {
            return new Vector<Number>
            {
                A1 = Number.Floor(a.A1),
                A2 = Number.Floor(a.A2),
                A3 = Number.Floor(a.A3),
                A4 = Number.Floor(a.A4),
                A5 = Number.Floor(a.A5),
                A6 = Number.Floor(a.A6),
                A7 = Number.Floor(a.A7),
                A8 = Number.Floor(a.A8)
            };
        }

        public static Vector<int> Xor(in Vector<int> left, in Vector<int> right)
        {
            return new Vector<int>
            {
                A1 = left.A1 ^ right.A1,
                A2 = left.A2 ^ right.A2,
                A3 = left.A3 ^ right.A3,
                A4 = left.A4 ^ right.A4,
                A5 = left.A5 ^ right.A5,
                A6 = left.A6 ^ right.A6,
                A7 = left.A7 ^ right.A7,
                A8 = left.A8 ^ right.A8
            };
        }
    }

    public struct Vector<T> where T : struct
    {
        public const int Count = 8;

        public T A1;
        public T A2;
        public T A3;
        public T A4;
        public T A5;
        public T A6;
        public T A7;
        public T A8;

        public Vector()
        {

        }

        public Vector(Span<T> span)
        {
            A1 = span[0]; A2 = span[1]; A3 = span[2]; A4 = span[3];
            A5 = span[4]; A6 = span[5]; A7 = span[6]; A8 = span[7];
        }

        public Vector(T val)
        {
            A1 = val;
            A2 = val;
            A3 = val;
            A4 = val;
            A5 = val;
            A6 = val;
            A7 = val;
            A8 = val;
        }

        public static Vector<T> operator+(in Vector<T> left, in Vector<T> right)
        {
            var res = new Vector<T>();
            res.A1 = Scalar<T>.Add(left.A1, right.A1);
            res.A2 = Scalar<T>.Add(left.A2, right.A2);
            res.A3 = Scalar<T>.Add(left.A3, right.A3);
            res.A4 = Scalar<T>.Add(left.A4, right.A4);
            res.A5 = Scalar<T>.Add(left.A5, right.A5);
            res.A6 = Scalar<T>.Add(left.A6, right.A6);
            res.A7 = Scalar<T>.Add(left.A7, right.A7);
            res.A8 = Scalar<T>.Add(left.A8, right.A8);
            return res;
        }

        public static Vector<T> operator *(in Vector<T> left, in Vector<T> right)
        {
            var res = new Vector<T>();
            res.A1 = Scalar<T>.Multiply(left.A1, right.A1);
            res.A2 = Scalar<T>.Multiply(left.A2, right.A2);
            res.A3 = Scalar<T>.Multiply(left.A3, right.A3);
            res.A4 = Scalar<T>.Multiply(left.A4, right.A4);
            res.A5 = Scalar<T>.Multiply(left.A5, right.A5);
            res.A6 = Scalar<T>.Multiply(left.A6, right.A6);
            res.A7 = Scalar<T>.Multiply(left.A7, right.A7);
            res.A8 = Scalar<T>.Multiply(left.A8, right.A8);
            return res;
        }
        public static Vector<T> operator *(T left, in Vector<T> right)
        {
            var res = new Vector<T>();
            res.A1 = Scalar<T>.Multiply(left, right.A1);
            res.A2 = Scalar<T>.Multiply(left, right.A2);
            res.A3 = Scalar<T>.Multiply(left, right.A3);
            res.A4 = Scalar<T>.Multiply(left, right.A4);
            res.A5 = Scalar<T>.Multiply(left, right.A5);
            res.A6 = Scalar<T>.Multiply(left, right.A6);
            res.A7 = Scalar<T>.Multiply(left, right.A7);
            res.A8 = Scalar<T>.Multiply(left, right.A8);
            return res;
        }

        public static Vector<T> operator *(in Vector<T> left, T right)
        {
            return right * left;
        }


        public static Vector<T> operator /(in Vector<T> left, in Vector<T> right)
        {
            var res = new Vector<T>();
            res.A1 = Scalar<T>.Divide(left.A1, right.A1);
            res.A2 = Scalar<T>.Divide(left.A2, right.A2);
            res.A3 = Scalar<T>.Divide(left.A3, right.A3);
            res.A4 = Scalar<T>.Divide(left.A4, right.A4);
            res.A5 = Scalar<T>.Divide(left.A5, right.A5);
            res.A6 = Scalar<T>.Divide(left.A6, right.A6);
            res.A7 = Scalar<T>.Divide(left.A7, right.A7);
            res.A8 = Scalar<T>.Divide(left.A8, right.A8);
            return res;
        }

        //public static Vector<T> operator +(Vector<T> left, Vector<T> right)
        //{
        //    var res = new Vector<T>();
        //    res.A1 = left.A1 + right.A1;
        //    res.A2 = left.A2 + right.A2;
        //    res.A3 = left.A3 + right.A3;
        //    res.A4 = left.A4 + right.A4;
        //    res.A5 = left.A5 + right.A5;
        //    res.A6 = left.A6 + right.A6;
        //    res.A7 = left.A7 + right.A7;
        //    res.A8 = left.A8 + right.A8;
        //    return res;
        //}

        public static Vector<T> operator -(in Vector<T> left, in Vector<T> right)
        {
            var res = new Vector<T>();
            res.A1 = Scalar<T>.Subtract(left.A1, right.A1);
            res.A2 = Scalar<T>.Subtract(left.A2, right.A2);
            res.A3 = Scalar<T>.Subtract(left.A3, right.A3);
            res.A4 = Scalar<T>.Subtract(left.A4, right.A4);
            res.A5 = Scalar<T>.Subtract(left.A5, right.A5);
            res.A6 = Scalar<T>.Subtract(left.A6, right.A6);
            res.A7 = Scalar<T>.Subtract(left.A7, right.A7);
            res.A8 = Scalar<T>.Subtract(left.A8, right.A8);
            return res;
        }

        public static Vector<T> operator -(in Vector<T> value)
        {
            var res = new Vector<T>();
            res.A1 = Scalar<T>.Negate(value.A1);
            res.A2 = Scalar<T>.Negate(value.A2);
            res.A3 = Scalar<T>.Negate(value.A3);
            res.A4 = Scalar<T>.Negate(value.A4);
            res.A5 = Scalar<T>.Negate(value.A5);
            res.A6 = Scalar<T>.Negate(value.A6);
            res.A7 = Scalar<T>.Negate(value.A7);
            res.A8 = Scalar<T>.Negate(value.A8);
            return res;
        }

        public static bool operator ==(in Vector<T> left, in Vector<T> right)
        {
            if (!Scalar<T>.Equals(left.A1, right.A1)) return false;
            if (!Scalar<T>.Equals(left.A2, right.A2)) return false;
            if (!Scalar<T>.Equals(left.A3, right.A3)) return false;
            if (!Scalar<T>.Equals(left.A4, right.A4)) return false;
            if (!Scalar<T>.Equals(left.A5, right.A5)) return false;
            if (!Scalar<T>.Equals(left.A6, right.A6)) return false;
            if (!Scalar<T>.Equals(left.A7, right.A7)) return false;
            if (!Scalar<T>.Equals(left.A8, right.A8)) return false;
            return true;
        }

        public static bool operator !=(in Vector<T> left, in Vector<T> right)
        {
            return !(left == right);
        }
        //public static Vector<T> operator &(Vector<T> left, Vector<T> right)
        //{
        //    var res = new Vector<T>();
        //    res.A1 = left.A1 & right.A1;
        //    res.A2 = left.A2 & right.A2;
        //    res.A3 = left.A3 & right.A3;
        //    res.A4 = left.A4 & right.A4;
        //    res.A5 = left.A5 & right.A5;
        //    res.A6 = left.A6 & right.A6;
        //    res.A7 = left.A7 & right.A7;
        //    res.A8 = left.A8 & right.A8;
        //    return res;
        //}

        //public static Vector<T> operator |(Vector<T> left, Vector<T> right)
        //{
        //    var res = new Vector<T>();
        //    res.A1 = left.A1 | right.A1;
        //    res.A2 = left.A2 | right.A2;
        //    res.A3 = left.A3 | right.A3;
        //    res.A4 = left.A4 | right.A4;
        //    res.A5 = left.A5 | right.A5;
        //    res.A6 = left.A6 | right.A6;
        //    res.A7 = left.A7 | right.A7;
        //    res.A8 = left.A8 | right.A8;
        //    return res;
        //}

        //public static Vector<T> operator ^(Vector<T> left, Vector<T> right)
        //{
        //    var res = new Vector<T>();
        //    res.A1 = left.A1 ^ right.A1;
        //    res.A2 = left.A2 ^ right.A2;
        //    res.A3 = left.A3 ^ right.A3;
        //    res.A4 = left.A4 ^ right.A4;
        //    res.A5 = left.A5 ^ right.A5;
        //    res.A6 = left.A6 ^ right.A6;
        //    res.A7 = left.A7 ^ right.A7;
        //    res.A8 = left.A8 ^ right.A8;
        //    return res;
        //}

        //public static Vector<T> operator ~(Vector<T> value)
        //{
        //    var res = new Vector<T>();
        //    res.A1 = ~value.A1;
        //    res.A2 = ~value.A2;
        //    res.A3 = ~value.A3;
        //    res.A4 = ~value.A4;
        //    res.A5 = ~value.A5;
        //    res.A6 = ~value.A6;
        //    res.A7 = ~value.A7;
        //    res.A8 = ~value.A8;
        //    return res;
        //}

        //public static Vector<T> operator /(Vector<T> left, Vector<T> right)
        //{
        //    var res = new Vector<T>();
        //    res.A1 = left.A1 / right.A1;
        //    res.A2 = left.A2 / right.A2;
        //    res.A3 = left.A3 / right.A3;
        //    res.A4 = left.A4 / right.A4;
        //    res.A5 = left.A5 / right.A5;
        //    res.A6 = left.A6 / right.A6;
        //    res.A7 = left.A7 / right.A7;
        //    res.A8 = left.A8 / right.A8;
        //    return res;
        //}

        public override string ToString()
        {
            return $"<{A1}, {A2}, {A3}, {A4}, {A5}, {A6}, {A7}, {A8}>";
        }

        public unsafe ref T this[int index]
        {
            get
            {
                return ref Unsafe.Add(ref A1, index);
            }
        }

        public static readonly Vector<T> One = new Vector<T>(Scalar<T>.One);
        public static readonly Vector<T> Zero = new Vector<T>(Scalar<T>.Zero);

        //public static readonly Vector<T> One = new Vector<T>(T.One);
        //public static readonly Vector<T> Zero = new Vector<T>(T.Zero);
    }
}
