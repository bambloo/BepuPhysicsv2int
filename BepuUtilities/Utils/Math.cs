using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;

namespace BepuUtilities.Utils
{
    public class Math
    {
        public static readonly Number PI = Number.PI;
        public static Number Sqrt(Number value)
        {
            return Number.Sqrt(value);
        }

        public static Number Max(Number left, Number right)
        {
            return left < right ? right : left;
        }

        public static int Max(int left, int right)
        {
            return left < right ? right : left;
        }

        public static Number Min(Number left, Number right)
        {
            return left > right ? right : left;
        }
        public static int Min(int left, int right)
        {
            return left > right ? right : left;
        }

        public static Number Abs(Number value)
        {
            return Number.Abs(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Abs(long value)
        {
            return System.Math.Abs(value);
        }

        public static Number Tan(Number value)
        {
            return Number.Tan(value);
        }

        public static Number Acos(Number value)
        {
            return Number.Acos(value);
        }

        public static Number Sin(Number number)
        {
            return Number.Sin(number);
        }

        public static Number Cos(Number number)
        {
            return Number.Cos(number);
        }

        public static Number Floor(Number number)
        {
            return Number.Floor(number);
        }

        public static Number Ceiling(Number number)
        {
            return Number.Ceiling(number);
        }
        public static Number Round(Number number)
        {
            return Number.Ceiling(number);
        }

        public static Number Clamp(Number value, Number min, Number max)
        {
            return Number.Clamp(value, min, max);
        }

        public static Number Pow(Number b, Number e)
        {
            return Number.Pow(b, e);
        }

        public static Number Asin(Number sval)
        {
            return Number.Asin(sval);
        }

        public static Number Log10(Number val)
        {
            return Number.Log10(val);
        }

        public static Number Round(Number x, int digit)
        {
            return Number.Round(x, digit);
        }

        public static int Sign(Number x)
        {
            return Number.Sign(x);
        }
    }

    public class MathF : Math {

        public static readonly Number Tau = Number.Tau;

        public static Number Asin(Number number)
        {
            return Number.Asin(number);
        }

        public static Number Atan(Number value)
        {
            return Number.Atan(value);
        }

        public static Number Atan2(Number x, Number number)
        {
            return Number.Atan2(x, number);
        }

        public static Number Log2(Number value)
        {
            return Number.Log2(value);
        }

        public static Number Truncate(Number value)
        {
            return Number.Truncate(value);
        }
    }

    public class BitOperations
    {
        public static uint RoundUpToPowerOf2(uint value)
        {
            --value;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }

        public static ulong RoundUpToPowerOf2(ulong value)
        {
            return System.Numerics.BitOperations.RoundUpToPowerOf2(value);
            --value;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value |= value >> 32;
            return value + 1;
        }

        public static int LeadingZeroCount(uint value)
        {
            return System.Numerics.BitOperations.LeadingZeroCount(value);
            if (value == 0)
                return 32;

            uint temp;
            int count = 0;
            while ((temp = value & 0xF0000000) == 0)
            {
                count += 4;
                value <<= 4;
            }

            if ((temp & 0x80000000) != 0)
                return count;

            if ((temp & 0x40000000) != 0)
                return count + 1;

            if ((temp & 0x20000000) != 0)
                return count + 2;
            return count + 3;
        }

        public static int Log2(uint value)
        {
            return System.Numerics.BitOperations.Log2(value);
            return 31 ^ LeadingZeroCount(value);
        }
    }
}
