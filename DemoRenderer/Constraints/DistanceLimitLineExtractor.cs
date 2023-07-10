using BepuPhysics;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Collections;
using BepuUtilities.Numerics;

namespace DemoRenderer.Constraints
{
    struct DistanceLimitLineExtractor : IConstraintLineExtractor<DistanceLimitPrestepData>
    {
        public int LinesPerConstraint => 5;

        public unsafe void ExtractLines(ref DistanceLimitPrestepData prestepBundle, int setIndex, int* bodyIndices,
            Bodies bodies, ref Vector3 tint, ref QuickList<LineInstance> lines)
        {
            //Could do bundles of constraints at a time, but eh.
            ref var poseA = ref bodies.Sets[setIndex].DynamicsState[bodyIndices[0]].Motion.Pose;
            ref var poseB = ref bodies.Sets[setIndex].DynamicsState[bodyIndices[1]].Motion.Pose;
            Vector3Wide.ReadFirst(prestepBundle.LocalOffsetA, out var localOffsetA);
            Vector3Wide.ReadFirst(prestepBundle.LocalOffsetB, out var localOffsetB);
            var minimumDistance = GatherScatter.GetFirst(ref prestepBundle.MinimumDistance);
            var maximumDistance = GatherScatter.GetFirst(ref prestepBundle.MaximumDistance);
            QuaternionEx.Transform(localOffsetA, poseA.Orientation, out var worldOffsetA);
            QuaternionEx.Transform(localOffsetB, poseB.Orientation, out var worldOffsetB);
            var endA = poseA.Position + worldOffsetA;
            var endB = poseB.Position + worldOffsetB;
            var color = new Vector3(Constants.C0p2, Constants.C0p2, Constants.C1) * tint;
            var packedColor = Helpers.PackColor(color);
            var backgroundColor = new Vector3(Constants.C0, Constants.C0, Constants.C1) * tint;
            lines.AllocateUnsafely() = new LineInstance(poseA.Position, endA, packedColor, 0);
            lines.AllocateUnsafely() = new LineInstance(poseB.Position, endB, packedColor, 0);
            var offset = endB - endA;
            var length = offset.Length();
            var direction = length < Constants.C1em9 ? new Vector3(1, 0, 0) : offset / length;
            var errorColor = new Vector3(1, 0, 0) * tint;
            var packedErrorColor = Helpers.PackColor(errorColor);
            var packedFarColor = Helpers.PackColor(color * Constants.C0p5);
            var packedNearColor = Helpers.PackColor(color * Constants.C0p25);
            var minimumPoint = endA + direction * minimumDistance;
            if (length >= minimumDistance && length <= maximumDistance)
            {
                //Create a darker bar to signify the minimum limit.
                lines.AllocateUnsafely() = new LineInstance(endA, minimumPoint, packedNearColor, 0);
                lines.AllocateUnsafely() = new LineInstance(minimumPoint, endB, packedFarColor, 0);
                lines.AllocateUnsafely() = new LineInstance(new Vector3(Number.MinValue), new Vector3(Number.MinValue), 0, 0);

            }
            else if (length < minimumDistance)
            {
                //Too close; draw an error line extending beyond the connecting line.
                lines.AllocateUnsafely() = new LineInstance(endA, endB, packedNearColor, 0);
                lines.AllocateUnsafely() = new LineInstance(endB, endA + direction * minimumDistance, packedErrorColor, 0);
                lines.AllocateUnsafely() = new LineInstance(new Vector3(Number.MinValue), new Vector3(Number.MinValue), 0, 0);
            }
            else
            {
                //Too far; draw an error line that extends from the desired endpoint to the current endpoint.
                var targetEnd = endA + direction * maximumDistance;
                lines.AllocateUnsafely() = new LineInstance(endA, minimumPoint, packedNearColor, 0);
                lines.AllocateUnsafely() = new LineInstance(minimumPoint, targetEnd, packedFarColor, 0);
                lines.AllocateUnsafely() = new LineInstance(targetEnd, endB, packedErrorColor, 0);
            }

        }
    }
}
