using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using BepuUtilities.Numerics;
using System;
using System.Runtime.CompilerServices;

namespace BepuPhysics.CollisionDetection.SweepTasks
{
    public class ConvexCompoundSweepTask<TShapeA, TShapeWideA, TCompound, TOverlapFinder> : SweepTask
        where TShapeA : unmanaged, IConvexShape
        where TShapeWideA : unmanaged, IShapeWide<TShapeA>
        where TCompound : unmanaged, ICompoundShape
        where TOverlapFinder : struct, IConvexCompoundSweepOverlapFinder<TShapeA, TCompound>
    {
        public ConvexCompoundSweepTask()
        {
            ShapeTypeIndexA = default(TShapeA).TypeId;
            ShapeTypeIndexB = default(TCompound).TypeId;
        }

        protected override unsafe bool PreorderedTypeSweep<TSweepFilter>(
            void* shapeDataA, Quaternion orientationA, in BodyVelocity velocityA,
            void* shapeDataB, Vector3 offsetB, Quaternion orientationB, in BodyVelocity velocityB,
            Number maximumT, Number minimumProgression, Number convergenceThreshold, int maximumIterationCount,
            bool flipRequired, ref TSweepFilter filter, Shapes shapes, SweepTaskRegistry sweepTasks, BufferPool pool, out Number t0, out Number t1, out Vector3 hitLocation, out Vector3 hitNormal)
        {
            ref var convex = ref Unsafe.AsRef<TShapeA>(shapeDataA);
            ref var compound = ref Unsafe.AsRef<TCompound>(shapeDataB);
            t0 = Number.MaxValue;
            t1 = Number.MaxValue;
            hitLocation = new Vector3();
            hitNormal = new Vector3();
            default(TOverlapFinder).FindOverlaps(ref convex, orientationA, velocityA, ref compound, offsetB, orientationB, velocityB, maximumT, shapes, pool, out var overlaps);
            for (int i = 0; i < overlaps.Count; ++i)
            {
                var compoundChildIndex = overlaps.Overlaps[i];
                if (filter.AllowTest(flipRequired ? compoundChildIndex : 0, flipRequired ? 0 : compoundChildIndex))
                {
                    ref var child = ref compound.GetChild(compoundChildIndex);
                    var childType = child.ShapeIndex.Type;
                    shapes[childType].GetShapeData(child.ShapeIndex.Index, out var childShapeData, out _);
                    var task = sweepTasks.GetTask(convex.TypeId, childType);
                    if (task != null && task.Sweep(
                        shapeDataA, convex.TypeId, new RigidPose() { Orientation = Quaternion.Identity }, orientationA, velocityA,
                        childShapeData, childType, CompoundChild.AsPose(ref child), offsetB, orientationB, velocityB,
                        maximumT, minimumProgression, convergenceThreshold, maximumIterationCount,
                        out var t0Candidate, out var t1Candidate, out var hitLocationCandidate, out var hitNormalCandidate))
                    {
                        //Note that we use t1 to determine whether to accept the new location. In other words, we're choosing to keep sweeps that have the earliest time of intersection.
                        //(t0 is *not* intersecting for any initially separated pair.)
                        if (t1Candidate < t1)
                        {
                            t0 = t0Candidate;
                            t1 = t1Candidate;
                            hitLocation = hitLocationCandidate;
                            hitNormal = hitNormalCandidate;
                        }
                    }
                }
            }
            overlaps.Dispose(pool);
            return t1 < Number.MaxValue;
        }

        protected override unsafe bool PreorderedTypeSweep(void* shapeDataA, in RigidPose localPoseA, Quaternion orientationA, in BodyVelocity velocityA, void* shapeDataB, in RigidPose localPoseB, Vector3 offsetB, Quaternion orientationB, in BodyVelocity velocityB, Number maximumT, Number minimumProgression, Number convergenceThreshold, int maximumIterationCount, out Number t0, out Number t1, out Vector3 hitLocation, out Vector3 hitNormal)
        {
            throw new NotImplementedException("Compounds cannot be nested; this should never be called.");
        }

    }
}
