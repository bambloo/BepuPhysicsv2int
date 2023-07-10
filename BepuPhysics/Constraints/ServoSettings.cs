using BepuUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BepuPhysics.Constraints
{
    public struct ServoSettings
    {
        public Number MaximumSpeed;
        public Number BaseSpeed;
        public Number MaximumForce;

        /// <summary>
        /// Gets settings representing a servo with unlimited force, speed, and no base speed.
        /// </summary>
        public static ServoSettings Default { get { return new ServoSettings(Number.MaxValue, 0, Number.MaxValue); } }

        /// <summary>
        /// Checks servo settings to ensure valid values.
        /// </summary>
        /// <param name="settings">Settings to check.</param>
        /// <returns>True if the settings contain valid values, false otherwise.</returns>
        public static bool Validate(in ServoSettings settings)
        {
            return ConstraintChecker.IsNonnegativeNumber(settings.MaximumSpeed) && ConstraintChecker.IsNonnegativeNumber(settings.BaseSpeed) && ConstraintChecker.IsNonnegativeNumber(settings.MaximumForce);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ServoSettings(Number maximumSpeed, Number baseSpeed, Number maximumForce)
        {
            MaximumSpeed = maximumSpeed;
            BaseSpeed = baseSpeed;
            MaximumForce = maximumForce;
            Debug.Assert(Validate(this), "Servo settings must have nonnegative maximum speed, base speed, and maximum force.");
        }
    }
    public struct ServoSettingsWide
    {
        public Vector<Number> MaximumSpeed;
        public Vector<Number> BaseSpeed;
        public Vector<Number> MaximumForce;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeClampedBiasVelocity(in Vector<Number> error, in Vector<Number> positionErrorToVelocity, in ServoSettingsWide servoSettings, Number dt, Number inverseDt, 
            out Vector<Number> clampedBiasVelocity, out Vector<Number> maximumImpulse)
        {
            //Can't request speed that would cause an overshoot.
            var baseSpeed = Vector.Min(servoSettings.BaseSpeed, Vector.Abs(error) * new Vector<Number>(inverseDt));
            var biasVelocity = error * positionErrorToVelocity;
            clampedBiasVelocity = Vector.ConditionalSelect(Vector.LessThan(biasVelocity, Vector<Number>.Zero),
                Vector.Max(-servoSettings.MaximumSpeed, Vector.Min(-baseSpeed, biasVelocity)),
                Vector.Min(servoSettings.MaximumSpeed, Vector.Max(baseSpeed, biasVelocity)));
            maximumImpulse = servoSettings.MaximumForce * new Vector<Number>(dt);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeClampedBiasVelocity(in Vector2Wide errorAxis, in Vector<Number> errorLength, in Vector<Number> positionErrorToBiasVelocity, in ServoSettingsWide servoSettings,
            Number dt, Number inverseDt, out Vector2Wide clampedBiasVelocity, out Vector<Number> maximumImpulse)
        {
            //Can't request speed that would cause an overshoot.
            var baseSpeed = Vector.Min(servoSettings.BaseSpeed, errorLength * new Vector<Number>(inverseDt));
            var unclampedBiasSpeed = errorLength * positionErrorToBiasVelocity;
            var targetSpeed = Vector.Max(baseSpeed, unclampedBiasSpeed);
            var scale = Vector.Min(Vector<Number>.One, servoSettings.MaximumSpeed / targetSpeed);
            //Protect against division by zero. The min would handle inf, but if MaximumSpeed is 0, it turns into a NaN.
            var useFallback = Vector.LessThan(targetSpeed, new Vector<Number>(Constants.C1em10));
            scale = Vector.ConditionalSelect(useFallback, Vector<Number>.One, scale);
            Vector2Wide.Scale(errorAxis, scale * unclampedBiasSpeed, out clampedBiasVelocity);
            maximumImpulse = servoSettings.MaximumForce * new Vector<Number>(dt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeClampedBiasVelocity(in Vector2Wide error, in Vector<Number> positionErrorToBiasVelocity, in ServoSettingsWide servoSettings,
            Number dt, Number inverseDt, out Vector2Wide clampedBiasVelocity, out Vector<Number> maximumImpulse)
        {
            Vector2Wide.Length(error, out var errorLength);
            Vector2Wide.Scale(error, Vector<Number>.One / errorLength, out var errorAxis);
            var useFallback = Vector.LessThan(errorLength, new Vector<Number>(Constants.C1em10));
            errorAxis.X = Vector.ConditionalSelect(useFallback, Vector<Number>.Zero, errorAxis.X);
            errorAxis.Y = Vector.ConditionalSelect(useFallback, Vector<Number>.Zero, errorAxis.Y);
            ComputeClampedBiasVelocity(errorAxis, errorLength, positionErrorToBiasVelocity, servoSettings, dt, inverseDt, out clampedBiasVelocity, out maximumImpulse);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeClampedBiasVelocity(in Vector3Wide errorAxis, in Vector<Number> errorLength, in Vector<Number> positionErrorToBiasVelocity, in ServoSettingsWide servoSettings,
            Number dt, Number inverseDt, out Vector3Wide clampedBiasVelocity, out Vector<Number> maximumImpulse)
        {
            //Can't request speed that would cause an overshoot.
            var baseSpeed = Vector.Min(servoSettings.BaseSpeed, errorLength * new Vector<Number>(inverseDt));
            var unclampedBiasSpeed = errorLength * positionErrorToBiasVelocity;
            var targetSpeed = Vector.Max(baseSpeed, unclampedBiasSpeed);
            var scale = Vector.Min(Vector<Number>.One, servoSettings.MaximumSpeed / targetSpeed);
            //Protect against division by zero. The min would handle inf, but if MaximumSpeed is 0, it turns into a NaN.
            var useFallback = Vector.LessThan(targetSpeed, new Vector<Number>(Constants.C1em10));
            scale = Vector.ConditionalSelect(useFallback, Vector<Number>.One, scale);
            Vector3Wide.Scale(errorAxis, scale * unclampedBiasSpeed, out clampedBiasVelocity);
            maximumImpulse = servoSettings.MaximumForce * new Vector<Number>(dt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeClampedBiasVelocity(in Vector3Wide error, in Vector<Number> positionErrorToBiasVelocity, in ServoSettingsWide servoSettings,
           Number dt, Number inverseDt, out Vector3Wide clampedBiasVelocity, out Vector<Number> maximumImpulse)
        {
            Vector3Wide.Length(error, out var errorLength);
            Vector3Wide.Scale(error, Vector<Number>.One / errorLength, out var errorAxis);
            var useFallback = Vector.LessThan(errorLength, new Vector<Number>(Constants.C1em10));
            errorAxis.X = Vector.ConditionalSelect(useFallback, Vector<Number>.Zero, errorAxis.X);
            errorAxis.Y = Vector.ConditionalSelect(useFallback, Vector<Number>.Zero, errorAxis.Y);
            errorAxis.Z = Vector.ConditionalSelect(useFallback, Vector<Number>.Zero, errorAxis.Z);
            ComputeClampedBiasVelocity(errorAxis, errorLength, positionErrorToBiasVelocity, servoSettings, dt, inverseDt, out clampedBiasVelocity, out maximumImpulse);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampImpulse(in Vector<Number> maximumImpulse, ref Vector<Number> accumulatedImpulse, ref Vector<Number> csi)
        {
            var previousImpulse = accumulatedImpulse;
            accumulatedImpulse = Vector.Max(-maximumImpulse, Vector.Min(maximumImpulse, accumulatedImpulse + csi));
            csi = accumulatedImpulse - previousImpulse;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampImpulse(in Vector<Number> maximumImpulse, ref Vector2Wide accumulatedImpulse, ref Vector2Wide csi)
        {
            var previousImpulse = accumulatedImpulse;
            Vector2Wide.Add(accumulatedImpulse, csi, out var unclamped);
            Vector2Wide.Length(unclamped, out var impulseMagnitude);
            var impulseScale = Vector.ConditionalSelect(
                Vector.LessThan(Vector.Abs(impulseMagnitude), new Vector<Number>(Constants.C1em10)),
                Vector<Number>.One,
                Vector.Min(maximumImpulse / impulseMagnitude, Vector<Number>.One));
            Vector2Wide.Scale(unclamped, impulseScale, out accumulatedImpulse);
            Vector2Wide.Subtract(accumulatedImpulse, previousImpulse, out csi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampImpulse(in Vector<Number> maximumImpulse, ref Vector3Wide accumulatedImpulse, ref Vector3Wide csi)
        {
            var previousAccumulatedImpulse = accumulatedImpulse;
            Vector3Wide.Add(accumulatedImpulse, csi, out accumulatedImpulse);
            Vector3Wide.Length(accumulatedImpulse, out var impulseMagnitude);
            var impulseScale = Vector.ConditionalSelect(
                Vector.LessThan(Vector.Abs(impulseMagnitude), new Vector<Number>(Constants.C1em10)), 
                Vector<Number>.One, 
                Vector.Min(maximumImpulse / impulseMagnitude, Vector<Number>.One));
            Vector3Wide.Scale(accumulatedImpulse, impulseScale, out accumulatedImpulse);
            Vector3Wide.Subtract(accumulatedImpulse, previousAccumulatedImpulse, out csi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFirst(in ServoSettings source, ref ServoSettingsWide target)
        {
            GatherScatter.GetFirst(ref target.MaximumSpeed) = source.MaximumSpeed;
            GatherScatter.GetFirst(ref target.BaseSpeed) = source.BaseSpeed;
            GatherScatter.GetFirst(ref target.MaximumForce) = source.MaximumForce;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadFirst(in ServoSettingsWide source, out ServoSettings target)
        {
            target.MaximumSpeed = source.MaximumSpeed[0];
            target.BaseSpeed = source.BaseSpeed[0];
            target.MaximumForce = source.MaximumForce[0];
        }

    }
}
