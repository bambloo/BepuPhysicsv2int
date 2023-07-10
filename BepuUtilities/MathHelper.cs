using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;

namespace BepuUtilities
{
    /// <summary>
    /// Contains helper math methods.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Approximate value of Pi.
        /// </summary>
        public static readonly Number Pi = Number.PI;

        /// <summary>
        /// Approximate value of Pi multiplied by two.
        /// </summary>
        public static readonly Number TwoPi = Number.TwoPi;

        /// <summary>
        /// Approximate value of Pi divided by two.
        /// </summary>
        public static readonly Number PiOver2 = Number.PiOver2;

        /// <summary>
        /// Approximate value of Pi divided by four.
        /// </summary>
        public static readonly Number PiOver4 = Number.PiOver4;

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum value.  If the value is less than this, the minimum is returned instead.</param>
        /// <param name="max">Maximum value.  If the value is more than this, the maximum is returned instead.</param>
        /// <returns>Clamped value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number Clamp(Number value, Number min, Number max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            return value;
        }


        /// <summary>
        /// Returns the higher value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Higher value of the two parameters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number Max(Number a, Number b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the lower value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Lower value of the two parameters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number Min(Number a, Number b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum value.  If the value is less than this, the minimum is returned instead.</param>
        /// <param name="max">Maximum value.  If the value is more than this, the maximum is returned instead.</param>
        /// <returns>Clamped value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            return value;
        }


        /// <summary>
        /// Returns the higher value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Higher value of the two parameters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the lower value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Lower value of the two parameters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="min">Minimum value.  If the value is less than this, the minimum is returned instead.</param>
        /// <param name="max">Maximum value.  If the value is more than this, the maximum is returned instead.</param>
        /// <returns>Clamped value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            return value;
        }


        /// <summary>
        /// Returns the higher value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Higher value of the two parameters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Max(long a, long b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the lower value of the two parameters.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>Lower value of the two parameters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long a, long b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees to convert.</param>
        /// <returns>Radians equivalent to the input degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number ToRadians(Number degrees)
        {
            return degrees * (Pi / Constants.C180);
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">Radians to convert.</param>
        /// <returns>Degrees equivalent to the input radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number ToDegrees(Number radians)
        {
            return radians * (Constants.C180 / Pi);
        }


        /// <summary>
        /// Returns -1 if the value is negative and 1 otherwise.
        /// </summary>
        /// <param name="x">Value to compute the sign of.</param>
        /// <returns>-1 if the input is negative, and 1 otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Number BinarySign(Number x)
        {
            return x < 0 ? -1 : 1;
        }

        //Note that these cos/sin implementations are not here for performance, but rather to:
        //1) Provide a SIMD accelerated version for wide processing, and
        //2) Provide a scalar implementation that is consistent with the SIMD version for systems which need to match its behavior.

        /// <summary>
        /// Computes an approximation of cosine. Maximum error a little below 8e-7 for the interval -2 * Pi to 2 * Pi. Values further from the interval near zero have gracefully degrading error.
        /// </summary>
        /// <param name="x">Value to take the cosine of.</param>
        /// <returns>Approximate cosine of the input value.</returns>
        public static Number Cos(Number x)
        {
            return Number.Cos(x);

        }
        /// <summary>
        /// Computes an approximation of sine. Maximum error a little below 5e-7 for the interval -2 * Pi to 2 * Pi. Values further from the interval near zero have gracefully degrading error.
        /// </summary>
        /// <param name="x">Value to take the sine of.</param>
        /// <returns>Approximate sine of the input value.</returns>
        public static Number Sin(Number x)
        {
            return Number.Sin(x);
        }

        /// <summary>
        /// Computes an approximation of arccos. Inputs outside of [-1, 1] are clamped. Maximum error less than 5.17e-07.
        /// </summary>
        /// <param name="x">Input value to the arccos function.</param>
        /// <returns>Result of the arccos function.</returns>
        public static Number Acos(Number x)
        {
            return Number.Acos(x);
        }

        /// <summary>
        /// Computes an approximation of cosine. Maximum error a little below 8e-7 for the interval -2 * Pi to 2 * Pi. Values further from the interval near zero have gracefully degrading error.
        /// </summary>
        /// <param name="x">Values to take the cosine of.</param>
        /// <returns>Approximate cosine of the input values.</returns>
        public static Vector<Number> Cos(Vector<Number> x)
        {
            return new Vector<Number>
            {
                A1 = Number.Cos(x.A1),
                A2 = Number.Cos(x.A2),
                A3 = Number.Cos(x.A3),
                A4 = Number.Cos(x.A4),
                A5 = Number.Cos(x.A5),
                A6 = Number.Cos(x.A6),
                A7 = Number.Cos(x.A7),
                A8 = Number.Cos(x.A8),
            };
        }
        /// <summary>
        /// Computes an approximation of sine. Maximum error a little below 5e-7 for the interval -2 * Pi to 2 * Pi. Values further from the interval near zero have gracefully degrading error.
        /// </summary>
        /// <param name="x">Value to take the sine of.</param>
        /// <returns>Approximate sine of the input value.</returns>
        public static Vector<Number> Sin(Vector<Number> x)
        {
            return new Vector<Number>
            {
                A1 = Number.Sin(x.A1),
                A2 = Number.Sin(x.A2),
                A3 = Number.Sin(x.A3),
                A4 = Number.Sin(x.A4),
                A5 = Number.Sin(x.A5),
                A6 = Number.Sin(x.A6),
                A7 = Number.Sin(x.A7),
                A8 = Number.Sin(x.A8),
            };
        }

        /// <summary>
        /// Computes an approximation of arccos. Inputs outside of [-1, 1] are clamped. Maximum error less than 5.17e-07.
        /// </summary>
        /// <param name="x">Input value to the arccos function.</param>
        /// <returns>Result of the arccos function.</returns>
        public static Vector<Number> Acos(Vector<Number> x)
        {
            x = Vector.Min(x, Vector<Number>.One);
            return new Vector<Number>
            {
                A1 = Number.Acos(x.A1),
                A2 = Number.Acos(x.A2),
                A3 = Number.Acos(x.A3),
                A4 = Number.Acos(x.A4),
                A5 = Number.Acos(x.A5),
                A6 = Number.Acos(x.A6),
                A7 = Number.Acos(x.A7),
                A8 = Number.Acos(x.A8),
            };
        }

        /// <summary>
        /// Gets the change in angle from a to b as a signed value from -pi to pi.
        /// </summary>
        /// <param name="a">Source angle.</param>
        /// <param name="b">Target angle.</param>
        /// <param name="difference">Difference between a and b, expressed as a value from -pi to pi.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetSignedAngleDifference(in Vector<Number> a, in Vector<Number> b, out Vector<Number> difference)
        {
            var half = new Vector<Number>(Constants.C0p5);
            var x = (b - a) * new Vector<Number>(Constants.C1 / TwoPi) + half;
            difference = (x - Vector.Floor(x) - half) * new Vector<Number>(TwoPi);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<Number> FastReciprocal(Vector<Number> v)
        {
            //if (Avx.IsSupported && Vector<Number>.Count == 8)
            //{
            //    return Avx.Reciprocal(v.AsVector256()).AsVector();
            //}
            //else if (Sse.IsSupported && Vector<Number>.Count == 4)
            //{
            //    return Sse.Reciprocal(v.AsVector128()).AsVector();
            //}
            //else
            //{
                return Vector<Number>.One / v;
            //}
            //TODO: Arm!
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<Number> FastReciprocalSquareRoot(Vector<Number> v)
        {
            //if (Avx.IsSupported && Vector<Number>.Count == 8)
            //{
            //    return Avx.ReciprocalSqrt(v.AsVector256()).AsVector();
            //}
            //else if (Sse.IsSupported && Vector<Number>.Count == 4)
            //{
            //    return Sse.ReciprocalSqrt(v.AsVector128()).AsVector();
            //}
            //else
            //{
                return Vector<Number>.One / Vector.SquareRoot(v);
            //}
            //TODO: Arm!
        }
    }
}
