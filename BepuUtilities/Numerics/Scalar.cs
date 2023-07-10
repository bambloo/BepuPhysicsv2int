using System;
using System.Runtime.CompilerServices;

namespace BepuUtilities.Numerics
{
    public static class Scalar<T>
    {
        public static T AllBitsSet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (typeof(T) == typeof(byte))
                {
                    return (T)(object)byte.MaxValue;
                }
                else if (typeof(T) == typeof(double))
                {
                    return (T)(object)BitConverter.Int64BitsToDouble(-1);
                }
                else if (typeof(T) == typeof(short))
                {
                    return (T)(object)(short)-1;
                }
                else if (typeof(T) == typeof(int))
                {
                    return (T)(object)-1;
                }
                else if (typeof(T) == typeof(long))
                {
                    return (T)(object)(long)-1;
                }
                else if (typeof(T) == typeof(nint))
                {
                    return (T)(object)(nint)(-1);
                }
                else if (typeof(T) == typeof(nuint))
                {
                    return (T)(object)nuint.MaxValue;
                }
                else if (typeof(T) == typeof(sbyte))
                {
                    return (T)(object)(sbyte)-1;
                }
                else if (typeof(T) == typeof(float))
                {
                    return (T)(object)BitConverter.Int32BitsToSingle(-1);
                }
                else if (typeof(T) == typeof(ushort))
                {
                    return (T)(object)ushort.MaxValue;
                }
                else if (typeof(T) == typeof(uint))
                {
                    return (T)(object)uint.MaxValue;
                }
                else if (typeof(T) == typeof(ulong))
                {
                    return (T)(object)ulong.MaxValue;
                }
                else if (typeof(T) == typeof(Number))
                {
                    var res = new Number();
                    Unsafe.As<Number, int>(ref res) = -1;
                    return (T)(object)res;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public static T One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (typeof(T) == typeof(byte))
                {
                    return (T)(object)(byte)1;
                }
                else if (typeof(T) == typeof(Number))
                {
                    return (T)(object)(Number)1;
                }
                else if (typeof(T) == typeof(short))
                {
                    return (T)(object)(short)1;
                }
                else if (typeof(T) == typeof(int))
                {
                    return (T)(object)1;
                }
                else if (typeof(T) == typeof(long))
                {
                    return (T)(object)(long)1;
                }
                else if (typeof(T) == typeof(nint))
                {
                    return (T)(object)(nint)1;
                }
                else if (typeof(T) == typeof(nuint))
                {
                    return (T)(object)(nuint)1;
                }
                else if (typeof(T) == typeof(sbyte))
                {
                    return (T)(object)(sbyte)1;
                }
                else if (typeof(T) == typeof(Number))
                {
                    return (T)(object)(Number)1;
                }
                else if (typeof(T) == typeof(ushort))
                {
                    return (T)(object)(ushort)1;
                }
                else if (typeof(T) == typeof(uint))
                {
                    return (T)(object)(uint)1;
                }
                else if (typeof(T) == typeof(ulong))
                {
                    return (T)(object)(ulong)1;
                }
                else if (typeof(T) == typeof(Number))
                {
                    return (T)(object)new Number(1);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public static T Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (typeof(T) == typeof(Number))
                    return (T)(object)(new Number(0.0f));
                else if (typeof(T) == typeof(int))
                    return (T)(object)(0);
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Abs(T value)
        {
            // byte, ushort, uint, and ulong should have already been handled

            if (typeof(T) == typeof(Number))
            {
                return (T)(object)BepuUtilities.Utils.Math.Abs((Number)(object)value);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)Math.Abs((short)(object)value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)System.Math.Abs((int)(object)value);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)Math.Abs((long)(object)value);
            }
            else if (typeof(T) == typeof(nint))
            {
                return (T)(object)Math.Abs((nint)(object)value);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)System.Math.Abs((sbyte)(object)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Add(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (T)(object)(byte)((byte)(object)left + (byte)(object)right);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)left + (float)(object)right);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)(short)((short)(object)left + (short)(object)right);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)((int)(object)left + (int)(object)right);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)((long)(object)left + (long)(object)right);
            }
            else if (typeof(T) == typeof(nint))
            {
                return (T)(object)((nint)(object)left + (nint)(object)right);
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (T)(object)((nuint)(object)left + (nuint)(object)right);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)(sbyte)((sbyte)(object)left + (sbyte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left + (Number)(object)right);
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (T)(object)(ushort)((ushort)(object)left + (ushort)(object)right);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)((uint)(object)left + (uint)(object)right);
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (T)(object)((ulong)(object)left + (ulong)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)((Number)(object)left + (Number)(object)right);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Ceiling(T value)
        {
            if (typeof(T) == typeof(Number))
            {
                return (T)(object)BepuUtilities.Utils.Math.Ceiling((Number)(object)value);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)MathF.Ceiling((float)(object)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Divide(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (T)(object)(byte)((byte)(object)left / (byte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left / (Number)(object)right);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)(short)((short)(object)left / (short)(object)right);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)((int)(object)left / (int)(object)right);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)((long)(object)left / (long)(object)right);
            }
            else if (typeof(T) == typeof(nint))
            {
                return (T)(object)((nint)(object)left / (nint)(object)right);
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (T)(object)((nuint)(object)left / (nuint)(object)right);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)(sbyte)((sbyte)(object)left / (sbyte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left / (Number)(object)right);
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (T)(object)(ushort)((ushort)(object)left / (ushort)(object)right);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)((uint)(object)left / (uint)(object)right);
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (T)(object)((ulong)(object)left / (ulong)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)((Number)(object)left / (Number)(object)right);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(T left, T right)
        {
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ExtractMostSignificantBit(T value)
        {
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Floor(T value)
        {
            if (typeof(T) == typeof(Number))
            {
                return (T)(object)BepuUtilities.Utils.Math.Floor((Number)(object)value);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)MathF.Floor((float)(object)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (byte)(object)left > (byte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left > (Number)(object)right;
            }
            else if (typeof(T) == typeof(short))
            {
                return (short)(object)left > (short)(object)right;
            }
            else if (typeof(T) == typeof(int))
            {
                return (int)(object)left > (int)(object)right;
            }
            else if (typeof(T) == typeof(long))
            {
                return (long)(object)left > (long)(object)right;
            }
            else if (typeof(T) == typeof(nint))
            {
                return (nint)(object)left > (nint)(object)right;
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (nuint)(object)left > (nuint)(object)right;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (sbyte)(object)left > (sbyte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left > (Number)(object)right;
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (ushort)(object)left > (ushort)(object)right;
            }
            else if (typeof(T) == typeof(uint))
            {
                return (uint)(object)left > (uint)(object)right;
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (ulong)(object)left > (ulong)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left > (Number)(object)right;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqual(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (byte)(object)left >= (byte)(object)right;
            }
            else if (typeof(T) == typeof(float))
            {
                return (float)(object)left >= (float)(object)right;
            }
            else if (typeof(T) == typeof(short))
            {
                return (short)(object)left >= (short)(object)right;
            }
            else if (typeof(T) == typeof(int))
            {
                return (int)(object)left >= (int)(object)right;
            }
            else if (typeof(T) == typeof(long))
            {
                return (long)(object)left >= (long)(object)right;
            }
            else if (typeof(T) == typeof(nint))
            {
                return (nint)(object)left >= (nint)(object)right;
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (nuint)(object)left >= (nuint)(object)right;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (sbyte)(object)left >= (sbyte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (double)(object)left >= (double)(object)right;
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (ushort)(object)left >= (ushort)(object)right;
            }
            else if (typeof(T) == typeof(uint))
            {
                return (uint)(object)left >= (uint)(object)right;
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (ulong)(object)left >= (ulong)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left >= (Number)(object)right;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (byte)(object)left < (byte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left < (Number)(object)right;
            }
            else if (typeof(T) == typeof(short))
            {
                return (short)(object)left < (short)(object)right;
            }
            else if (typeof(T) == typeof(int))
            {
                return (int)(object)left < (int)(object)right;
            }
            else if (typeof(T) == typeof(long))
            {
                return (long)(object)left < (long)(object)right;
            }
            else if (typeof(T) == typeof(nint))
            {
                return (nint)(object)left < (nint)(object)right;
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (nuint)(object)left < (nuint)(object)right;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (sbyte)(object)left < (sbyte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left < (Number)(object)right;
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (ushort)(object)left < (ushort)(object)right;
            }
            else if (typeof(T) == typeof(uint))
            {
                return (uint)(object)left < (uint)(object)right;
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (ulong)(object)left < (ulong)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left < (Number)(object)right;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqual(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (byte)(object)left <= (byte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left <= (Number)(object)right;
            }
            else if (typeof(T) == typeof(short))
            {
                return (short)(object)left <= (short)(object)right;
            }
            else if (typeof(T) == typeof(int))
            {
                return (int)(object)left <= (int)(object)right;
            }
            else if (typeof(T) == typeof(long))
            {
                return (long)(object)left <= (long)(object)right;
            }
            else if (typeof(T) == typeof(nint))
            {
                return (nint)(object)left <= (nint)(object)right;
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (nuint)(object)left <= (nuint)(object)right;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (sbyte)(object)left <= (sbyte)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left <= (Number)(object)right;
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (ushort)(object)left <= (ushort)(object)right;
            }
            else if (typeof(T) == typeof(uint))
            {
                return (uint)(object)left <= (uint)(object)right;
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (ulong)(object)left <= (ulong)(object)right;
            }
            else if (typeof(T) == typeof(Number))
            {
                return (Number)(object)left <= (Number)(object)right;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Multiply(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (T)(object)(byte)((byte)(object)left * (byte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left * (Number)(object)right);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)(short)((short)(object)left * (short)(object)right);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)((int)(object)left * (int)(object)right);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)((long)(object)left * (long)(object)right);
            }
            else if (typeof(T) == typeof(nint))
            {
                return (T)(object)((nint)(object)left * (nint)(object)right);
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (T)(object)((nuint)(object)left * (nuint)(object)right);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)(sbyte)((sbyte)(object)left * (sbyte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left * (Number)(object)right);
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (T)(object)(ushort)((ushort)(object)left * (ushort)(object)right);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)((uint)(object)left * (uint)(object)right);
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (T)(object)((ulong)(object)left * (ulong)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)((Number)(object)left * (Number)(object)right);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Subtract(T left, T right)
        {
            if (typeof(T) == typeof(byte))
            {
                return (T)(object)(byte)((byte)(object)left - (byte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left - (Number)(object)right);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)(short)((short)(object)left - (short)(object)right);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)((int)(object)left - (int)(object)right);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)((long)(object)left - (long)(object)right);
            }
            else if (typeof(T) == typeof(nint))
            {
                return (T)(object)((nint)(object)left - (nint)(object)right);
            }
            else if (typeof(T) == typeof(nuint))
            {
                return (T)(object)((nuint)(object)left - (nuint)(object)right);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)(sbyte)((sbyte)(object)left - (sbyte)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)(Number)((Number)(object)left - (Number)(object)right);
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (T)(object)(ushort)((ushort)(object)left - (ushort)(object)right);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)((uint)(object)left - (uint)(object)right);
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (T)(object)((ulong)(object)left - (ulong)(object)right);
            }
            else if (typeof(T) == typeof(Number))
            {
                return (T)(object)((Number)(object)left - (Number)(object)right);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Negate(T value)
        {
            if (typeof(T) == typeof(Number))
            {
                return (T)(object)(-(Number)(object)value);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)(short)(-(short)(object)value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)(-(int)(object)value);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)(-(long)(object)value);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)(sbyte)(-(sbyte)(object)value);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)(float)(-(float)(object)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
