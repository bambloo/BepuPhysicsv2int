using BepuUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BepuPhysics.Constraints
{
    /// <summary>
    /// Defines some of the shared behavior across motor constraints.
    /// </summary>
    public struct MotorSettings
    {
        /// <summary>
        /// Maximum amount of force the motor can apply in one unit of time.
        /// </summary>
        public Number MaximumForce;
        /// <summary>
        /// Mass-scaled damping constant. If you want to simulate a viscous damping coefficient of D with an object of mass M, set this damping value to D / M.
        /// </summary>
        public Number Damping;

        /// <summary>
        /// Gets or sets how soft the constraint is. Values range from 0 to infinity. Softness is inverse damping; 0 is perfectly rigid, 1 is very soft, Number.MaxValue is effectively nonexistent.
        /// </summary>
        public Number Softness { get { return Constants.C1 / Damping; } set { Damping = value <= 0 ? Number.MaxValue : Constants.C1 / value; } }

        /// <summary>
        /// Checks if a settings instance has valid nonnegative values.
        /// </summary>
        /// <param name="settings">Instance to examine.</param>
        /// <returns>True if the settings are valid, false otherwise.</returns>
        public static bool Validate(in MotorSettings settings)
        {
            return ConstraintChecker.IsNonnegativeNumber(settings.MaximumForce) && ConstraintChecker.IsNonnegativeNumber(settings.Damping);
        }

        /// <summary>
        /// Defines settings for a motor constraint.
        /// </summary>
        /// <param name="maximumForce">Maximum amount of force the motor can apply in one unit of time.</param>
        /// <param name="softness">Gets or sets how soft the constraint is. Values range from 0 to infinity. Softness is inverse damping; 0 is perfectly rigid, 1 is very soft, Number.MaxValue is effectively nonexistent.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MotorSettings(Number maximumForce, Number softness) : this()
        {
            MaximumForce = maximumForce;
            Softness = softness;
            Debug.Assert(Validate(this), "Motor settings must have nonnegative maximum force and nonnegative damping.");
        }
    }
    public struct MotorSettingsWide
    {
        public Vector<Number> MaximumForce;
        public Vector<Number> Damping;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFirst(in MotorSettings source, ref MotorSettingsWide target)
        {
            GatherScatter.GetFirst(ref target.MaximumForce) = source.MaximumForce;
            GatherScatter.GetFirst(ref target.Damping) = source.Damping;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadFirst(in MotorSettingsWide source, out MotorSettings target)
        {
            target.MaximumForce = source.MaximumForce[0];
            target.Damping = source.Damping[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeSoftness(in MotorSettingsWide settings, Number dt, out Vector<Number> effectiveMassCFMScale, out Vector<Number> softnessImpulseScale, out Vector<Number> maximumImpulse)
        {
            //We can't use damping ratio for a velocity motor; there is no position goal, so there is no such thing as critical damping.
            //Instead, we start with the damping constant.
            //However, just as we use damping ratio and frequency to avoid dealing with mass tuning, the user tunes the damping constant *divided by mass*.
            //d = user damping constant, representing the predivided value
            //CFM = 1 / raw damping constant
            //CFM = (d * effectiveMass)^-1 = effectiveMass^-1 * d^-1
            //softenedEffectiveMass = (effectiveMass^-1 + CFM * dt^-1)^-1
            //softenedEffectiveMass = (effectiveMass^-1 + effectiveMass^-1 * (d * dt)^-1)^-1
            //softenedEffectiveMass = (effectiveMass^-1 * (1 + (d * dt)^-1))^-1
            //softenedEffectiveMass = (1 + (d * dt)^-1)^-1 * effectiveMass
            //softenedEffectiveMass = effectiveMass / (1 + 1 / (d * dt))

            //For the accumulated impulse scaling component:
            //impulse = bias * softenedEffectiveMass - accumulatedImpulse * CFM/dt * softenedEffectiveMass - wsv * JT * softenedEffectiveMass
            //Focusing on softness term:
            //CFM/dt * softenedEffectiveMass = (effectiveMass^-1 / (d * dt)) * effectiveMass / (1 + 1 / (d * dt))
            //CFM/dt * softenedEffectiveMass = effectiveMass^-1 * (effectiveMass / (1 + 1 / (d * dt))) / (d * dt)
            //CFM/dt * softenedEffectiveMass = 1 / (1 + 1 / (d * dt))) / (d * dt)
            //CFM/dt * softenedEffectiveMass = 1 / (d * dt + 1)
            //(For more, see the Inequality1DOF example constraint.)

            var dtWide = new Vector<Number>(dt);
            var dtd = dtWide * settings.Damping;
            maximumImpulse = settings.MaximumForce * dtWide;
            softnessImpulseScale = Vector<Number>.One / (dtd + Vector<Number>.One);
            effectiveMassCFMScale = dtd * softnessImpulseScale;
        }
    }
}
