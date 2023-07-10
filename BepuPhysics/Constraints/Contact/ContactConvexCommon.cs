using BepuUtilities;
using BepuUtilities.Numerics;

namespace BepuPhysics.Constraints.Contact
{
    public struct ConvexContactWide
    {
        public Vector3Wide OffsetA;
        public Vector<Number> Depth;
    }

    public struct MaterialPropertiesWide
    {
        public Vector<Number> FrictionCoefficient;
        public SpringSettingsWide SpringSettings;
        public Vector<Number> MaximumRecoveryVelocity;
    }

    public interface IContactPrestep<TPrestep> where TPrestep : struct, IContactPrestep<TPrestep>
    {
        ref MaterialPropertiesWide GetMaterialProperties(ref TPrestep prestep);
        int ContactCount { get; }
        int BodyCount { get; }
    }


    public interface IConvexContactPrestep<TPrestep> : IContactPrestep<TPrestep> where TPrestep : struct, IConvexContactPrestep<TPrestep>
    {
        ref Vector3Wide GetNormal(ref TPrestep prestep);
        ref ConvexContactWide GetContact(ref TPrestep prestep, int index);

    }

    public interface ITwoBodyConvexContactPrestep<TPrestep> : IConvexContactPrestep<TPrestep> where TPrestep : struct, ITwoBodyConvexContactPrestep<TPrestep>
    {
        ref Vector3Wide GetOffsetB(ref TPrestep prestep);
    }

    public interface IContactAccumulatedImpulses<TAccumulatedImpulses> where TAccumulatedImpulses : struct, IContactAccumulatedImpulses<TAccumulatedImpulses>
    {
        int ContactCount { get; }
    }

    public interface IConvexContactAccumulatedImpulses<TAccumulatedImpulses> : IContactAccumulatedImpulses<TAccumulatedImpulses> where TAccumulatedImpulses : struct, IConvexContactAccumulatedImpulses<TAccumulatedImpulses>
    {
        ref Vector2Wide GetTangentFriction(ref TAccumulatedImpulses impulses);
        ref Vector<Number> GetTwistFriction(ref TAccumulatedImpulses impulses);
        ref Vector<Number> GetPenetrationImpulseForContact(ref TAccumulatedImpulses impulses, int index);
    }

}
