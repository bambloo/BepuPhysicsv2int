using BepuUtilities.Numerics;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BepuPhysics.Constraints
{
    public static class ConstraintChecker
    {
        /// <summary>
        /// Checks if a value is a finite number- neither infinite nor NaN.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the value is neither infinite nor NaN, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFiniteNumber(Number value)
        {
            return !Number.IsInfinity(value) && !Number.IsNaN(value);
        }
        /// <summary>
        /// Checks if a value is a finite value greater than zero and not NaN.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the value is a finite number greater than zero and not NaN, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveNumber(Number value)
        {
            return IsFiniteNumber(value) && value > 0;
        }
        /// <summary>
        /// Checks if a value is a finite value greater than or equal to zero and not NaN.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the value is a finite number greater than or equal to zero and not NaN, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNonnegativeNumber(Number value)
        {
            return IsFiniteNumber(value) && value >= 0;
        }
        /// <summary>
        /// Checks if a value is a finite value less than zero and not NaN.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the value is a finite number less than zero and not NaN, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeNumber(Number value)
        {
            return IsFiniteNumber(value) && value < 0;
        }
        /// <summary>
        /// Checks if a value is a finite value less than or equal to zero and not NaN.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the value is a finite number less than or equal to zero and not NaN, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNonpositiveNumber(Number value)
        {
            return IsFiniteNumber(value) && value <= 0;
        }
        [Conditional("DEBUG")]
        public static void AssertUnitLength(Vector3 v, string typeName, string propertyName)
        {
            var lengthSquared = v.LengthSquared();
            if (lengthSquared > 1 + Constants.C1em5 || lengthSquared < 1 - Constants.C1em5 || !IsFiniteNumber(lengthSquared))
            {
                Debug.Fail($"{typeName}.{propertyName} must be unit length.");
            }
        }
        [Conditional("DEBUG")]
        public static void AssertUnitLength(Quaternion q, string typeName, string propertyName)
        {
            var lengthSquared = q.LengthSquared();
            if (lengthSquared > 1 + Constants.C1em5 || lengthSquared < 1 - Constants.C1em5 || !IsFiniteNumber(lengthSquared))
            {
                Debug.Fail($"{typeName}.{propertyName} must be unit length.");
            }
        }

        [Conditional("DEBUG")]
        public static void AssertValid(in SpringSettings settings, string typeName)
        {
            if (!SpringSettings.Validate(settings))
            {
                Debug.Fail($"{typeName}.SpringSettings must have positive frequency and nonnegative damping ratio.");
            }
        }

        [Conditional("DEBUG")]
        public static void AssertValid(in MotorSettings settings, string typeName)
        {
            if (!MotorSettings.Validate(settings))
            {
                Debug.Fail($"{typeName}.MotorSettings must have nonnegative maximum force and damping.");
            }
        }

        [Conditional("DEBUG")]
        public static void AssertValid(in ServoSettings settings, string typeName)
        {
            if (!ServoSettings.Validate(settings))
            {
                Debug.Fail($"{typeName}.ServoSettings must have nonnegative maximum speed, base speed, and maximum force.");
            }
        }

        [Conditional("DEBUG")]
        public static void AssertValid(in ServoSettings servoSettings, in SpringSettings springSettings, string typeName)
        {
            AssertValid(servoSettings, typeName);
            AssertValid(springSettings, typeName);
        }
    }
}
