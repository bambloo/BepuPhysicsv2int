using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace BepuUtilities.Numerics
{
    public struct Number : 
        IMultiplyOperators<Number, Number, Number>, 
        IAdditionOperators<Number, Number, Number>, 
        IComparisonOperators<Number, Number, bool>,
        ISubtractionOperators<Number, Number, Number>,
        IDivisionOperators<Number, Number, Number>,
        IBitwiseOperators<Number, Number, Number>,
        IUnaryNegationOperators<Number, Number>,
        IComparable<Number>
    {
        public static readonly Number PI = new Number(3.1415926535898f);
        public static readonly Number TwoPi = new Number(6.283185307179586477f);
        public static readonly Number PiOver2 = new Number(1.570796326794896619f);
        public static readonly Number PiOver4 = new Number(0.785398163397448310f);
        public static readonly Number MaxValue = new Number(float.MaxValue);
        public static readonly Number MinValue = new Number(float.MinValue);
        public static readonly Number PositiveInfinity = new Number(float.PositiveInfinity);
        public static readonly Number NormalEpsilon = new Number(1.1754943508e-38f);
        public static readonly Number Tau = new Number(float.Tau);
        float val;

        public Number(float val)
        {
            this.val = val;
        }

        // TODO
        public static implicit operator Number(float value)
        {
            Number res;
            res.val = value;
            return res;
        }

        public static implicit operator Number(double value)
        {
            Number res;
            res.val = (float)value;
            return res;
        }

        public static implicit operator Number(int value)
        {
            Number res;
            res.val = value;
            return res;
        }

        public static implicit operator Number(uint value)
        {
            Number res;
            res.val = value;
            return res;
        }

        public static implicit operator float(Number value)
        {
            return value.val;
        }

        public static explicit operator int(Number value)
        {
            return (int)value.val;
        }

        public static explicit operator uint(Number value)
        {
            return (uint)value.val;
        }

        public static Number Sqrt(Number a)
        {
            Number res;
            res.val = System.MathF.Sqrt(a.val);
            return res;
        }

        public static Number Abs(Number a)
        {
            Number res;
            res.val = System.MathF.Abs(a.val);
            return res;
        }

        public static Number Sin(Number a)
        {
            Number res;
            res.val = System.MathF.Sin(a.val);
            return res;
        }

        public static Number Cos(Number a)
        {
            Number res;
            res.val = System.MathF.Cos(a.val);
            return res;
        }

        public static Number Tan(Number a)
        {
            Number res;
            res.val = System.MathF.Tan(a.val);
            return res;
        }
        public static Number Floor(Number a)
        {
            Number res;
            res.val = System.MathF.Floor(a.val);
            return res;
        }

        public static Number Ceiling(Number a)
        {
            Number res;
            res.val = System.MathF.Ceiling(a.val);
            return res;
        }

        public static Number Round(Number a)
        {
            Number res;
            res.val = System.MathF.Round(a.val);
            return res;
        }

        public static Number operator *(Number left, Number right)
        {
            Number res;
            res.val = left.val * right.val;
            return res;
        }

        public static Number operator *(Number left, uint right)
        {
            Number res;
            res.val = left.val * right;
            return res;
        }

        public static Number operator +(Number left, Number right)
        {
            Number res;
            res.val = left.val + right.val;
            return res;
        }

        public static Number operator %(Number left, int right)
        {
            return new Number(left.val % right);
        }

        public static Number operator %(Number left, Number right)
        {
            return new Number(left.val % right.val);
        }

        public static bool operator >(Number left, Number right)
        {
            return left.val > right.val;
            
        }

        public static bool operator >=(Number left, Number right)
        {
            return left.val >= right.val;
        }

        public static bool operator <(Number left, Number right)
        {
            return left.val < right.val;
        }

        public static bool operator <=(Number left, Number right)
        {
            return left.val <= right.val;
        }

        public static bool operator ==(Number left, Number right)
        {
            return left.val == right.val;
        }

        public static bool operator !=(Number left, Number right)
        {
            return left.val != right.val;
        }

        public static Number operator -(Number left, Number right)
        {
            Number res;
            res.val = left.val - right.val;
            return res;
        }

        public static Number operator /(Number left, Number right)
        {
            Number res;
            res.val = left.val / right.val;
            return res;
        }

        public static Number operator -(Number value)
        {
            return new Number(-value.val);
        }

        public static Number operator &(Number left, Number right)
        {
            throw new NotImplementedException();
        }

        public unsafe static Number operator |(Number left, Number right)
        {
            Number res;
            *(int*)&res = *(int*)&left | *(int*)&right;
            return res;
        }

        public static Number operator ^(Number left, Number right)
        {
            throw new NotImplementedException();
        }

        public static Number Max(Number left, Number right)
        {
            return left > right ? left : right;
        }

        public static Number Min(Number left, Number right)
        {
            return left < right ? left : right;
        }

        public static Number operator ~(Number value)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return val.GetHashCode();
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is Number)
            {
                return ((Number)obj).val == val;
            }
            return false;
        }

        internal static Number Acos(Number value)
        {
            return new Number(System.MathF.Acos(value.val));
        }

        public static bool IsNaN(Number value)
        {
            return float.IsNaN(value.val);
        }

        public static bool IsInfinity(Number value)
        {
            return float.IsInfinity(value.val);
        }

        public int CompareTo(Number other)
        {
            return val.CompareTo(other.val);
        }

        public static Number Pow(Number a, Number b)
        {
            return new Number(MathF.Pow(a.val, b.val));
        }

        internal static Number Clamp(Number value, Number min, Number max)
        {
            return new Number(System.Math.Clamp(value.val, min.val, max.val));
        }

        internal static Number Atan(Number value)
        {
            return new Number(System.MathF.Atan(value.val));
        }

        internal static Number Log2(Number value)
        {
            return new Number(System.MathF.Log2(value.val));
        }

        internal static Number Asin(Number number)
        {
            return new Number(System.MathF.Asin(number.val));
        }

        internal static Number Atan2(Number x, Number number)
        {
            return new Number(System.MathF.Atan2(x.val, number.val));
        }

        internal static Number Truncate(Number value)
        {
            return new Number(System.MathF.Truncate(value.val));
        }

        internal static Number Log10(Number val)
        {
            return new Number(System.MathF.Log10(val.val)); 
        }

        internal static Number Round(Number x, int digit)
        {
            return new Number(System.MathF.Round(x.val, digit));
        }

        internal static int Sign(Number x)
        {
            return System.MathF.Sign(x.val);
        }

        public override string ToString()
        {
            return val.ToString();
        }
    }
}
