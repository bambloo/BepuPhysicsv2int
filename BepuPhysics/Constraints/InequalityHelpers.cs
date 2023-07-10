using System;
using System.Collections.Generic;
using BepuUtilities.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BepuPhysics.Constraints
{
    public static class InequalityHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeBiasVelocity(Vector<Number> error, in Vector<Number> positionErrorToVelocity, Number inverseDt, out Vector<Number> biasVelocity)
        {
            biasVelocity = Vector.Min(error * inverseDt, error * positionErrorToVelocity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampPositive(ref Vector<Number> accumulatedImpulse, ref Vector<Number> impulse)
        {
            var previous = accumulatedImpulse;
            accumulatedImpulse = Vector.Max(Vector<Number>.Zero, accumulatedImpulse + impulse);
            impulse = accumulatedImpulse - previous;
        }
    }
}
