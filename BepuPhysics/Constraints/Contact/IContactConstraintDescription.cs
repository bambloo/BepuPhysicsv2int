﻿using BepuPhysics.CollisionDetection;
using BepuUtilities.Numerics;

namespace BepuPhysics.Constraints.Contact
{
    public struct ConstraintContactData
    {
        public Vector3 OffsetA;
        public Number PenetrationDepth;
    }
    public interface IConvexOneBodyContactConstraintDescription<TDescription> : IOneBodyConstraintDescription<TDescription> 
        where TDescription : unmanaged, IConvexOneBodyContactConstraintDescription<TDescription>
    {
        void CopyManifoldWideProperties(ref Vector3 normal, ref PairMaterialProperties material);
        ref ConstraintContactData GetFirstContact(ref TDescription description);
    }
    public interface IConvexTwoBodyContactConstraintDescription<TDescription> : ITwoBodyConstraintDescription<TDescription> 
        where TDescription : unmanaged, IConvexTwoBodyContactConstraintDescription<TDescription>
    {
        void CopyManifoldWideProperties(ref Vector3 offsetB, ref Vector3 normal, ref PairMaterialProperties material);
        ref ConstraintContactData GetFirstContact(ref TDescription description);
    }

    public struct NonconvexConstraintContactData
    {
        public Vector3 OffsetA;
        public Vector3 Normal;
        public Number PenetrationDepth;
    }
    public struct NonconvexTwoBodyManifoldConstraintProperties
    {
        public Vector3 OffsetB;
        //Note that the positioning of the friction coefficient, spring settings, and maximum recovery velocity are used by the UnsafeManifoldViewer. Careful about moving these.
        public Number FrictionCoefficient;
        public SpringSettings SpringSettings;
        public Number MaximumRecoveryVelocity;
    }
    public struct NonconvexOneBodyManifoldConstraintProperties
    {
        //Note that the positioning of the friction coefficient, spring settings, and maximum recovery velocity are used by the UnsafeManifoldViewer. Careful about moving these.
        public Number FrictionCoefficient;
        public SpringSettings SpringSettings;
        public Number MaximumRecoveryVelocity;
    }
    public interface INonconvexOneBodyContactConstraintDescription<TDescription> : IOneBodyConstraintDescription<TDescription> 
        where TDescription : unmanaged, INonconvexOneBodyContactConstraintDescription<TDescription>
    {
        void CopyManifoldWideProperties(ref PairMaterialProperties material);
        int ContactCount { get; }

        ref NonconvexOneBodyManifoldConstraintProperties GetCommonProperties(ref TDescription description);
        ref NonconvexConstraintContactData GetFirstContact(ref TDescription description);
    }
    public interface INonconvexTwoBodyContactConstraintDescription<TDescription> : ITwoBodyConstraintDescription<TDescription> 
        where TDescription : unmanaged, INonconvexTwoBodyContactConstraintDescription<TDescription>
    {
        void CopyManifoldWideProperties(ref Vector3 offsetB, ref PairMaterialProperties material);
        int ContactCount { get; }

        ref NonconvexTwoBodyManifoldConstraintProperties GetCommonProperties(ref TDescription description);
        ref NonconvexConstraintContactData GetFirstContact(ref TDescription description);
    }

}
