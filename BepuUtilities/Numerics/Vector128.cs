using System.Runtime.CompilerServices;

namespace BepuUtilities.Numerics
{
    public class Vector128
    {
        public static Vector128<T> ConditionalSelect<T>(Vector128<int> axisIsDegenerate, Vector128<T> left, Vector128<T> right) where T : struct
        {
            return new Vector128<T>
            {
                A1 = axisIsDegenerate.A1 != 0 ? left.A1 : right.A1,
                A2 = axisIsDegenerate.A2 != 0 ? left.A2 : right.A2,
                A3 = axisIsDegenerate.A3 != 0 ? left.A3 : right.A3,
                A4 = axisIsDegenerate.A4 != 0 ? left.A4 : right.A4
            };
        }

        public static Vector128<T> Create<T>(T value) where T: struct
        {
            return new Vector128<T>(value);
        }

        public static Vector128<T> Create<T>(T x, T y, T z, T w) where T : struct
        {
            return new Vector128<T>(x, y, z, w);
        }

        public static int ExtractMostSignificantBits(Vector128<int> axisIsDegenerate)
        {
            int res = 0;
            if (axisIsDegenerate.A1 < 0) res |= 1;
            if (axisIsDegenerate.A2 < 0) res |= 2;
            if (axisIsDegenerate.A3 < 0) res |= 4;
            if (axisIsDegenerate.A4 < 0) res |= 8;
            return res;
        }

        public static Vector128<int> LessThanOrEqual<T>(in Vector128<T> left, in Vector128<T> right) where T : struct
        {
            return new Vector128<int>
            {
                A1 = Scalar<T>.LessThanOrEqual(left.A1, right.A1) ? -1 : 0,
                A2 = Scalar<T>.LessThanOrEqual(left.A2, right.A2) ? -1 : 0,
                A3 = Scalar<T>.LessThanOrEqual(left.A3, right.A3) ? -1 : 0,
                A4 = Scalar<T>.LessThanOrEqual(left.A4, right.A4) ? -1 : 0,
            };
        }
    }
    public struct Vector128<T> where T : struct
    {
        public T A1;
        public T A2;
        public T A3;
        public T A4;

        public Vector128(T value)
        {
            A1 = value;
            A2 = value;
            A3 = value;
            A4 = value;
        }

        public Vector128(T x, T y, T z, T w)
        {
            A1 = x;
            A2 = y;
            A3 = z;
            A4 = w;
        }

        public static Vector128<T> operator -(in Vector128<T> left, in Vector128<T> right)
        {
            return new Vector128<T>
            {
                A1 = Scalar<T>.Subtract(left.A1, right.A1),
                A2 = Scalar<T>.Subtract(left.A2, right.A2),
                A3 = Scalar<T>.Subtract(left.A3, right.A3),
                A4 = Scalar<T>.Subtract(left.A4, right.A4),
            };
        }

        public Vector4 AsVector4()
        {
            return new Vector4(Unsafe.As<T, Number>(ref A1), Unsafe.As<T, Number>(ref A2), Unsafe.As<T, Number>(ref A3), Unsafe.As<T, Number>(ref A4));
        }

        public static readonly Vector128<T> Zero = new Vector128<T>(Scalar<T>.Zero);
        public static readonly Vector128<T> One = new Vector128<T>(Scalar<T>.One);
        public static readonly Vector128<T> AllBitsSet = new Vector128<T>(Scalar<T>.AllBitsSet);

        //public static Vector4 ConditionalSelect(Vector4 axisIsDegenerate, Vector128<Number> zero, Vector4 vector4)
        //{
        //    throw new NotImplementedException();
        //}

        //public static Vector4 Create(Number x, Number y, Number z, Number w)
        //{
        //    return new Vector4(x, y, z, w);
        //}

        //public static Vector4 LessThanOrEqual(in Vector4 l, in Vector4 r)
        //{
        //    return new Vector4
        //    {
        //        X = l.X <= r.X ? Constants.Cm1 : 0,
        //        Y = l.Y <= r.Y ? Constants.Cm1 : 0,
        //        Z = l.Z <= r.Z ? Constants.Cm1 : 0,
        //        W = l.Z <= r.W ? Constants.Cm1 : 0
        //    };
        //}
    }
}
